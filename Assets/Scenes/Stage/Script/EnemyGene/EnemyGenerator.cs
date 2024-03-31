using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * “G‚Ì—N‚«•û‚É‚Â‚¢‚Ä
 * 
 * Ql ‚Ù‚ë‚«‚ã‚ ‚P–Ê
 * Å‰‘å‘Ì1•b‚É‚Â‚«‚P‘Ì
 * 30•b‚Å•Ê‚Ì“G‚ª‚P‘ÌAˆÍ‚Ş‚æ‚¤‚Éœ‚ª—N‚¢‚½
 * 1•ª‚Å’¹‚ÌŒQ‚ê‚ªoŒ»
 * 1•ª‚R‚O•b‚Åœ‚ªc—ñ‚É•À‚ñ‚ÅoŒ»
 * ‚Q•ª‚Åƒ{ƒX
 * ‚Q•ª‚R‚O•bœ‚ªˆÍ‚Ş
 * ‚Q•ª‚S‚T•b@œ‚ª‰¡‚©‚çƒCƒ“
 * ‚R•ªV‚½‚È“G‚—‚İ‚½‚¢‚È‚â‚Â
 * ‚R•ª‚P‚T•bã‰º‚ğˆÍ‚Ş—p‚É“G‚ª—N‚­
 * ‚R•ª‚Q‚O•b’¹‚Ì“ËŒ‚
 * ‚R•ª‚S‚O•bã‰º‚Ì“Ëi
 * ‚S•ªƒ{ƒX{V‚µ‚¢“Gi’¹j
 * ‚S•ª‚Q‚O•bˆÍ‚Ş“G
 * ‚T•ª’¹‚ÌŒQ‚êAV‚µ‚¢“Gi•‚¢‚â‚Âj
 * ‚T•ª‚R‚O•b‰¡‚©‚çƒAƒ‰[ƒg‚SC‚T—ñ‚ÌŒQ‚ê
 * ‚U•ªƒ{ƒXAV‚µ‚¢“Gi”’‚¢ƒ„ƒcj
 * ‚U•ª‚R‚O•bV‚µ‚¢“Giƒƒjj
 * ‚V•ªiZˆÍ‚¢j
 * ‚V•ª‚R‚O•b
 * ‚W•ªƒ{ƒX
 * ‚W•ª‚R‚O•b‚‚¿œ‚ÌZˆÍ‚İ‚X˜A‚­‚ç‚¢@‚Ö‚Ìš‚ÌV‚µ‚¢“G
 * ‚X•ª’¹‚ÌŒQ‚êA‚T•b‚²‚Æ‚É‚S‰ñ
 * ‚P‚O•ªƒ{ƒX
 * ‚P‚O•ª‚P‚T•bã‰º¶‰E‚©‚çƒAƒ‰[ƒg
 * ‚P‚P•ª@V“GiƒŠ[ƒ[ƒ“ƒg’¹j
 * ‚P‚P•ª‚R‚O•b’¹‚ÌZˆÍ‚¢
 * ‚P‚Q•ªV“GiŒvj
 * ‚P‚Q•ª‚Q‚O•b¶‰EƒAƒ‰[ƒg
 * ‚P‚Q•ª‚Q‚T•bã‰ºƒAƒ‰[ƒg
 * ‚P‚Q•ª‚R‚O•bŒvFˆá‚¢
 * ‚P‚Q•ª—p•bã‰º¶‰EƒAƒ‰[ƒg
 * ‚P‚R•ªŒvƒ{ƒX
 * ‚P‚R•ª‚R‚O•b’¹ŒQ‚êA‚T•b‚²‚Æ‚É‚S‰ñ
 * ‚P‚S•ªœ‚Æ•€‚¿‚Ì¶‰E‚©‚ç˜A‘±oŒ»
 * ‚P‚S•ª‚Q‚O•bã‚©‚çƒAƒ‰[ƒgœ‚T•b‚²‚Æ‚S‰ñiãA‰ºAã‰ºA¶‰E‚S˜Aj
 * ‚P‚S•ª‚S‚T•bƒAƒ‰[ƒgã‰º
 * ‚P‚T•ª‚Q‘Ìƒ{ƒX
 * ‚P‚T•ª‚S‚T•bV“GiƒtƒNƒƒEA“¤Åj
 * ‚P‚U•ª‚P‚T•b@ ˆÍ‚¢
 * ‚P‚V•ªã‰º¶‰EƒAƒ‰[ƒg
 * ‚P‚V•ª‚R‚O•bƒ{ƒX
 * ‚P‚W•ª‚Å‚©“G
 * ‚P‚X•ª‘S•”‚Å‚©‚­
 * ‚Q‚O•ªƒXƒe[ƒWƒ{ƒXAV“G
 * 
 * 
 * 
 * ƒ”ƒ@ƒ“ƒpƒCƒAƒTƒoƒCƒo[@‚P•ª–ˆ‚É‘å‘Ì“G‚Ì•¦‚«•û‚ª•Ï‚í‚é
 * */


// ‚Æ‚è‚ ‚¦‚¸“G‚Ìí—Ş‚P‚Oí—Ş‚­‚ç‚¢ì‚é‚©cH
public enum EnemyNo
{
    SP1Blue,                        // ƒXƒvƒŠƒKƒ“
    SP2Green,   
    SP3Pink,

    SUB1,                           // ƒTƒ{ƒeƒ“ƒ_[
    SUB2Black,                      // 
    SUB3Red,                        // 

    Bomb1Red,                       // ƒ{ƒ€   
    Bomb2Blue,
    Bomb3Green,

    Cat1Black,                      // ƒQƒCƒ‰ƒLƒƒƒbƒg
    Cat2Brown,
    Cat3Blue,

    Purin1Green,                    // ƒvƒŠƒ“
    Purin2Red,
    Purin3Blue,
    Purin4Purple,
    Purin5Yellow,

    Ahriman1Yellow,                 // ƒA[ƒŠƒ}ƒ“
    Ahriman2Blue,
    Ahriman3Red,                    

    Tonberi1Green,                  // ƒgƒ“ƒxƒŠ
    Tonberi2Blue,
    Tonberi3Black,

    BossSpBlue,                     // ƒ{ƒXFƒXƒvƒŠƒKƒ“
    BossWallPurinPurble,            // ƒ{ƒX—pF•ÇƒvƒŠƒ“
    BossCatBlack,                   // ƒ{ƒXFƒQƒCƒ‰ƒLƒƒƒbƒg
    BossAhriman,                    // ƒ{ƒXFƒA[ƒŠƒ}ƒ“
    BossBomb,                       // ƒ{ƒXFƒ{ƒ€
    BossTaitan,                     // ƒ{ƒXFƒ^ƒCƒ^ƒ“

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

    // ì¬ƒf[ƒ^
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

        // ’Êí
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
        new GeneratData(20, EnemyNo.SP1Blue, GeneType.Circle, 20),              // ˆÍ‚Ş
        new GeneratData(50, EnemyNo.SUB1, GeneType.Group, 6),                   // “ËŒ‚
        new GeneratData(80, EnemyNo.SUB1, GeneType.Group, 10),                  // “ËŒ‚

        // 120-240
        new GeneratData(120, EnemyNo.BossSpBlue, GeneType.Default),             // ƒ{ƒX‚P     
        new GeneratData(120, EnemyNo.BossWallPurinPurble, GeneType.Circle, 60), // ˆÍ‚Ş        
        new GeneratData(180, EnemyNo.Bomb1Red, GeneType.Circle, 4),             // ˆÍ‚Ş
        new GeneratData(210, EnemyNo.Bomb1Red, GeneType.Circle, 8),             // ˆÍ‚Ş

        // 240-360
        new GeneratData(240, EnemyNo.BossCatBlack, GeneType.Default),           // ƒ{ƒX‚Q        
        new GeneratData(260, EnemyNo.SUB1, GeneType.Horizontal, 15, 1),         // ‰E‚©‚ç“ËŒ‚
        new GeneratData(270, EnemyNo.SUB1, GeneType.Vertical, 15, 1),           // ã‚©‚çc“ËŒ‚  
        new GeneratData(280, EnemyNo.SUB1, GeneType.Horizontal, 15, -1),        // ¶‚©‚ç‰¡“ËŒ‚  
        new GeneratData(290, EnemyNo.SUB1, GeneType.Vertical, 15, 1),           // ‰º‚©‚çc“ËŒ‚  

        // 360-480
        new GeneratData(360, EnemyNo.BossAhriman, GeneType.Default),            // ƒ{ƒX‚R       
        new GeneratData(380, EnemyNo.SUB2Black, GeneType.Circle, 4),            // 
        new GeneratData(390, EnemyNo.SUB2Black, GeneType.Circle, 6),            // 
        new GeneratData(400, EnemyNo.SUB2Black, GeneType.Circle, 8),            // 
        new GeneratData(410, EnemyNo.SUB2Black, GeneType.Circle, 10),           // 
        new GeneratData(420, EnemyNo.SUB2Black, GeneType.Circle, 12),           //  

        new GeneratData(450, EnemyNo.Bomb2Blue, GeneType.Circle, 4),           //
        new GeneratData(460, EnemyNo.Bomb2Blue, GeneType.Circle, 6),           //
        new GeneratData(470, EnemyNo.Bomb2Blue, GeneType.Circle, 8),           //

        // 480-600
        new GeneratData(480, EnemyNo.BossBomb, GeneType.Default),               // ƒ{ƒX‚S        
        new GeneratData(480, EnemyNo.BossWallPurinPurble, GeneType.Circle, 70), // ˆÍ‚Ş        
        new GeneratData(530, EnemyNo.SUB3Red, GeneType.Horizontal, 15, 1),         // ‰E‚©‚ç“ËŒ‚
        new GeneratData(540, EnemyNo.SUB3Red, GeneType.Vertical, 15, 1),           // ã‚©‚çc“ËŒ‚  
        new GeneratData(550, EnemyNo.SUB3Red, GeneType.Horizontal, 15, -1),        // ¶‚©‚ç‰¡“ËŒ‚  
        new GeneratData(560, EnemyNo.SUB3Red, GeneType.Vertical, 15, 1),           // ‰º‚©‚çc“ËŒ‚  
        new GeneratData(570, EnemyNo.SUB3Red, GeneType.Circle, 15),             // 
        new GeneratData(580, EnemyNo.Bomb3Green, GeneType.Circle, 10),             // 

        // 600
        new GeneratData(600, EnemyNo.BossTaitan, GeneType.Default),             // ƒ{ƒX‚T        
        new GeneratData(600, EnemyNo.Purin5Yellow, GeneType.Circle, 80), // ˆÍ‚Ş        
        new GeneratData(660, EnemyNo.Purin5Yellow, GeneType.Circle, 80), // ˆÍ‚Ş        
   };

    int geneSPTblNo = 0;
    const float BootWidth = 140.0f;

    void Start()
    {
        pl = StageManager.Ins.PlObj;
    }

    void Update()
    {
        // ’Êí‹N“®
        float stgTime = StageManager.Ins.StageTime;
        if (geneTblNo < geneTbl.Length - 1)
        {
            if (stgTime >= geneTbl[geneTblNo + 1].startSec)
            {
                // ƒe[ƒuƒ‹•ÏX
                geneTblNo++;
                // ƒpƒ‰ƒ[ƒ^‰Šú‰»
                timer = 0;
            }
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            generateEnemy(geneTbl[geneTblNo]);
            timer = geneTbl[geneTblNo].geneTime;
        }


        // “Áê‹N“®
        if (geneSPTblNo < geneSPTbl.Length)
        {
            if (stgTime >= geneSPTbl[geneSPTblNo].startSec)
            {
                generateEnemy(geneSPTbl[geneSPTblNo]);
                // ƒe[ƒuƒ‹•ÏX
                geneSPTblNo++;
                // ƒpƒ‰ƒ[ƒ^‰Šú‰»
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
                // Šî–{
                {
                    float degree = Random.Range(0, 360.0f);
                    float radian = Mathf.Deg2Rad * degree;

                    float setX = radius * Mathf.Cos(radian);
                    if (Mathf.Abs(setX) > limitX) { setX = limitX * Mathf.Sign(setX); }
                    float setY = radius * Mathf.Sin(radian);
                    if (Mathf.Abs(setY) > limitY) { setY = limitY * Mathf.Sign(setY); }

                    pos.x = pl.transform.position.x + setX;
                    pos.y = pl.transform.position.y + setY;

                    // ‚Æ‚è‚ ‚¦‚¸ƒ‰ƒ“ƒ_ƒ€‚Åo‚·
                    //int rand = Random.Range(0, enemyTbl.Length);
                    Instantiate(enemyTbl[tNo], pos, Quaternion.identity);
                }
                break;

            case GeneType.Circle:
            case GeneType.CircleTitan:
                // ‰~ó‚É”z’u
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
                // W’c‚Å‹N“®
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
                // c‚É”z’u
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
                        // ˆÚ“®ƒ^ƒCƒv‚ğ‹­§•ÏX
                        obj.GetComponent<Enemy1>().moveType =
                            (dir < 0 ? Enemy1.MoveType.LToR : Enemy1.MoveType.RToL);
                    }
                }
                break;

            case GeneType.Horizontal:
                // ‰¡‚É”z’u
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
                        // ˆÚ“®ƒ^ƒCƒv‚ğ‹­§•ÏX
                        obj.GetComponent<Enemy1>().moveType =
                            (dir < 0 ? Enemy1.MoveType.DToU : Enemy1.MoveType.UToD);
                    }
                }
                break;
        }
    }
}