using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager ins;
    public static EnemyManager Ins
    {
        get
        {
            if (ins == null)
            {
                ins = (EnemyManager)FindObjectOfType(typeof(EnemyManager));
                if (ins == null) { Debug.LogError(typeof(EnemyManager) + "is nothing"); }
            }
            return ins;
        }
    }

    public EnemySOBJDataTable table;

    private void Awake()
    {
        Debug.Assert(!(table == null), "em table is null");
    }

    // スクリプタブルオブジェクトを取得
    public EnemySOBJData GetEnemySOBJData(int sobjNo) {
        EnemySOBJData obj = table.data[sobjNo];
        return obj;
    }
}
