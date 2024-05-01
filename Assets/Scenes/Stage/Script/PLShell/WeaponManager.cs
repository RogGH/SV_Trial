using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 武器＞LV4で強く、LV7で超強くなるようにするか
// 
// DPSチェック
// ハンドガンLV7　     150
// ドリルLV7           100
// のこぎりLV7         100      
// 火炎LV7             100
// タレットLV7          85
// 波動LV7             100
//
// 理想装備（攻撃、クリ、短縮は基本
// HG     1000  (弾数ビルドがヤバすぎる）
// どり   350     
// のこ   500
// 火炎   550   (弾数ありでコレか…)
// タレ   400
// 波動   300

#if false
// 武器のレベルアップによる変化
public class WeaponPara {
    int shotNum = 1;                // 弾数
    float atkRate = 1.0f;           // 攻撃力レート（1が等倍）
    int criHitRate = 0;             // クリティカル（％指定）
    int criDmgRate = 0;             // クリティカルダメージ（％指定）
    float recastRate = 1.0f;        // 0から1.0fを入れるっぽい（0.5だと50%短縮っぽい）
    float areaRate = 1.0f;          // 1.0f以上の倍率指定するっぽい
    float atkTimeRate = 1.0f;       // 攻撃時間レート
    float moveSpdRate = 1.0f;       // 弾速
};
#endif

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager ins;
    public static WeaponManager Ins
    {
        get
        {
            if (ins == null)
            {
                ins = (WeaponManager)FindObjectOfType(typeof(WeaponManager));
                if (ins == null) { Debug.LogError(typeof(WeaponManager) + "is nothing"); }
            }
            return ins;
        }
    }

    public GameObject[] WeaponObjTbl;                   // 起動用ゲームオブジェクト

    public WeaponInitSOBJDataTable initDataTable;
    public WeaponLVUPSOBJDataTable lvupDataTable;

    public WeaponInitSOBJDataTable InitTable { get { return initDataTable; } }
    public WeaponLVUPSOBJDataTable LVUPTable{ get { return lvupDataTable; } }

    public GunSOBJ gunSobj;

    private void Awake()
    {
        Debug.Assert(!(initDataTable == null), "initDataTable is null");
        Debug.Assert(!(lvupDataTable == null), "lvupDataTable is null");
    }


    // リキャストデフォルト時間取得
    public float GetWeaponRecastDef(int sNo)
    {
        return initDataTable.data[sNo].recastDef;
    }
    public float GetWeaponRecastMin(int sNo)
    {
        return initDataTable.data[sNo].recastMin;
    }
    public float GetWeaponRecastCalc(int sNo) {
        return GetWeaponRecastDef(sNo) - GetWeaponRecastMin(sNo);
    }
}
