using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HitResult;

public class HitData : MonoBehaviour
{
    HitType type;           // タイプ：敵、PL
    public HitType Type { get { return type; } set { type = value; } }

    HitLayer layer;         // レイヤー：攻撃、防御
    public HitLayer Layer { get { return layer; } set { layer = value; } }

    // 接触中の処理
    private void OnTriggerStay2D(Collider2D collision)
    {
        // ①から⑥までのチェック
        HitData chkData = collision.gameObject.GetComponent<HitData>();
        if (!CheckCmmHit(chkData)) { return; }

        // データ取り出し
        HitBase atkHB = this.transform.root.gameObject.GetComponent<HitBase>();
        HitBase defHB = chkData.transform.root.GetComponent<HitBase>();
        HitAtkData atkData = this.GetComponent<HitAtkData>();
        HitDefData defData = chkData.GetComponent<HitDefData>();
        // ⑦攻撃している側が既に同じオブジェクトを攻撃しているかチェック
        if (!AtkObjCheck(atkHB, atkData, defHB, defData)) { return; }

        // ⑧攻撃時成立！！（ようやく）
        AtkFunc(atkHB, atkData, defHB, defData);
    }

    // 接触終了の処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ①から⑥までのチェック
        HitData chkData = collision.gameObject.GetComponent<HitData>();
        if (!CheckCmmHit(chkData)) { return; }


