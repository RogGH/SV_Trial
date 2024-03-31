using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 敵に設定するタイプ
public enum DropType
{
    ExpS,      // A（経験値小が多め）
    ExpM,      // B（経験値中が多め）   
    ExpL,      // C（経験値大が多め）   
    Tresure,    // 宝箱

    Num
};

// オブジェクト番号
public enum DropTblNo {
    ExpS,
    ExpM,
    ExpL,
    MoneyS,
    HealS,
    Tresure,

    Num
};


public class DropManager : MonoBehaviour
{
    private static DropManager ins;
    public static DropManager Ins
    {
        get
        {
            if (ins == null)
            {
                ins = (DropManager)FindObjectOfType(typeof(DropManager));
                if (ins == null) { Debug.LogError(typeof(DropManager) + "is nothing"); }
            }
            return ins;
        }
    }

    public GameObject[] ItemObjTbl;

    public const int MoneyRate = 10;
    public const int HealItemRate = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BootDropItem(Vector3 pos, DropType dType)
    {
        switch (dType) {
            case DropType.ExpS:
                {
                    // アイテム起動
                    Instantiate(ItemObjTbl[(int)DropTblNo.ExpS], pos, Quaternion.identity);
                    // その他ドロップは基本通り
                    otherItemDropDefault(pos);
                }
                break;

            case DropType.ExpM:
                {
                    // アイテム起動
                    Instantiate(ItemObjTbl[(int)DropTblNo.ExpM], pos, Quaternion.identity);

                    // その他ドロップは基本通り
                    otherItemDropDefault(pos);
                }
                break;

            case DropType.ExpL:
                {
                    // アイテム起動
                    Instantiate(ItemObjTbl[(int)DropTblNo.ExpL], pos, Quaternion.identity);
                    // その他ドロップは基本通り
                    otherItemDropDefault(pos);
                }
                break;

            case DropType.Tresure:
                {
                    // アイテム起動
                    Instantiate(ItemObjTbl[(int)DropTblNo.Tresure], pos, Quaternion.identity);
                }
                break;
        }
    }

    // その他アイテムドロップ
    void otherItemDropDefault(Vector3 pos) {
        // ランダムで追加で落とすアイテムを設定
        Player plScr = StageManager.Ins.PlScr;

        int luck = plScr.GetCalcLuck();
        float calcRate = luck / 100;

        int rand = Random.Range(0, 100);
        int moneyDrop = Mathf.RoundToInt(MoneyRate * calcRate);
        int healDrop = moneyDrop + Mathf.RoundToInt(HealItemRate * calcRate);
        if (rand < moneyDrop)
        {
            // 金を落とすかチェック
            pos.x += Random.Range(-3.0f, 3.0f) * 10;
            Instantiate(ItemObjTbl[(int)DropTblNo.MoneyS], pos, Quaternion.identity);
        }
        else if (rand < healDrop)
        {
            // 回復アイテムも落とす
            pos.x += Random.Range(-3.0f, 3.0f) * 10;
            Instantiate(ItemObjTbl[(int)DropTblNo.HealS], pos, Quaternion.identity);
        }
    }
}
