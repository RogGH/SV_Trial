using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 敵の湧き方について
 * 
 * 参考 ほろきゅあ１面
 * 最初大体1秒につき１体
 * 30秒で別の敵が１体、囲むように骨が湧いた
 * 1分で鳥の群れが出現
 * 1分３０秒で骨が縦列に並んで出現
 * ２分でボス
 * ２分３０秒骨が囲む
 * ２分４５秒　骨が横からイン
 * ３分新たな敵ｗみたいなやつ
 * ３分１５秒上下を囲む用に敵が湧く
 * ３分２０秒鳥の突撃
 * ３分４０秒上下の突進
 * ４分ボス＋新しい敵（鳥）
 * ４分２０秒囲む敵
 * ５分鳥の群れ、新しい敵（黒いやつ）
 * ５分３０秒横からアラート４，５列の群れ
 * ６分ボス、新しい敵（白いヤツ）
 * ６分３０秒新しい敵（ワニ）
 * ７分（〇囲い）
 * ７分３０秒
 * ８分ボス
 * ８分３０秒盾持ち骨の〇囲み９連くらい　への字の新しい敵
 * ９分鳥の群れ、５秒ごとに４回
 * １０分ボス
 * １０分１５秒上下左右からアラート
 * １１分　新敵（リーゼント鳥）
 * １１分３０秒鳥の〇囲い
 * １２分新敵（時計）
 * １２分２０秒左右アラート
 * １２分２５秒上下アラート
 * １２分３０秒時計色違い
 * １２分用秒上下左右アラート
 * １３分時計ボス
 * １３分３０秒鳥群れ、５秒ごとに４回
 * １４分骨と斧持ちの左右から連続出現
 * １４分２０秒上からアラート骨５秒ごと４回（上、下、上下、左右４連）
 * １４分４５秒アラート上下
 * １５分２体ボス
 * １５分４５秒新敵（フクロウ、豆芝）
 * １６分１５秒　□囲い
 * １７分上下左右アラート
 * １７分３０秒ボス
 * １８分でか敵
 * １９分全部でかく
 * ２０分ステージボス、新敵
 * 
 * 
 * 
 * ヴァンパイアサバイバー　１分毎に大体敵の沸き方が変わる
 * */


// とりあえず敵の種類１０種類くらい作るか…？
public enum EnemyNo
{
    SP1Blue,                        // スプリガン
    SP2Green,   
    SP3Pink,

    SUB1,                           // サボテンダー
    SUB2Black,                      // 
    SUB3Red,                        // 

    Bomb1Red,                       // ボム   
    Bomb2Blue,
    Bomb3Green,

    Cat1Black,                      // ゲイラキャット
    Cat2Brown,
    Cat3Blue,

    Purin1Green,                    // プリン
    Purin2Red,
    Purin3Blue,
    Purin4Purple,
    Purin5Yellow,

    Ahriman1Yellow,                 // アーリマン
    Ahriman2Blue,
    Ahriman3Red,                    

    Tonberi1Green,                  // トンベリ
    Tonberi2Blue,
    Tonberi3Black,

    BossSpBlue,                     // ボス：スプリガン
    BossWallPurinPurble,            // ボス用：壁プリン
    BossCatBlack,                   // ボス：ゲイラキャット
    BossAhriman,                    // ボス：アーリマン
    BossBomb,                       // ボス：ボム
    BossTaitan,                     // ボス：タイタン