        // bodyタイプは一度接触を外れたらもう一度当たるので、それのチェック
        // 攻撃データの種類をチェック
        HitAtkData atk = this.GetComponent<HitAtkData>();
        // bodyタイプ以外は何もしない
        if (atk.AtkKind != HitAtkKind.Body) { return; }
        // ヒット数が残っているかチェック
        if (atk.AtkList.Count <= 0){ return; }
        // 攻撃データチェック
        foreach (HitAtkData.EntryData entryData in atk.AtkList)
        {
            // 攻撃データが登録されていたら外す
            if (entryData.Obj == collision.gameObject)
            {
                atk.AtkList.Remove(entryData);
                break;
            }
        }
    }

    // 同じ相手を攻撃したときの処理
    bool AtkObjCheck(HitBase atkHB, HitAtkData atkData, HitBase defHB, HitDefData defData) {

        // リザルト設定
        atkHB.Result.SetAtkFlag(AtkFlag.AtkHit);
        defHB.Result.SetDefFlag(DefFlag.DefHit);

        // 攻撃リストの数があるかチェック
        if (atkData.AtkList.Count > 0) {
            // 攻撃リストチェック
            foreach (HitAtkData.EntryData chkData in atkData.AtkList) {
                // nullチェック
                if (chkData == null) {
                    atkData.AtkList.Remove(chkData);
                    continue;
                }

                // 既にリストに登録されているかチェック
                if (chkData.Obj.GetInstanceID() != defData.gameObject.GetInstanceID()) { continue; }

                // 攻撃したオブジェクトのヒットが無効ならリスト外す
                if (!defHB.DefActive ) {                    
                    atkData.AtkList.Remove(chkData);
                    return false;
                }

                // ヒット数超過していたら終了
                if (chkData.HitNum >= atkData.HitNum) { return false; }
                // コンボインターバルを計算し、コンボ時間が時間が過ぎていなければ終了
                chkData.ComboCount -= Time.deltaTime;
                if (chkData.ComboCount > 0) { return false; }

                chkData.HitNum++;                                       // ヒット数加算           
                chkData.ComboCount = HitDefine.ComboInterval;           // インターバル設定 
                return true;
            }
        }
        else {
            // 攻撃したオブジェクトのヒットが無効ならこれ以上行わない
            if (!defHB.DefActive )
            {
                return false;
            }
        }


        // リストに設定
        atkData.AtkList.Add(new HitAtkData.EntryData(defData.gameObject, 1));
        return true;
    }

    // 攻撃をした側の処理
    private void AtkFunc(HitBase atkHB, HitAtkData atkData, HitBase defHB, HitDefData defData) {
        // 判定が行われたので、HitBaseのHP計算処理
        AtkDamageCalc(atkHB, atkData, defHB, defData);

        // 攻撃関数が登録されていたら実行
        if (atkHB.AtkReactFunc != null) { atkHB.AtkReactFunc(atkData, defData); }
    }

    // ダメージ計算処理
    void AtkDamageCalc(HitBase atkHB, HitAtkData atk, HitBase defHB, HitDefData def)
    {
        AtkFlag setAtkResult = AtkFlag.AtkHit;
        DefFlag setDefResult = DefFlag.DefHit;

        // ダメージ計算
        int value = atk.AtkPow;

        // 防御力も加味
        int defPow = def.DefPow;

        // 接触した座標の中間位置を計算（だいたいの座標）
        CalcAtkCenterPos(atk, defHB, def);

        // 属性とかの計算（特になし）
        if (value > 0)
        {
            // 無敵中かチェック
            if (def.DefKind == HitDefKind.Invincivle)
            {
                // 無敵状態に殴った
                setAtkResult |= AtkFlag.AtkDoneInvincible;
                // 無敵状態なので、ダメージ計算は行わない（ヒットはしている）
                setDefResult |= DefFlag.DefDoneInvincible;
            }
            // 回避中かチェック
            else
            if (def.DefKind == HitDefKind.Avoid)
            {
                // 回避状態に殴った
                setAtkResult |= AtkFlag.AtkDoneAboid;
                // 回避状態なので、ダメージ計算は行わない（ヒットエフェクトとか出さないとか）
                setDefResult |= DefFlag.DefDoneAvoid;
            }
            // 通常ダメージ
            else
            {
                if (def.DefKind == HitDefKind.CheckOnly)
                {
                    // チェック判定
                    // チェック判定に攻撃をした
                    setAtkResult |= AtkFlag.AtkDoneCheck;
                    // チェック判定で防御した
                    setDefResult |= DefFlag.DefDoneCheck;
                }
                else {
                    // ダメージ判定
                    // クリティカル率から計算
                    if (atk.CriticalHit > 0) {
                        int rand = Random.Range(0, 100);
                        if (rand <= atk.CriticalHit)
                        {
                            // クリティカル発生
                            // ダメージ計算（1.5＋武器事のクリティカルダメージアップ）
                            float criDmgRate = HitDefine.CriDmgRateDef + (atk.CriticalDmg/100);
                            // 
                            value = Mathf.RoundToInt(value * criDmgRate);

                            setAtkResult |= AtkFlag.AtkDoneCritical;
                        }
                    }

                    // 防御力計算
                    value -= defPow;
                    if (value < 1) { value = 1; }            // 最低保証は１

                    // HPを減らす
                    defHB.HP -= value;
                    if (defHB.HP < 0) { defHB.HP = 0; }      // 死亡したら０

                    // 攻撃をした
                    setAtkResult |= AtkFlag.AtkDoneDamage;
                    // ダメージを受けた
                    setDefResult |= DefFlag.DefDoneDamage;

                    // ヒットテキスト表示
                    string setText = value.ToString();
                    GameObject obj =
                        Instantiate((GameObject)Resources.Load("HitDispText"), defHB.Result.HitPos, Quaternion.identity);
                    Color setCol = Color.white;
                    if ( def.Layer == HitLayer.Player) {setCol = Color.red; }

                    bool criticalFlag = (setAtkResult & AtkFlag.AtkDoneCritical) != 0 ? true : false;
                    if (criticalFlag)
                    {
                        setCol = Color.yellow;
                        setText += "!"; 
                    }
                    obj.GetComponent<HitDispText>().SetString(setText, setCol);

                    // ダメージ計算
                    DmgCalcSys.Ins.AddTotalDmg(value);

                }
            }
        }
        else if (value < 0)
        {
            // 回復をした
            setAtkResult = AtkFlag.AtkDoneDamage;
            // 回復
            defHB.HP += value;
            if (defHB.HP > defHB.MaxHP)
            {
                defHB.HP = defHB.MaxHP;                 // 最大HPを超えない
            }
            // ダメージを受けた
            setDefResult |= DefFlag.DefDoneHeal;
        }
        else {
            // ノーダメージ
            setDefResult |= DefFlag.DefDoneNoDamage;
        }

        // 攻撃側
        {
            // 攻撃した
            atkHB.Result.SetAtkFlag(setAtkResult);
            // SEを鳴らしてみる
            if (atk.layer == HitLayer.Player) {
                string seName;
                if (atkHB.Result.CheckAtkFlag(AtkFlag.AtkDoneCritical)) {
                    seName = "Hit1";
                }
                else
                {
                    seName = "Hit2";
                }
                // 再生数は５までにしとく
                SeManager.Instance.Play(seName, 5);
            }
        }

        // 防御側
        {
            // ダメージ
            defHB.Result.Damage = atk.AtkPow;
            // リザルト設定
            defHB.Result.SetDefFlag(setDefResult);
        }
    }

    // 接触座標の計算
    void CalcAtkCenterPos(HitAtkData atk, HitBase defHB, HitDefData def)
    {
        // コライダー同士の中間位置を計算（Y座標を合わせる）
        Collider2D atkCol = def.GetComponent<Collider2D>();
        Vector3 atkPos = atk.transform.position;
        atkPos.y += atkCol.offset.y;

        Collider2D defCol = def.GetComponent<Collider2D>();
        Vector3 defPos = def.transform.position;
        defPos.y += defCol.offset.y;

        // X幅がクソ長いアタリ判定の場合、座標がおかしくなるので、
        // X座標を防御側の端っこに合わせる
        // なお、Y座標が長いアタリ判定は未対応
        Vector3 calcPos = Vector3.Lerp(atkPos, defPos, 0.5f);
        float width = defCol.bounds.size.x / 2;
        if (defPos.x - width > calcPos.x || defPos.x + width < calcPos.x)
        {
            float dir = calcPos.x < defPos.x ? -1 : 1;
            calcPos.x = defPos.x + width * dir;
        }

        // 接触した座標同士の中間地点を保存
        defHB.Result.HitPos = calcPos;
    }

    // 共通チェック
    private bool CheckCmmHit(HitData chkData) {
        // ①接触した相手もHitDataを持っているかチェック
        if (!chkData) { return false; }

        // 攻撃側VS防御の形のみ行う
        // ②自身が攻撃かチェック
        if (this.Type != HitType.ATK) { return false; }
        // ③攻撃側のデータが設定されているかチェック
        HitBase atkHB = this.transform.root.gameObject.GetComponent<HitBase>();
        if (atkHB == null) { Debug.Log(this + "atk HitBase is None"); }
        if (!atkHB.AtkActive) { return false; }


        // ④接触した相手が防御かチェック
        if (chkData.Type != HitType.DEF) { return false; }
        // ⑤防御側のデータが設定されているかチェック
        HitBase defHB = chkData.transform.root.GetComponent<HitBase>();
        if (defHB == null) { Debug.Log(chkData + "target HitBase is None"); return false; }

        // ⑥レイヤーでの当たり判定チェック
        if (!AtkLayerHitCheck(this.Layer, chkData.Layer)) { return false; }

        // ⑦現状HP０なら判定しなくする
        if (atkHB.HP <= 0 || defHB.HP <= 0) { return false; }


        return true;
    }

    // レイヤーによる攻撃判定チェック
    bool AtkLayerHitCheck(HitLayer aktLayer, HitLayer defLayer)
    {
        switch (aktLayer)
        {
            // PL攻撃 VS EM防御または中立防御
            case HitLayer.Player:
                if (defLayer == HitLayer.Enemy || defLayer == HitLayer.Neutral) { return true; }
                break;
            // EM攻撃 VS PL防御または中立防御
            case HitLayer.Enemy:
                if (defLayer == HitLayer.Player || defLayer == HitLayer.Neutral) { return true; }
                break;
            // 中立攻撃 VS PL防御または敵防御
            case HitLayer.Neutral:
                if (defLayer == HitLayer.Player || defLayer == HitLayer.Enemy) { return true; }
                break;
        }
        return false;
    }
}