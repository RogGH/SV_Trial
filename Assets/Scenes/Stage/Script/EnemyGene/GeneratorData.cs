using UnityEngine;
using System;

// 生産タイプ
public enum GeneType
{
    Default,
    Circle,
    Group,
    Boss,
    Vertical,
    Horizontal,
    CircleTitan,

    Num
};

// 生産ライン番号
public enum GLine{
    Line1, Line2, Line3, Line4, Num
}


// 作成データ
[System.Serializable]
public class GenerateData{
    // パラメータ
    public int emNo;                    // 敵番号
    public float start;                 // 開始時間
    public float end;                   // 終了時間
    public float interval;              // 間隔（秒）
    public int lineNo;                  // 生産ライン番号

    public GenerateData(EnemySOBJEnum no, float s, float e, float i, GLine lNo = GLine.Line1) {
        emNo = (int)no; start = s; end = e; interval = i; lineNo = (int)lNo;
    }
};


// 特殊データ
[Serializable]
public class GenerateSPData{
    // パラメータ
    public int emNo;                     // 敵番号
    public float start;                  // 開始時間
    public int geneType;                 // 生成パターン      
    public float fTemp0 = 0;             // 汎用0
    public float fTemp1 = 0;             // 汎用1

    public GenerateSPData(EnemySOBJEnum no, float s, GeneType gType, float f0 = 0, float f1 = 0){
        emNo = (int)no; start = s; geneType = (int)gType; fTemp0 = f0; fTemp1 = f1;
    }
};


// まとめデータ
public static class GeneDataCtl {
    public static GenerateData[][] geneJagTbl = {
        Stage1Data.stg1DataTbl,
        Stage0Data.stg0DataTbl,
        Stage1Data.stg1DataTbl,
        Stage1Data.stg1DataTbl,
        Stage1Data.stg1DataTbl,
        Stage1Data.stg1DataTbl,
    };
};

public static class GeneSPDataCtl{
    public static GenerateSPData[][] geneSPJagTbl = {
        Stage1SPData.stg1SPDataTbl,
        Stage0SPData.stg0SPDataTbl,
        Stage1SPData.stg1SPDataTbl,
        Stage1SPData.stg1SPDataTbl,
        Stage1SPData.stg1SPDataTbl,
        Stage1SPData.stg1SPDataTbl,
    };
}



// スクリプタブルオブジェクト
[CreateAssetMenu(menuName = "MyScriptable/Create GeneData")]
public class GeneSObjData : ScriptableObject
{
    public GenerateData[] geneTbl;
};

// ジェイソン用データ
[System.Serializable]
public class JSonGeneData
{
    public GenerateData[] geneTbl;
};
