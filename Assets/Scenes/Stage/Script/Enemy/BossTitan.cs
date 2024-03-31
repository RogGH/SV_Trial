using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyCmm
{
    // 行動順番
    TitanActNo[] titanActTbl = new TitanActNo[] {
        TitanActNo.Crash,
        TitanActNo.Slide,
        TitanActNo.Pressure,
        TitanActNo.BombCircle,
        TitanActNo.Pressure,
        TitanActNo.Crash,
        TitanActNo.Sammon,
        TitanActNo.BombCross,
        TitanActNo.Slide,
        TitanActNo.Pressure,
        TitanActNo.BombLine,
        TitanActNo.Slide,
    };
    // パラメータ
    const float CrashSignCount = 2.0f;
    const float CrashEndCount = 1.0f;
    const float SlideSignCount = 2.0f;
    const float SlideEndCount = 2.0f;
    const float PressureSignCount = 2.0f;
    const float PressureWaitCount = 1.5f;
    const float PressureEndCount = 1.0f;
    const float SammonSignCount = 2.0f;
    const float SammonEndCount = 1.5f;
    const float SammonOfsX = 150.0f;
    const float BombCircleInterval = 0.3f;
    const float BombCircleEndCount = 3.0f;
    const float BombCircleOfs = 400.0f;
    const float JailOfsY = 500.0f;
    const float BombLineInterval = 1.0f;
    const float BombLineEndCount = 1.5f;
    const float BombLineOfs = 400.0f;
    const float BombCrossOfs = 400.0f;
    const float BombCrossInterval = 1.0f;
    const float BombCrossEndCount = 1.5f;

    // タイタン用
    void atkTitanFunc()
    {
        // タイタンの攻撃方法
        // ジオクラ＞ランスラ＞重み＞雑魚呼び出し＞三列ボム＞十字ボム＞円ボム
        titanFTbl[actRno]();
    }

    void setAtkRno(TitanActNo rno)
    {
        actRno = (int)rno;
        atkRno = 0;
    }
    void atkCrash()
    {
        switch (atkRno)
        {
            case 0:
                hb.SetAtkActive(false);
                hb.SetDefActive(false);
                atkCounter = 0.5f;
                moveVec = CrashUpSpd;
                atkRno++;
                atkCrash();
                break;

            case 1:
                // とりあえず上昇
                transform.position += moveVec * Time.deltaTime;
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    // ここで表示を消す
                    spCmp.material.color = new Color(0, 0, 0, 0);

                    // 目標座標を設定
                    targetPos = plObj.transform.position;
                    // 予兆を起動
                    GameObject obj = EffecgtManager.Ins.BootEffect(EffectNo.AtkSign, targetPos);
                    obj.GetComponent<AtkSign>().
                        SetUpSignOnly(
                            gameObject,
                            AtkSign.SignType.Circle,
                            400,
                            CrashSignCount);

                    atkCounter = CrashSignCount;
                    atkRno++;
                }
                break;

            case 2:
                // 予兆を待ち
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    Vector3 setPos = targetPos;
                    setPos.y += 800.0f;
                    transform.position = setPos;
                    moveVec = CrashDownSpd;
                    spCmp.material.color = new Color(0.5f, 0.5f, 0.5f, 1);
                    atkRno++;
                }
                break;

            case 3:
                // 落下
                transform.position += moveVec * Time.deltaTime;
                if (transform.position.y <= targetPos.y)
                {
                    transform.position = targetPos;
                    // 攻撃起動
                    Vector3 bootPos = targetPos;
                    bootPos.y += 100.0f;
                    Instantiate(BossShellTbl[0], bootPos, Quaternion.identity);
                    hb.SetAtkActive(true);
                    hb.SetDefActive(true);
                    atkCounter = CrashEndCount;
                    atkRno++;
                }
                break;

            case 4:
                // 
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    TitanSetNextAct();
                }
                break;
        }
    }

    void atkSlide()
    {
        switch (atkRno)
        {
            case 0:
                // 予兆を起動 
                float radian = Mathf.Atan2(plObj.transform.position.y - transform.position.y,
                   plObj.transform.position.x - transform.position.x);
                targetDeg = radian * Mathf.Rad2Deg;
                {
                    GameObject obj = EffecgtManager.Ins.BootEffect(EffectNo.AtkSign, targetPos);
                    obj.GetComponent<AtkSign>().SetUpRect(gameObject, 1200, SlideSignCount, targetDeg);

                    obj = EffecgtManager.Ins.BootEffect(EffectNo.AtkSign, targetPos);
                    obj.GetComponent<AtkSign>().SetUpRect(gameObject, 1200, SlideSignCount, targetDeg + 45);

                    obj = EffecgtManager.Ins.BootEffect(EffectNo.AtkSign, targetPos);
                    obj.GetComponent<AtkSign>().SetUpRect(gameObject, 1200, SlideSignCount, targetDeg - 45);
                }

                atkCounter = SlideSignCount;
                atkRno++;
                atkSlide();
                break;

            case 1:
                // 待ち
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    Vector3 bootPos = transform.position;
                    bootPos.y += 60.0f;
                    // シェル起動
                    GameObject obj = Instantiate(BossShellTbl[1], bootPos, Quaternion.identity);
                    obj.GetComponent<EShellBooter>().SetUp(targetDeg);

                    obj = Instantiate(BossShellTbl[1], bootPos, Quaternion.identity);
                    obj.GetComponent<EShellBooter>().SetUp(targetDeg + 45);

                    obj = Instantiate(BossShellTbl[1], bootPos, Quaternion.identity);
                    obj.GetComponent<EShellBooter>().SetUp(targetDeg - 45);


                    atkCounter = SlideEndCount;
                    atkRno++;
                }
                break;

            case 2:
                // 
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    TitanSetNextAct();
                }
                break;
        }
    }

    void atkPressure()
    {
        switch (atkRno)
        {
            case 0:
                // 予兆を起動 
                // 目標座標を設定
                targetPos = plObj.transform.position;
                {
                    GameObject obj = EffecgtManager.Ins.BootEffect(EffectNo.AtkSign, targetPos);
                    obj.GetComponent<AtkSign>().
                        SetUpSignOnly(
                            gameObject,
                            AtkSign.SignType.Circle,
                            350,
                            PressureSignCount,
                            BossShellTbl[2],
                            AtkSign.ShellType.Def);
                }
                atkCounter = PressureWaitCount;
                atkRno++;
                atkPressure();
                break;

            case 1:
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    targetPos = plObj.transform.position;
                    {
                        GameObject obj = EffecgtManager.Ins.BootEffect(EffectNo.AtkSign, targetPos);
                        obj.GetComponent<AtkSign>().
                            SetUpSignOnly(
                                gameObject,
                                AtkSign.SignType.Circle,
                                350,
                                PressureSignCount,
                                BossShellTbl[2],
                                AtkSign.ShellType.Def);
                    }
                    atkCounter = PressureSignCount;
                    atkRno++;
                }
                break;

            case 2:
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    atkCounter = PressureEndCount;
                    atkRno++;
                }
                break;

            case 3:
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    TitanSetNextAct();
                }
                break;
        }
    }

    void atkSammon()
    {
        switch (atkRno)
        {
            case 0:
                // 予兆を起動 
                targetPos = transform.position;
                targetPos.y -= 40.0f;

                for (int no = 0; no < 2; ++no)
                {
                    Vector3 bootPos = targetPos;
                    bootPos.x += SammonOfsX * (no == 0 ? -1 : 1);
                    GameObject obj = EffecgtManager.Ins.BootEffect(EffectNo.AtkSign, bootPos);
                    obj.GetComponent<AtkSign>().
                        SetUpSignOnly(
                            gameObject,
                            AtkSign.SignType.Circle,
                            150,
                            SammonSignCount,
                            BossShellTbl[3],
                            AtkSign.ShellType.Eff
                            );
                }

                atkCounter = SammonSignCount;
                atkRno++;
                atkPressure();
                break;

            case 1:
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    // ここで敵を起動する
                    Vector3 bootPos = targetPos;
                    bootPos.y += -50.0f;

                    bootPos.x = targetPos.x - SammonOfsX;
                    Instantiate(BossShellTbl[4], bootPos, Quaternion.identity);
                    bootPos.x = targetPos.x + SammonOfsX;
                    Instantiate(BossShellTbl[4], bootPos, Quaternion.identity);

                    atkCounter = SammonEndCount;
                    atkRno++;
                }
                break;

            case 2:
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    TitanSetNextAct();
                }
                break;
        }
    }

    void atkBombCircle()
    {
        switch (atkRno)
        {
            case 0:
                // 初期化
                targetPos = plObj.transform.position;
                // 真ん中に起動
                {
                    GameObject obj = Instantiate(BossShellTbl[5], targetPos, Quaternion.identity);
                    obj.GetComponent<Enemy1>().JailSetUp();
                }
                // まずど真ん中にシェルを起動
                atkCounter = BombCircleInterval;

                atkIctr0 = 0;                           //
                atkIctr1 = Random.Range(0, 9) * 45;     // どこの角度からやるか
                atkIctr2 = Random.Range(0, 2);          // 0が時計

                ++atkRno;
                break;

            case 1:
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    {
                        Vector3 bootPos = targetPos;
                        float length = BombCircleOfs;
                        float radian = (atkIctr1 + atkIctr0 * 45 * (atkIctr2 == 0 ? -1 : 1)) * Mathf.Deg2Rad;
                        bootPos.x += length * Mathf.Cos(radian);
                        bootPos.y += length * Mathf.Sin(radian);
                        // 真ん中に起動
                        GameObject obj = Instantiate(BossShellTbl[5], bootPos, Quaternion.identity);
                        obj.GetComponent<Enemy1>().JailSetUp();
                    }

                    // 起動数を加算
                    atkIctr0++;
                    if (atkIctr0 >= 8)
                    {
                        // 次へ
                        atkCounter = BombCircleEndCount;
                        ++atkRno;
                    }
                    else
                    {
                        // もう一度
                        atkCounter = BombCircleInterval;
                    }
                }
                break;

            case 2:
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    TitanSetNextAct();
                }
                break;
        }
    }

    void atkBombLine()
    {
        switch (atkRno)
        {
            case 0:
                // 初期化
                targetPos = plObj.transform.position;
                // 真ん中に起動
                {
                    GameObject obj = Instantiate(BossShellTbl[5], targetPos, Quaternion.identity);
                    obj.GetComponent<Enemy1>().JailSetUp();
                }
                // 2列
                for (int lp = 1; lp < 2; ++lp)
                {
                    // 上下に＋2個
                    Vector3 bootPos = targetPos;
                    bootPos.y = targetPos.y - BombLineOfs * lp;
                    GameObject obj = Instantiate(BossShellTbl[5], bootPos, Quaternion.identity);
                    obj.GetComponent<Enemy1>().JailSetUp();

                    bootPos.y = targetPos.y + BombLineOfs * lp;
                    obj = Instantiate(BossShellTbl[5], bootPos, Quaternion.identity);
                    obj.GetComponent<Enemy1>().JailSetUp();
                }

                atkCounter = BombLineInterval;

                atkIctr0 = 0;                           //
                atkIctr1 = Random.Range(0, 2);          // ボムの左か右か

                ++atkRno;
                break;

            case 1:
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    Vector3 bootPos = targetPos;
                    // ランダムで左か右かを設定
                    bootPos.x += BombLineOfs * (atkIctr0 == atkIctr1 ? -1 : 1);
                    // 真ん中に起動
                    GameObject obj = Instantiate(BossShellTbl[5], bootPos, Quaternion.identity);
                    obj.GetComponent<Enemy1>().JailSetUp();

                    // 2列
                    for (int lp = 1; lp < 2; ++lp)
                    {
                        // 上下に＋2個
                        bootPos.y = targetPos.y - BombLineOfs * lp;
                        obj = Instantiate(BossShellTbl[5], bootPos, Quaternion.identity);
                        obj.GetComponent<Enemy1>().JailSetUp();

                        bootPos.y = targetPos.y + BombLineOfs * lp;
                        obj = Instantiate(BossShellTbl[5], bootPos, Quaternion.identity);
                        obj.GetComponent<Enemy1>().JailSetUp();
                    }

                    // 起動数を加算
                    atkIctr0++;
                    if (atkIctr0 >= 2)
                    {
                        // 次へ
                        atkCounter = BombLineEndCount;
                        ++atkRno;
                    }
                    else
                    {
                        // もう一度
                        atkCounter = BombLineInterval;
                    }
                }
                break;

            case 2:
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    TitanSetNextAct();
                }
                break;
        }
    }

    void atkBombCross()
    {
        switch (atkRno)
        {
            case 0:
                // 初期化
                targetPos = plObj.transform.position;
                // 
                atkIctr0 = 0;                           //
                atkIctr1 = Random.Range(0, 2);          // ボムの十字か左右か

                // 真ん中に起動
                {
                    GameObject obj = Instantiate(BossShellTbl[5], targetPos, Quaternion.identity);
                    obj.GetComponent<Enemy1>().JailSetUp();
                }

                {
                    // 十字か斜めか
                    float deg = (atkIctr1 == atkIctr0 ? 0 : 45);
                    float length = BombCrossOfs;
                    for (int lp = 0; lp < 4; ++lp)
                    {
                        Vector3 bootPos = targetPos;
                        float radian = (deg + (lp * 90.0f)) * Mathf.Deg2Rad;
                        bootPos.x += length * Mathf.Cos(radian);
                        bootPos.y += length * Mathf.Sin(radian);
                        // 真ん中に起動
                        GameObject obj = Instantiate(BossShellTbl[5], bootPos, Quaternion.identity);
                        obj.GetComponent<Enemy1>().JailSetUp();
                    }
                }
                atkCounter = BombCrossInterval;

                ++atkRno;
                break;

            case 1:
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    // 起動数を加算
                    atkIctr0++;
                    // 
                    float deg = (atkIctr1 == atkIctr0 ? 0 : 45);
                    float length = BombCrossOfs;
                    for (int lp = 0; lp < 4; ++lp)
                    {
                        Vector3 bootPos = targetPos;
                        float radian = (deg + (lp * 90.0f)) * Mathf.Deg2Rad;
                        bootPos.x += length * Mathf.Cos(radian);
                        bootPos.y += length * Mathf.Sin(radian);
                        // 真ん中に起動
                        GameObject obj = Instantiate(BossShellTbl[5], bootPos, Quaternion.identity);
                        obj.GetComponent<Enemy1>().JailSetUp();
                    }

                    // 次へ
                    atkCounter = BombCrossEndCount;
                    ++atkRno;
                }
                break;

            case 2:
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    TitanSetNextAct();
                }
                break;
        }
    }

    // 次の行動を決定
    void TitanSetNextAct()
    {
        setAtkRno(titanActTbl[actTblNo]);
        ++actTblNo;
        if (actTblNo >= titanActTbl.Length)
        {
            actTblNo = 0;
        }
    }

}
