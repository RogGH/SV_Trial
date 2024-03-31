using static EnemySOBJEnum;

public static class Stage1Data
{
    // ステージ１
    public static GenerateData[] stg1DataTbl = new GenerateData[]
    { 
        //              敵番                   開始     終了     間隔   生産ライン番号
        // 0-120
        new GenerateData(SlimeA,                0.0f,   30.0f,   1.0f,   GLine.Line1),
        new GenerateData(DogA,                 30.0f,   60.0f,   0.9f,   GLine.Line1),
        new GenerateData(JellyA,               60.0f,   90.0f,   1.0f,   GLine.Line1),
        new GenerateData(BirdA,                90.0f,  120.0f,   0.9f,   GLine.Line1),
        // 120-240                            
        new GenerateData(SlimeA,              120.0f,  150.0f,   1.0f,   GLine.Line1),
        new GenerateData(SklA,                150.0f,  180.0f,   1.0f,   GLine.Line1),
        new GenerateData(SlimeB,              180.0f,  210.0f,   0.7f,   GLine.Line1),
        new GenerateData(JellyB,              210.0f,  240.0f,   0.8f,   GLine.Line1),
        // 240-360                            
        new GenerateData(DogB,                240.0f,  300.0f,   1.0f,   GLine.Line1),
        new GenerateData(BirdB,               300.0f,  330.0f,   1.0f,   GLine.Line1),
        new GenerateData(DogB,                330.0f,  360.0f,   0.7f,   GLine.Line1),
        // 360-480                            
        new GenerateData(BatB,                360.0f,  420.0f,   0.8f,   GLine.Line1),
        new GenerateData(SklB,                420.0f,  450.0f,   0.7f,   GLine.Line1),
        new GenerateData(SlimeC,              450.0f,  480.0f,   0.4f,   GLine.Line1),
        // 480-600                            
        new GenerateData(DogC,                480.0f,  510.0f,   0.8f,   GLine.Line1),
        new GenerateData(BirdC,               510.0f,  540.0f,   0.5f,   GLine.Line1),
        new GenerateData(BatC,                540.0f,  570.0f,   0.5f,   GLine.Line1),
        new GenerateData(JellyC,              570.0f,  600.0f,   0.6f,   GLine.Line1),
        // 600                                
        new GenerateData(SlimeC,              600.0f,  660.0f,   1.0f,   GLine.Line1),
        new GenerateData(BatC,                660.0f,  720.0f,   0.5f,   GLine.Line1),
        new GenerateData(SklC,                720.0f,  1200.0f,   0.5f,   GLine.Line1),
    };
};

// 
public static class Stage1SPData
{
    // ステージ１
    public static GenerateSPData[] stg1SPDataTbl = new GenerateSPData[]
    {
        //                 敵番                   開始    起動タイプ           汎用0   汎用1 
        // 0-120
        new GenerateSPData(SlimeA,          20,      GeneType.Circle,    20),
        new GenerateSPData(PiyoA,           50,      GeneType.Group,      6),
        new GenerateSPData(PiyoA,           80,      GeneType.Group,     10),
        
        // 120-240
        new GenerateSPData(BossSlime,    　 120,      GeneType.Boss),
        new GenerateSPData(KnightA,      　 120,      GeneType.Circle,    60),
        new GenerateSPData(BombA,        　 150,      GeneType.Circle,     4),
        new GenerateSPData(BombA,        　 180,      GeneType.Circle,     6),
        new GenerateSPData(BombA,        　 210,      GeneType.Circle,     8),

        // 240-360
        new GenerateSPData(BossDog,        240,      GeneType.Boss),
        new GenerateSPData(PiyoA,          260,      GeneType.Horizontal,   15, 1),
        new GenerateSPData(PiyoA,          270,      GeneType.Vertical,     15, 1),
        new GenerateSPData(PiyoA,          280,      GeneType.Horizontal,   15, -1),
        new GenerateSPData(PiyoA,          290,      GeneType.Vertical,     15, -1),

        // 360-480
        new GenerateSPData(BossEye,        360,      GeneType.Boss),
        new GenerateSPData(PiyoB,          380,      GeneType.Circle, 4),            // 
        new GenerateSPData(PiyoB,          390,      GeneType.Circle, 6),
        new GenerateSPData(PiyoB,          400,      GeneType.Circle, 8),
        new GenerateSPData(PiyoB,          410,      GeneType.Circle, 10),
        new GenerateSPData(PiyoB,          420,      GeneType.Circle, 12),

        new GenerateSPData(BombB,          450,      GeneType.Circle, 4),
        new GenerateSPData(BombB,          460,      GeneType.Circle, 6),
        new GenerateSPData(BombB,          470,      GeneType.Circle, 8),

        // 480-600
        new GenerateSPData(BossBomb,       480,      GeneType.Boss),
        new GenerateSPData(KnightB,        480,      GeneType.Circle,    60),
        new GenerateSPData(PiyoC,          530,      GeneType.Horizontal,    15, 1),
        new GenerateSPData(PiyoC,          540,      GeneType.Vertical,      15, 1),
        new GenerateSPData(PiyoC,          550,      GeneType.Horizontal,    15, -1),
        new GenerateSPData(PiyoC,          560,      GeneType.Vertical,      15, -1),
        new GenerateSPData(PiyoC,          570,      GeneType.Circle,   15),
        new GenerateSPData(BombC,          580,      GeneType.Circle,   10),

        // 600-900
        new GenerateSPData(BossTitan,      600,      GeneType.Boss),
        new GenerateSPData(KnightC,        600,      GeneType.Circle,    60),
        new GenerateSPData(KnightC,        660,      GeneType.Circle,    60),
        new GenerateSPData(KnightC,        720,      GeneType.Circle,    60),
        new GenerateSPData(KnightC,        780,      GeneType.Circle,    60),
        new GenerateSPData(KnightC,        840,      GeneType.Circle,    60),
        new GenerateSPData(KnightC,        900,      GeneType.Circle,    60),
        new GenerateSPData(DeathA,         900,      GeneType.Boss),
   };
};