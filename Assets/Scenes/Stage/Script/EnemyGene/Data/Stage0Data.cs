using static EnemySOBJEnum;

public static class Stage0Data
{
    // ステージ１
    public static GenerateData[] stg0DataTbl = new GenerateData[]
    { 
        //              敵番                   開始     終了     間隔   生産ライン番号
    };
};

// 
public static class Stage0SPData
{
    // ステージ１
    public static GenerateSPData[] stg0SPDataTbl = new GenerateSPData[]
    {
        //                 敵番     開始    起動タイプ           汎用0   汎用1 
        // 0-120
        new GenerateSPData(WoodA,   0,      GeneType.Boss),
   };
};