using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyCmm : MonoBehaviour
{
    public enum MoveType
    {
        HomingToPL,         // PLへ誘導
        StraightToPL,       // PLへ直進
        LToR,               // 左から右
        RToL,               // 右から左
        UToD,               // 上から下
        DToU,               // 下から上
        None,               // 移動無
    };
    public enum AtkType
    {
        MoveOnly,           // 攻撃無し
        Bomb,               // 爆発（自爆）
        GazeSector,         // 視線（扇）
        Explosion,          // 爆発（定期的）
        BossTitan,          // ボス（タイタン専用行動）
        LandSlide,          // ランスラ（定期的）
        Jail,               // ジェイル（自爆）
    };

    // 外部設定
    public Material dieMat;
    [SerializeField] int defaultDir = -1;
    public MoveType moveType = MoveType.HomingToPL;
    [SerializeField] AtkType atkType = AtkType.MoveOnly;
    [SerializeField] float escapeTime = 60;
    // 外部設定（拡張)
    public GameObject ShellObj;
    public GameObject[] BossShellTbl;
    // コンポーネント
    GameObject plObj;
    SpriteRenderer spCmp;
    HitBase hb;
    // 基本パラメータ
    bool dieFlag = false;
    int emTblNo;
    DropType dType;
    Vector3 oldPos;
    Vector3 moveVec;
    float moveSpd = 1.0f * 60;
    float scaleSpd = 8 * 60;
    float scaleRate = 0.04f;
    Vector3 defScale;
    float scaleTimer = 0;
    int moveRno = 0;
    bool escapeFlag = false;
    //int nowLV = 0;

    // 攻撃関連
    int actRno = 0;
    int atkRno = 0;
    float atkCounter = 0;
    int atkIctr0 = 0;
    int atkIctr1 = 0;
    int atkIctr2 = 0;
    bool nearFlag = false;

    // 爆弾関連
    const float nearDistance = 100.0f;
    const float BombCount = 2.5f;
    Vector3 BombEffOfs = new Vector3(0, 20, 0);
    // 目線攻撃関連
    const float GazeCount = 6.0f;
    const float GazeSignCount = 3.0f;
    Vector3 GazeOfs = new Vector3(0, 80, 0);
    // 爆発攻撃関連
    const float ExplodeCount = 5.0f;
    const float ExplodeSignCount = 2.5f;
    // ランスラ関連
    const float LandSlideCount = 5.0f;
    const float LandSlideSignCount = 2.0f;
    // ジェイル関連
    const float JailCount = 5.0f;

    // タイタン用
    enum TitanActNo
    {
        Crash,      // ジオクラッシュ
        Slide,      // ランスラ
        Pressure,   // 重み
        Sammon,     // 雑魚召喚
        BombCircle, // 爆弾（円）
        BombLine,   // 爆弾（三列）
        BombCross,  // 爆弾（十字）
        Num
    };
    delegate void dFunc();
    private dFunc[] titanFTbl;
    Vector3 CrashUpSpd = new Vector3(0, 60.0f * 60.0f, 0);
    Vector3 CrashDownSpd = new Vector3(0, -70.0f * 60.0f, 0);
    Vector3 targetPos;
    float targetDeg;
    int actTblNo = 0;

#if false
    // 
    public void SetCharactor(int tNo, int lv)
    {
        EnemyData eData = GetComponent<EnemyData>();

        eData.lv = lv;
        nowLV = lv;

        // データから取得
        moveSpd = eData.GetMoveSpd();
        dType = eData.GetDropType();
        spCmp.sprite = eData.GetSpriteNo();
        hb.HP = hb.MaxHP = eData.GetHP();

        // 
        GetComponentInChildren<HitAtkData>().AtkPow = eData.GetAtkPow();
        GetComponentInChildren<HitDefData>().DefPow = eData.GetDefPow();

        // 
        emTblNo = tNo;
    }
#endif

    public void SetSOBJData(EnemySOBJData data) {
        string path = "EnemySpriteP/";
        path += data.sprite;
        Sprite sprite = Resources.Load<Sprite>(path);
        GetComponent<SpriteRenderer>().sprite = sprite;

        emTblNo = data.id;
        moveSpd = data.speed;
        dType = data.dropType;

        hb.HP = hb.MaxHP = data.hp;
        GetComponentInChildren<HitAtkData>().AtkPow = data.attack;
        GetComponentInChildren<HitDefData>().DefPow = data.defense;
    }


    void Awake()
	{
        hb = GetComponent<HitBase>();
        spCmp = GetComponent<SpriteRenderer>();
        defScale = transform.localScale;    // 向き

        // ボス用
        titanFTbl = new dFunc[(int)TitanActNo.Num];
        titanFTbl[0] = atkCrash;
        titanFTbl[1] = atkSlide;
        titanFTbl[2] = atkPressure;
        titanFTbl[3] = atkSammon;
        titanFTbl[4] = atkBombCircle;
        titanFTbl[5] = atkBombLine;
        titanFTbl[6] = atkBombCross;

        // 攻撃用カウンタとか初期化
        switch (atkType)
        {
            case AtkType.Bomb:
                atkCounter = BombCount;
                break;
            case AtkType.GazeSector:
                atkCounter = GazeCount;
                break;
            case AtkType.BossTitan:
                TitanSetNextAct();
                break;
            case AtkType.LandSlide:
                atkCounter = LandSlideCount;
                break;
            case AtkType.Jail:
                atkCounter = JailCount;
                atkRno = 0;
                moveVec = CrashDownSpd;
                break;
        }
    }

    void Start()
    {
        // PL取得
        plObj = StageManager.Ins.PlObj;

        // とりあえず木人だけ位置固定させる
        if (emTblNo == (int)EnemySOBJEnum.WoodA)
        {
            transform.position = new Vector3(90, 40, 0);
        }
    }

    void Update()
    {
        if (StageManager.Ins.CheckStop()) { return; }

        if (StageManager.Ins.CheckForceEscape()) { escapeFlag = true; }

        // 強制終了タイマー
        escapeTime -= Time.deltaTime;
        if (escapeTime <= 0)
        {
            escapeFlag = true;
        }

        hb.PreUpdate();
        if (hb.CheckDie() || escapeFlag)
        {
            if (emTblNo != (int)EnemySOBJEnum.WoodA)
            {
                if (dieFlag == false)
                {
                    StartCoroutine("Die");
                    dieFlag = true;
                }
                return;
            }
        }
        else if (hb.CheckDamage())
        {
            StartCoroutine("Damage");
        }


        // 移動
        moveControl();

        // 現在の座標から、表示優先を決定させてみる
        spCmp.sortingOrder = IObject.GetSpriteOrder(transform.position);

        // 攻撃制御
        atkControl();

        // 拡大縮小設定
        {
            scaleTimer += Time.deltaTime * scaleSpd;

            Vector3 scale = transform.localScale;
            float rad = Mathf.Deg2Rad * scaleTimer;

            scale.x = defScale.x + scaleRate * Mathf.Sin(rad);
            scale.y = defScale.y + scaleRate * Mathf.Sin(rad);

            // 移動した方向を計算
            int moveDir = transform.position.x < oldPos.x ? -1 : 1;
            // 移動方向によって向きが変わる
            scale.x *= defaultDir * moveDir;

            transform.localScale = scale;
        }


        hb.PostUpdate();
    }


    // 移動
    void moveControl()
    {
        oldPos = transform.position;

        switch (moveType)
        {
            case MoveType.HomingToPL:
                if (plObj == null) { return; }
                // PL方向へ向かう
                EnemyMoveControl();
                break;

            case MoveType.StraightToPL:
                if (moveRno == 0)
                {
                    float radian =
                        Mathf.Atan2(plObj.transform.position.y - transform.position.y,
                                    plObj.transform.position.x - transform.position.x);
                    moveVec = Vector3.zero;
                    moveVec.x = moveSpd * Mathf.Cos(radian);
                    moveVec.y = moveSpd * Mathf.Sin(radian);
                    ++moveRno;
                }
                transform.position += moveVec * Time.deltaTime;
                break;

            case MoveType.LToR:
            case MoveType.RToL:
                if (moveRno == 0)
                {
                    moveVec.x = moveType == MoveType.LToR ? moveSpd : -moveSpd;
                    moveVec.y = 0;
                    ++moveRno;
                }
                transform.position += moveVec * Time.deltaTime;
                break;

            case MoveType.UToD:
            case MoveType.DToU:
                if (moveRno == 0)
                {
                    moveVec.x = 0;
                    moveVec.y = moveType == MoveType.UToD ? -moveSpd : moveSpd;
                    ++moveRno;
                }
                transform.position += moveVec * Time.deltaTime;
                break;

            case MoveType.None:
                break;
        }
    }

    // 攻撃
    void atkControl()
    {
        switch (atkType)
        {
            case AtkType.MoveOnly:
                break;

            case AtkType.Bomb:
                // PLに接近したかチェック
                if (!nearFlag)
                {
                    float dist = Vector2.Distance(plObj.transform.position, transform.position);
                    if (dist <= nearDistance)
                    {
                        nearFlag = true;
                        atkCounter = BombCount + 0.1f;
                        // 予兆を起動
                        GameObject signObj = EffecgtManager.Ins.BootEffect(EffectNo.AtkSign);
                        // 予兆側に攻撃もセット
                        signObj.GetComponent<AtkSign>().
                            SetUp(gameObject,
                            AtkSign.SignType.Circle,
                            BombEffOfs,
                            150,
                            BombCount,
                            ShellObj);
                        break;
                    }
                }

                // 爆発
                if (nearFlag)
                {
                    atkCounter -= Time.deltaTime;
                    if (atkCounter <= 0)
                    {
                        // 爆発して死ぬ
                        Destroy(gameObject);
                    }
                }
                break;

            case AtkType.GazeSector:
                // 視線攻撃
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    atkCounter = GazeCount + GazeSignCount;
                    // 視線起動
                    GameObject signObj = EffecgtManager.Ins.BootEffect(EffectNo.AtkSign);
                    // 予兆側に攻撃もセット
                    signObj.GetComponent<AtkSign>().
                        SetUp(gameObject,
                        AtkSign.SignType.Sec45,
                        GazeOfs,
                        520,
                        GazeSignCount,
                        ShellObj,
                        AtkSign.ShellType.Gaze);
                }
                break;

            case AtkType.Explosion:
                // 爆発
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    atkCounter = ExplodeCount + ExplodeSignCount;
                    // 視線起動
                    GameObject signObj = EffecgtManager.Ins.BootEffect(EffectNo.AtkSign);
                    // 予兆側に攻撃もセット
                    signObj.GetComponent<AtkSign>().
                        SetUp(gameObject,
                            AtkSign.SignType.Circle,
                        BombEffOfs,
                        400,
                        ExplodeSignCount,
                        ShellObj,
                        AtkSign.ShellType.Bomb);
                }
                break;

            case AtkType.BossTitan:
                // タイタン
                atkTitanFunc();
                break;

            case AtkType.LandSlide:
                if (atkRno == 0)
                {
                    atkCounter -= Time.deltaTime;
                    if (atkCounter <= 0)
                    {
                        // 予兆を起動 
                        float radian = Mathf.Atan2(plObj.transform.position.y - transform.position.y,
                           plObj.transform.position.x - transform.position.x);
                        targetDeg = radian * Mathf.Rad2Deg;
                        {
                            GameObject obj = EffecgtManager.Ins.BootEffect(EffectNo.AtkSign, targetPos);
                            obj.GetComponent<AtkSign>().SetUpRect(gameObject, 1200, LandSlideSignCount, targetDeg);
                        }
                        atkCounter = LandSlideSignCount;
                        atkRno = 1;
                    }
                }
                else
                {
                    atkCounter -= Time.deltaTime;
                    if (atkCounter <= 0)
                    {
                        Vector3 bootPos = transform.position;
                        bootPos.y += 60.0f;
                        {
                            GameObject obj = Instantiate(ShellObj, bootPos, Quaternion.identity);
                            obj.GetComponent<EShellBooter>().SetUp(targetDeg);
                        }
                        atkCounter = LandSlideCount;
                        atkRno = 0;
                    }
                }
                break;

            case AtkType.Jail:
                // ジェイル
                if (atkRno == 0)
                {
                    transform.position += moveVec * Time.deltaTime;
                    if (transform.position.y <= targetPos.y)
                    {
                        SeManager.Instance.Play("Sand");
                        transform.position = targetPos;
                        ++atkRno;
                    }
                }
                else if (atkRno == 1)
                {
                    atkCounter -= Time.deltaTime;
                    if (atkCounter <= 0)
                    {
                        GameObject bombObj = Instantiate(ShellObj, transform.position, Quaternion.identity);
                        bombObj.GetComponent<EShellBomb>().SetUp(300);
                        // 爆発して死ぬ
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }

    // ダメージ
    IEnumerator Damage()
    {
        spCmp.material.color = Color.white;

        yield return new WaitForSeconds(0.05f);

        spCmp.material.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }

    // 死亡
    IEnumerator Die()
    {
        hb.SetAtkActive(false);
        hb.SetDefActive(false);

        // 強制終了の時は行わない
        if (!escapeFlag)
        {
            DropItemControl();
            StageManager.Ins.PlScr.KillNum++;
        }
        GetComponent<BoxCollider2D>().enabled = false;

        float counter = 1.0f;
        float strengthY = 0;

        spCmp.material = dieMat;

        if (atkType == AtkType.BossTitan)
        {
            BgmManager.Instance.StopImmediately();
            SeManager.Instance.Play("Bomb1");
        }

        do
        {
            spCmp.material.SetFloat("_Alpha", counter);
            spCmp.material.SetFloat("_StrengthY", strengthY);

            counter -= Time.deltaTime * 2;
            strengthY -= Time.deltaTime * 2;

            yield return new WaitForSeconds(Time.deltaTime);
        } while (counter > 0);

        // タイタン倒したら終了
        if (atkType == AtkType.BossTitan)
        {
            StageManager.Ins.SetClear();
        }

        Destroy(gameObject);
    }

    // ジェイル用セットアップ
    public void JailSetUp()
    {
        Vector3 pos = transform.position;
        targetPos = pos;
        pos.y += JailOfsY;
        transform.position = pos;
    }
}
