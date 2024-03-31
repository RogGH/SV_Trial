using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAtkData : HitData
{
    // 親設定
    [SerializeField] HitLayer atkLayer;                     // レイヤー
    // 攻撃専用
    [SerializeField] int atkPow = 1;                        // 攻撃力（攻撃力）
    [SerializeField] int criHitRate = 0;                    // クリティカル率（０～１００％）
    [SerializeField] int criDmgRate = 0;                    // クリティカル加算ダメージ（％）
    [SerializeField] HitElement atkEle = HitElement.None;   // 属性（計算用属性）
    [SerializeField] HitAtkKind atkKind = HitAtkKind.Body;  // 種類（接触が外れたらどうなるか）
    [SerializeField] HitAtkSP   atkSP = HitAtkSP.None;      // 付加（特殊なタイプ）
    [SerializeField] int hitNum = 1;                        // ヒット数
    [SerializeField] HitShape atkShape = HitShape.Rect;     // 形状（現状矩形のみ）

    List<EntryData> atkList = new List<EntryData>();

    // ゲッタ
    public int AtkPow { get { return atkPow; } set { atkPow = value; } }
    public int CriticalHit { get { return criHitRate; } set { criHitRate = value; } }
    public int CriticalDmg { get { return criDmgRate; } set { criDmgRate = value; } }
    public HitElement AtkEle { get { return atkEle; } }
    public HitAtkKind AtkKind { get { return atkKind; } }
    public HitAtkSP AtkSP { get { return atkSP; } }
    public int HitNum { get { return hitNum; } }
    public List<EntryData> AtkList { get { return atkList; } set { atkList = value; } }

    BoxCollider2D box;
    HitBase hb;

    // エントリーデータ
    public class EntryData {
        GameObject obj;
        int hitNum;
        float comboCount;
        public EntryData(GameObject obj, int hitNum){
            this.obj = obj;
            this.hitNum = hitNum;
            this.comboCount = HitDefine.ComboInterval;
        }
        public GameObject Obj { get { return obj; } }
        public int HitNum { get { return hitNum; } set { hitNum = value; } }
        public float ComboCount { get { return comboCount; } set { comboCount = value; } }
    };

    void Awake() {
        // 親設定
        this.Layer = atkLayer;
        this.Type = HitType.ATK;
        // 攻撃のみ
        atkList.Clear();

#pragma warning disable CS0162
        if (HitDefine.DebugDisp) {
            box = GetComponent<BoxCollider2D>();
        }
#pragma warning disable CS0162
        hb = HitDefine.getHitBase(gameObject);
    }

    void Update(){
        // 自身が無効かチェック
        if ( hb != null ) {
            if (!hb.AtkActive) {
                if (AtkList.Count > 0) {
                    AtkList.Clear();
                }
            }
        }

        // 消すオブジェクトをリスト化
        if (AtkList.Count > 0) 
        {
            // オブジェクト死亡チェック
            List<EntryData> temp = new List<EntryData>();
            foreach (var data in AtkList)
            {
                if (data.Obj == null)
                {
                    // 消すリストへ登録
                    temp.Add(data);
                }
            }

            // 消すリストがあるかチェック
            if (temp.Count > 0)
            {
                foreach (var data in temp)
                {
                    // 攻撃リストから消去
                    AtkList.Remove(data);
                }
            }
        }

#pragma warning disable CS0162
        // 表示
        if (HitDefine.DebugDisp) {
            if (hb.AtkActive){
                if (atkShape == HitShape.Rect){
                    HitDefine.RectDisp(
                        transform.root.localScale.x,
                        box.offset,
                        box.size * transform.root.localScale,
                        transform.root.position,
                        Color.red);
                }
            }
        }
#pragma warning disable CS0162
    }
}
