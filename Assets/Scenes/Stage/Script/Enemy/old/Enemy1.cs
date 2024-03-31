using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public enum MoveType
    {
        HomingToPL,         // PLへ誘導
        StraightToPL,       // PLへ直進
        LToR,               // 左から右
        RToL,               // 右から左
        UToD,               // 上から下
        DToU,               // 下から上
        None,               // 移動無
    };
    public enum AtkType
    {
        MoveOnly,           // 攻撃無し
        Bomb,               // 爆発（自爆）
        GazeSector,         // 視線（扇）
        Explosion,          // 爆発（定期的）
        BossTitan,          // ボス（タイタン専用行動）
        LandSlide,          // ランスラ（定期的）
        Jail,               // ジェイル（自爆）
    };

    // 外部設定
    public Material dieMat;
    [SerializeField] DropType dType;
    [SerializeField] int defaultDir = -1;
    public MoveType moveType = MoveType.HomingToPL;
    [SerializeField] float moveSpd = 1.0f * 60;
    [SerializeField] AtkType atkType = AtkType.MoveOnly;
    [SerializeField] float escapeTime = 60;
    // 外部設定（拡張)
    public GameObject SignObj;
    public GameObject ShellObj;
    public GameObject[] BossShellTbl;
    // コンポーネント
    GameObject plObj;
    SpriteRenderer spComp;
    HitBase hb;
    // 基本パラメータ
    bool dieFlag = false;
    Vector3 oldPos;
    Vector3 moveVec;
    float scaleSpd = 8 * 60;
    float scaleRate = 0.04f;
    Vector3 defScale;
    float scaleTimer = 0;
    int moveRno = 0;
    bool escapeFlag = false;

    // 攻撃関連
    int actRno = 0;
    int atkRno = 0;
    float atkCounter = 0;
    int atkIctr0 = 0;
    int atkIctr1 = 0;
    int atkIctr2 = 0;
    bool nearFlag = false;

    // 爆弾関連
    const float nearDistance = 100.0f;
    const float BombCount = 2.5f;
    Vector3 BombEffOfs = new Vector3(0, 20, 0);
    // 目線攻撃関連
    const float GazeCount = 6.0f;
    const float GazeSignCount = 3.0f;
    Vector3 GazeOfs = new Vector3(0, 80, 0);
    // 爆発攻撃関連
    const float ExplodeCount = 5.0f;
    const float ExplodeSignCount = 2.5f;
    // ランスラ関連
    const float LandSlideCount = 5.0f;
    const float LandSlideSignCount = 2.0f;
    // ジェイル関連
    const float JailCount = 5.0f;

    // タイタン用
    enum TitanActNo
    {
        Crash,      // ジオクラッシュ
        Slide,      // ランスラ
        Pressure,   // 重み
        Sammon,     // 雑魚召喚
        BombCircle, // 爆弾（円）
        BombLine,   // 爆弾（三列）
        BombCross,  // 爆弾（十字）
        Num
    };
    delegate void dFunc();
    private dFunc[] titanFTbl;
    Vector3 CrashUpSpd = new Vector3(0, 60.0f * 60.0f, 0);
    Vector3 CrashDownSpd = new Vector3(0, -70.0f * 60.0f, 0);
    Vector3 targetPos;
    float targetDeg;
    int actTblNo = 0;

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

    void Start()
    {
        hb = GetComponent<HitBase>();
        spComp = GetComponent<SpriteRenderer>();
        plObj = StageManager.Ins.PlObj;

        defScale = transform.localScale;

        titanFTbl = new dFunc[(int)TitanActNo.Num];
        titanFTbl[0] = atkCrash;
        titanFTbl[1] = atkSlide;
        titanFTbl[2] = atkPressure;
        titanFTbl[3] = atkSammon;
        titanFTbl[4] = atkBombCircle;
        titanFTbl[5] = atkBombLine;
        titanFTbl[6] = atkBombCross;

        // それぞれの初期化
        switch (atkType) {
            case AtkType.Bomb:
                atkCounter = BombCount;
                break;
            case AtkType.GazeSector:
                atkCounter = GazeCount;
                break;
            case AtkType.BossTitan:
                TitanSetNextAct();
                break;
            case AtkType.LandSlide:
                atkCounter = LandSlideCount;
                break;
            case AtkType.Jail:
                atkCounter = JailCount;
                atkRno = 0;
                moveVec = CrashDownSpd;
                break;
        }
    }

    void Update()
    {
        if (StageManager.Ins.CheckStop()) { return; }

        if (StageManager.Ins.CheckForceEscape()) { escapeFlag = true; }

        // 強制終了タイマー
        escapeTime -= Time.deltaTime;
        if (escapeTime <= 0) {
            escapeFlag = true;
        }

        hb.PreUpdate();
        if (hb.CheckDie() || escapeFlag)
        {
            if (dieFlag == false)
            {
                StartCoroutine("Die");
                dieFlag = true;
            }
            return;
        }
        else if (hb.CheckDamage()) {
            StartCoroutine("Damage");
        }


        // 移動
        moveControl();

        // 現在の座標から、表示優先を決定させてみる
        spComp.sortingOrder = IObject.GetSpriteOrder(transform.position);

        // 攻撃制御
        atkControl();

        // 拡大縮小設定
        {
            scaleTimer += Time.deltaTime * scaleSpd;

            Vector3 scale = transform.localScale;
            float rad = Mathf.Deg2Rad * scaleTimer;

            scale.x = defScale.x + scaleRate * Mathf.Sin(rad);
            scale.y = defScale.y + scaleRate * Mathf.Sin(rad);

            // 移動した方向を計算
            int moveDir = transform.position.x < oldPos.x ? -1 : 1;
            // 移動方向によって向きが変わる
            scale.x *= defaultDir * moveDir;

            transform.localScale = scale;
        }


        hb.PostUpdate();
    }


    // 移動
    void moveControl()
    {
        oldPos = transform.position;

        switch (moveType) {
            case MoveType.HomingToPL:
                if (plObj == null) { return; }
                // PL方向へ向かう
                {
                    float radian =
                        Mathf.Atan2(plObj.transform.position.y - transform.position.y,
                                    plObj.transform.position.x - transform.position.x);
                    moveVec = Vector3.zero;
                    moveVec.x = moveSpd * Mathf.Cos(radian);
                    moveVec.y = moveSpd * Mathf.Sin(radian);
                }
                transform.position += moveVec * Time.deltaTime;
                break;

            case MoveType.StraightToPL:
                if (moveRno == 0) {
                    float radian =
                        Mathf.Atan2(plObj.transform.position.y - transform.position.y,
                                    plObj.transform.position.x - transform.position.x);
                    moveVec = Vector3.zero;
                    moveVec.x = moveSpd * Mathf.Cos(radian);
                    moveVec.y = moveSpd * Mathf.Sin(radian);
                    ++moveRno;
                }
                transform.position += moveVec * Time.deltaTime;
                break;

            case MoveType.LToR:
            case MoveType.RToL:
                if (moveRno == 0)
                {
                    moveVec.x = moveType == MoveType.LToR ? moveSpd : -moveSpd;
                    moveVec.y = 0;
                    ++moveRno;
                }
                transform.position += moveVec * Time.deltaTime;
                break;

            case MoveType.UToD:
            case MoveType.DToU:
                if (moveRno == 0)
                {
                    moveVec.x = 0;
                    moveVec.y = moveType == MoveType.UToD ? -moveSpd : moveSpd;
                    ++moveRno;
                }
                transform.position += moveVec * Time.deltaTime;
                break;

            case MoveType.None:
                break;
        }
    }

    // 攻撃
    void atkControl()
    {
        switch (atkType) {
            case AtkType.MoveOnly:
                break;

            case AtkType.Bomb:
                // PLに接近したかチェック
                if (!nearFlag) {
                    float dist = Vector2.Distance(plObj.transform.position, transform.position);
                    if (dist <= nearDistance)
                    {
                        nearFlag = true;
                        atkCounter = BombCount + 0.1f;
                        // 予兆を起動
                        GameObject signObj = Instantiate(SignObj);
                        // 予兆側に攻撃もセット
                        signObj.GetComponent<AtkSign>().
                            SetUp(gameObject,
                            AtkSign.SignType.Circle,
                            BombEffOfs,
                            150,
                            BombCount,
                            ShellObj);
                        break;
                    }
                }

                // 爆発
                if (nearFlag) {
                    atkCounter -= Time.deltaTime;
                    if (atkCounter <= 0) {
                        // 爆発して死ぬ
                        Destroy(gameObject);
                    }
                }
                break;

            case AtkType.GazeSector:
                // 視線攻撃
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0) {
                    atkCounter = GazeCount + GazeSignCount;
                    // 視線起動
                    GameObject signObj = Instantiate(SignObj);
                    // 予兆側に攻撃もセット
                    signObj.GetComponent<AtkSign>().
                        SetUp(gameObject,
                        AtkSign.SignType.Sec45,
                        GazeOfs,
                        520,
                        GazeSignCount,
                        ShellObj,
                        AtkSign.ShellType.Gaze);
                }
                break;

            case AtkType.Explosion:
                // 爆発
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0)
                {
                    atkCounter = ExplodeCount + ExplodeSignCount;
                    // 視線起動
                    GameObject signObj = Instantiate(SignObj);
                    // 予兆側に攻撃もセット
                    signObj.GetComponent<AtkSign>().
                        SetUp(gameObject,
                            AtkSign.SignType.Circle,
                        BombEffOfs,
                        400,
                        ExplodeSignCount,
                        ShellObj,
                        AtkSign.ShellType.Bomb);
                }
                break;

            case AtkType.BossTitan:
                // タイタン
                atkTitanFunc();
                break;

            case AtkType.LandSlide:
                if (atkRno == 0)
                {
                    atkCounter -= Time.deltaTime;
                    if (atkCounter <= 0)
                    {
                        // 予兆を起動 
                        float radian = Mathf.Atan2(plObj.transform.position.y - transform.position.y,
                           plObj.transform.position.x - transform.position.x);
                        targetDeg = radian * Mathf.Rad2Deg;
                        {
                            GameObject obj = Instantiate(SignObj, targetPos, Quaternion.identity);
                            obj.GetComponent<AtkSign>().SetUpRect(gameObject, 1200, LandSlideSignCount, targetDeg);
                        }
                        atkCounter = LandSlideSignCount;
                        atkRno = 1;
                    }
                }
                else {
                    atkCounter -= Time.deltaTime;
                    if (atkCounter <= 0)
                    {
                        Vector3 bootPos = transform.position;
                        bootPos.y += 60.0f;
                        {
                            GameObject obj = Instantiate(ShellObj, bootPos, Quaternion.identity);
                            obj.GetComponent<EShellBooter>().SetUp(targetDeg);
                        }
                        atkCounter = LandSlideCount;
                        atkRno = 0;
                    }
                }
                break;

            case AtkType.Jail:
                // ジェイル
                if (atkRno == 0)
                {
                    transform.position += moveVec * Time.deltaTime;
                    if (transform.position.y <= targetPos.y)
                    {
                        SeManager.Instance.Play("Sand");
                        transform.position = targetPos;
                        ++atkRno;
                    }
                }
                else if (atkRno == 1)
                {
                    atkCounter -= Time.deltaTime;
                    if (atkCounter <= 0)
                    {
                        GameObject bombObj = Instantiate(ShellObj, transform.position, Quaternion.identity);
                        bombObj.GetComponent<EShellBomb>().SetUp(300);
                        // 爆発して死ぬ
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }

    // タイタン用
    void atkTitanFunc()
    {
        // タイタンの攻撃方法
        // ジオクラ＞ランスラ＞重み＞雑魚呼び出し＞三列ボム＞十字ボム＞円ボム
        titanFTbl[actRno]();
    }

    void setAtkRno(TitanActNo rno) {
        actRno = (int)rno;
        atkRno = 0;
    }
    void atkCrash() {
        switch (atkRno) {
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
                if ( atkCounter <= 0  ) {
                    // ここで表示を消す
                    spComp.material.color = new Color(0,0,0,0);

                    // 目標座標を設定
                    targetPos = plObj.transform.position;
                    // 予兆を起動
                    GameObject obj = Instantiate(SignObj, targetPos, Quaternion.identity);
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
                if (atkCounter <= 0) {
                    Vector3 setPos = targetPos;
                    setPos.y += 800.0f;
                    transform.position = setPos;
                    moveVec = CrashDownSpd;
                    spComp.material.color = new Color(0.5f, 0.5f, 0.5f, 1);
                    atkRno++;
                }
                break;

            case 3:
                // 落下
                transform.position += moveVec * Time.deltaTime;
                if (transform.position.y <= targetPos.y ) {
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
                if (atkCounter <= 0 ) {
                    TitanSetNextAct();
                }
                break;
        }
    }

    void atkSlide()
    {
        switch (atkRno){
            case 0:
                // 予兆を起動 
                float radian = Mathf.Atan2(plObj.transform.position.y - transform.position.y,
                   plObj.transform.position.x - transform.position.x);
                targetDeg = radian * Mathf.Rad2Deg;
                {
                    GameObject obj = Instantiate(SignObj, targetPos, Quaternion.identity);
                    obj.GetComponent<AtkSign>().SetUpRect(gameObject, 1200, SlideSignCount, targetDeg);

                    obj = Instantiate(SignObj, targetPos, Quaternion.identity);
                    obj.GetComponent<AtkSign>().SetUpRect(gameObject, 1200, SlideSignCount, targetDeg + 45);

                    obj = Instantiate(SignObj, targetPos, Quaternion.identity);
                    obj.GetComponent<AtkSign>().SetUpRect(gameObject, 1200, SlideSignCount, targetDeg - 45);
                }

                atkCounter = SlideSignCount;
                atkRno++;
                atkSlide();
                break;

            case 1:
                // 待ち
                atkCounter -= Time.deltaTime;
                if (atkCounter <= 0) {
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
                    GameObject obj = Instantiate(SignObj, targetPos, Quaternion.identity);
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
                        GameObject obj = Instantiate(SignObj, targetPos, Quaternion.identity);
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
        switch (atkRno){
            case 0:
                // 予兆を起動 
                targetPos = transform.position;
                targetPos.y -= 40.0f;

                for (int no = 0; no < 2; ++no) {
                    Vector3 bootPos = targetPos;
                    bootPos.x += SammonOfsX * (no == 0 ? -1 : 1);
                    GameObject obj = Instantiate(SignObj, bootPos, Quaternion.identity);
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
        switch (atkRno){
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
                if (atkCounter <= 0){
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
                    if (atkIctr0 >= 8){
                        // 次へ
                        atkCounter = BombCircleEndCount;
                        ++atkRno;
                    }
                    else{
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
                for( int lp = 1; lp < 2; ++lp ){
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
        if (actTblNo >= titanActTbl.Length) {
            actTblNo = 0;
        }
    }


    // ダメージ
    IEnumerator Damage() {
        spComp.material.color = Color.white;

        yield return new WaitForSeconds(0.05f);

        spComp.material.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }

    // 死亡
    IEnumerator Die()
    {
        hb.SetAtkActive(false);
        hb.SetDefActive(false);

        // 強制終了の時は行わない
        if (!escapeFlag)
        {
            DropManager.Ins.BootDropItem(transform.position, dType);
            StageManager.Ins.PlScr.KillNum++;
        }
        GetComponent<BoxCollider2D>().enabled = false;

        float counter = 1.0f;
        float strengthY = 0;

        spComp.material = dieMat;

        if (atkType == AtkType.BossTitan)
        {
            BgmManager.Instance.StopImmediately();
            SeManager.Instance.Play("Bomb1");
        }

        do
        {
            spComp.material.SetFloat("_Alpha", counter);
            spComp.material.SetFloat("_StrengthY", strengthY);

            counter -= Time.deltaTime * 2;
            strengthY -= Time.deltaTime * 2;

            yield return new WaitForSeconds(Time.deltaTime);
        }while(counter > 0);

        // タイタン倒したら終了
        if (atkType == AtkType.BossTitan) {
            StageManager.Ins.SetClear();
        }

        Destroy(gameObject);
    }

    // ジェイル用セットアップ
    public void JailSetUp() {
        Vector3 pos = transform.position;
        targetPos = pos;
        pos.y += JailOfsY;
        transform.position = pos;
    }
}