    Num

};

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] enemyTbl;
    GameObject pl;
    const float InvalidTimer = 100;

    enum GeneType
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

    // 作成データ
    class GeneratData
    {
        public GeneratData(float sec, EnemyNo eNo, float gTime)
        {
            startSec = sec;
            enemyNo = eNo;
            geneTime = gTime;
            geneType = GeneType.Default;
        }
        public GeneratData(float sec, EnemyNo eNo, GeneType gType, float t1 = 0, float t2 = 0)
        {
            startSec = sec;
            enemyNo = eNo;
            geneTime = InvalidTimer;
            geneType = gType;
            fTemp1 = t1;
            fTemp2 = t2;
        }

        // 通常
        public float startSec;
        public EnemyNo enemyNo;
        public float geneTime;
        public GeneType geneType;
        public float fTemp1 = 0;
        public float fTemp2 = 0;
    };
    GeneratData[] geneTbl = new GeneratData[]{
        // 0-120
        new GeneratData(0, EnemyNo.SP1Blue, 1.0f),
        new GeneratData(30, EnemyNo.Cat1Black, 0.9f),
        new GeneratData(60, EnemyNo.Purin1Green, 1.0f),
        new GeneratData(90, EnemyNo.Ahriman1Yellow, 0.9f),

        // 120-240
        new GeneratData(120, EnemyNo.SP1Blue, 1.0f),
        new GeneratData(150, EnemyNo.Tonberi1Green, 1.0f),
        new GeneratData(180, EnemyNo.SP2Green, 0.7f),
        new GeneratData(210, EnemyNo.Purin2Red, 0.8f),

        // 240-360
        new GeneratData(240, EnemyNo.Cat1Black, 0.3f),
        new GeneratData(300, EnemyNo.Ahriman2Blue, 0.6f),
        new GeneratData(330, EnemyNo.Cat2Brown, 0.6f),

        // 360-480
        new GeneratData(360, EnemyNo.Ahriman2Blue, 0.8f),
        new GeneratData(420, EnemyNo.Tonberi2Blue, 0.7f),
        new GeneratData(450, EnemyNo.SP3Pink, 0.4f),

        // 480-600
        new GeneratData(480, EnemyNo.Purin3Blue, 0.8f),
        new GeneratData(510, EnemyNo.Cat3Blue, 0.5f),
        new GeneratData(540, EnemyNo.Ahriman3Red, 0.5f),
        new GeneratData(570, EnemyNo.Tonberi3Black, 0.6f),

        // 600
        new GeneratData(600, EnemyNo.SP3Pink, 1.0f),
        new GeneratData(660, EnemyNo.Ahriman3Red, 1.0f),
        new GeneratData(720, EnemyNo.Tonberi3Black, 0.1f),
    };

    int geneTblNo = 0;
    float timer = 0;

    GeneratData[] geneSPTbl = new GeneratData[]{
        // 0-120
        new GeneratData(20, EnemyNo.SP1Blue, GeneType.Circle, 20),              // 囲む
        new GeneratData(50, EnemyNo.SUB1, GeneType.Group, 6),                   // 突撃
        new GeneratData(80, EnemyNo.SUB1, GeneType.Group, 10),                  // 突撃

        // 120-240
        new GeneratData(120, EnemyNo.BossSpBlue, GeneType.Default),             // ボス１     
        new GeneratData(120, EnemyNo.BossWallPurinPurble, GeneType.Circle, 60), // 囲む        
        new GeneratData(180, EnemyNo.Bomb1Red, GeneType.Circle, 4),             // 囲む
        new GeneratData(210, EnemyNo.Bomb1Red, GeneType.Circle, 8),             // 囲む

        // 240-360
        new GeneratData(240, EnemyNo.BossCatBlack, GeneType.Default),           // ボス２        
        new GeneratData(260, EnemyNo.SUB1, GeneType.Horizontal, 15, 1),         // 右から突撃
        new GeneratData(270, EnemyNo.SUB1, GeneType.Vertical, 15, 1),           // 上から縦突撃  
        new GeneratData(280, EnemyNo.SUB1, GeneType.Horizontal, 15, -1),        // 左から横突撃  
        new GeneratData(290, EnemyNo.SUB1, GeneType.Vertical, 15, 1),           // 下から縦突撃  

        // 360-480
        new GeneratData(360, EnemyNo.BossAhriman, GeneType.Default),            // ボス３       
        new GeneratData(380, EnemyNo.SUB2Black, GeneType.Circle, 4),            // 
        new GeneratData(390, EnemyNo.SUB2Black, GeneType.Circle, 6),            // 
        new GeneratData(400, EnemyNo.SUB2Black, GeneType.Circle, 8),            // 
        new GeneratData(410, EnemyNo.SUB2Black, GeneType.Circle, 10),           // 
        new GeneratData(420, EnemyNo.SUB2Black, GeneType.Circle, 12),           //  

        new GeneratData(450, EnemyNo.Bomb2Blue, GeneType.Circle, 4),           //
        new GeneratData(460, EnemyNo.Bomb2Blue, GeneType.Circle, 6),           //
        new GeneratData(470, EnemyNo.Bomb2Blue, GeneType.Circle, 8),           //

        // 480-600
        new GeneratData(480, EnemyNo.BossBomb, GeneType.Default),               // ボス４        
        new GeneratData(480, EnemyNo.BossWallPurinPurble, GeneType.Circle, 70), // 囲む        
        new GeneratData(530, EnemyNo.SUB3Red, GeneType.Horizontal, 15, 1),         // 右から突撃
        new GeneratData(540, EnemyNo.SUB3Red, GeneType.Vertical, 15, 1),           // 上から縦突撃  
        new GeneratData(550, EnemyNo.SUB3Red, GeneType.Horizontal, 15, -1),        // 左から横突撃  
        new GeneratData(560, EnemyNo.SUB3Red, GeneType.Vertical, 15, 1),           // 下から縦突撃  
        new GeneratData(570, EnemyNo.SUB3Red, GeneType.Circle, 15),             // 
        new GeneratData(580, EnemyNo.Bomb3Green, GeneType.Circle, 10),             // 

        // 600
        new GeneratData(600, EnemyNo.BossTaitan, GeneType.Default),             // ボス５        
        new GeneratData(600, EnemyNo.Purin5Yellow, GeneType.Circle, 80), // 囲む        
        new GeneratData(660, EnemyNo.Purin5Yellow, GeneType.Circle, 80), // 囲む        
   };

    int geneSPTblNo = 0;
    const float BootWidth = 140.0f;

    void Start()
    {
        pl = StageManager.Ins.PlObj;
    }

    void Update()
    {
        // 通常起動
        float stgTime = StageManager.Ins.StageTime;
        if (geneTblNo < geneTbl.Length - 1)
        {
            if (stgTime >= geneTbl[geneTblNo + 1].startSec)
            {
                // テーブル変更
                geneTblNo++;
                // パラメータ初期化
                timer = 0;
            }
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            generateEnemy(geneTbl[geneTblNo]);
            timer = geneTbl[geneTblNo].geneTime;
        }


        // 特殊起動
        if (geneSPTblNo < geneSPTbl.Length)
        {
            if (stgTime >= geneSPTbl[geneSPTblNo].startSec)
            {
                generateEnemy(geneSPTbl[geneSPTblNo]);
                // テーブル変更
                geneSPTblNo++;
                // パラメータ初期化
                timer = 0;
            }
        }
    }

    void generateEnemy(GeneratData data)
    {
        GeneType gType = data.geneType;
        float sclX = 1280 / 2;
        float sclY = 720 / 2;
        float offset = 40;
        float radius = Mathf.Sqrt(sclX * sclX + sclY * sclY) + offset;
        float limitX = sclX + offset;
        float limitY = sclY + offset;
        int tNo = (int)data.enemyNo;
        Vector2 pos;

        switch (gType)
        {
            case GeneType.Default:
            case GeneType.Boss:
                // 基本
                {
                    float degree = Random.Range(0, 360.0f);
                    float radian = Mathf.Deg2Rad * degree;

                    float setX = radius * Mathf.Cos(radian);
                    if (Mathf.Abs(setX) > limitX) { setX = limitX * Mathf.Sign(setX); }
                    float setY = radius * Mathf.Sin(radian);
                    if (Mathf.Abs(setY) > limitY) { setY = limitY * Mathf.Sign(setY); }

                    pos.x = pl.transform.position.x + setX;
                    pos.y = pl.transform.position.y + setY;

                    // とりあえずランダムで出す
                    //int rand = Random.Range(0, enemyTbl.Length);
                    Instantiate(enemyTbl[tNo], pos, Quaternion.identity);
                }
                break;

            case GeneType.Circle:
            case GeneType.CircleTitan:
                // 円状に配置
                {
                    int loopNum = (int)data.fTemp1;
                    float degInterval = 360 / data.fTemp1;
                    float shotRadius = radius / 1.25f;
                    float longRadius = radius;
                    if (gType == GeneType.CircleTitan) {
                        shotRadius *= 1.2f;
                        longRadius *= 1.2f;
                    }

                    for (int loopNo = 0; loopNo < loopNum; ++loopNo)
                    {
                        float radian = Mathf.Deg2Rad * (degInterval * loopNo);

                        float setX = longRadius * Mathf.Cos(radian);
                        float setY = shotRadius * Mathf.Sin(radian);

                        pos.x = pl.transform.position.x + setX;
                        pos.y = pl.transform.position.y + setY;

                        Instantiate(enemyTbl[tNo], pos, Quaternion.identity);
                    }
                }
                break;

            case GeneType.Group:
                // 集団で起動
                {
                    int loopNum = (int)(data.fTemp1);
                    float degree = Random.Range(0, 360.0f);
                    float radian = Mathf.Deg2Rad * degree;
                    float setX = radius * Mathf.Cos(radian);
                    if (Mathf.Abs(setX) > limitX) { setX = limitX * Mathf.Sign(setX); }
                    float setY = radius * Mathf.Sin(radian);
                    if (Mathf.Abs(setY) > limitY) { setY = limitY * Mathf.Sign(setY); }

                    float degInterval = 360 / loopNum;
                    float groupOffset = 40.0f;

                    pos.x = pl.transform.position.x + setX;
                    pos.y = pl.transform.position.y + setY;
                    Instantiate(enemyTbl[tNo], pos, Quaternion.identity);

                    for (int loopNo = 0; loopNo < loopNum; ++loopNo)
                    {
                        float offsetX = groupOffset * Mathf.Cos(degInterval * loopNo);
                        float offsetY = groupOffset * Mathf.Sin(degInterval * loopNo);

                        pos.x = pl.transform.position.x + setX + offsetX;
                        pos.y = pl.transform.position.y + setY + offsetY;

                        Instantiate(enemyTbl[tNo], pos, Quaternion.identity);
                    }
                }
                break;

            case GeneType.Vertical:
                // 縦に配置
                {
                    int bootNum = (int)data.fTemp1;
                    float dir = data.fTemp2;
                    float width = BootWidth;

                    float setX = pl.transform.position.x + sclX * dir;
                    float setY = pl.transform.position.y;
                    setY += width * bootNum / 2;

                    for (int loopNo = 0; loopNo < bootNum; ++loopNo)
                    {
                        pos.x = setX;
                        pos.y = setY - width * loopNo;

                        GameObject obj = Instantiate(enemyTbl[tNo], pos, Quaternion.identity);
                        // 移動タイプを強制変更
                        obj.GetComponent<Enemy1>().moveType =
                            (dir < 0 ? Enemy1.MoveType.LToR : Enemy1.MoveType.RToL);
                    }
                }
                break;

            case GeneType.Horizontal:
                // 横に配置
                {
                    int bootNum = (int)data.fTemp1;
                    float dir = data.fTemp2;
                    float width = BootWidth;

                    float setY = pl.transform.position.y + sclY * dir;
                    float setX = pl.transform.position.x;
                    setX -= width * bootNum / 2;

                    for (int loopNo = 0; loopNo < bootNum; ++loopNo)
                    {
                        pos.x = setX + width * loopNo;
                        pos.y = setY;

                        GameObject obj = Instantiate(enemyTbl[tNo], pos, Quaternion.identity);
                        // 移動タイプを強制変更
                        obj.GetComponent<Enemy1>().moveType =
                            (dir < 0 ? Enemy1.MoveType.DToU : Enemy1.MoveType.UToD);
                    }
                }
                break;
        }
    }
}