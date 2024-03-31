// スクリプタブルオブジェクトからの読み込み
//#define GDataLoadSObj

// JSONからの読み込み
//#define GDataLoadJson

// スクリプトからの読み込み
#define GDataLoadScript

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class EnemyGenerator2 : MonoBehaviour
{
    //private static EnemyGenerator2 ins;
    //public static EnemyGenerator2 Ins
    //{
    //    get
    //    {
    //        if (ins == null)
    //        {
    //            ins = (EnemyGenerator2)FindObjectOfType(typeof(EnemyGenerator2));
    //            if (ins == null) { Debug.LogError(typeof(EnemyGenerator2) + "is nothing"); }
    //        }
    //        return ins;
    //    }
    //}

    const string GeneDataPath = "Stage/";
    const string debugPath = "/Debug/";                 // デバッグ用
    const string debugJson = "AdvData.json";			// デバッグのファイル名

    const float BootWidth = 140.0f; // 敵起動の幅
    // 
    GameObject plObj;
    Player plScr;
    EnemyManager emMng;
    // 
    GenerateData[] gDataTbl = new GenerateData[(int)GLine.Num];
    float[] gLineTimer = new float[(int)GLine.Num];

    int geneSPTblNo = 0;            // 特殊テーブル番号


    // 敵のデータをどうやって読み込むのがいいか
#if GDataLoadSObj
    public GeneSObjData sObjData;
#endif
#if GDataLoadJson
    [SerializeField] JSonGeneData jsonData; // ジェイソンから
#endif

    GenerateData[] geneTblData;                 // 実データ
    GenerateSPData[] geneSPTblData;             // 実データ
   
    void Awake()
    {
    }

    void Start()
    {
        plObj = StageManager.Ins.PlObj;
        plScr = StageManager.Ins.PlScr;
        emMng = EnemyManager.Ins;

        int stgNo = (int)SystemManager.Ins.selStgNo;

        //stgNo = (int)StageNo.Practice;
#if GDataLoadSObj
        geneTblData = sObjData.geneTbl;
#endif
#if GDataLoadJson
        // ジェイソンからデータ読み込み
        string dataPath = GeneDataPath;
        dataPath += "Stage1Data";
        jsonData = loadTextData(dataPath);
        geneTblData = jsonData.geneTbl;
#endif
#if GDataLoadScript
        geneTblData = GeneDataCtl.geneJagTbl[stgNo];
        geneSPTblData = GeneSPDataCtl.geneSPJagTbl[stgNo];
#endif
    }

    void Update()
    {
        if (plScr.CheckDie()) { return; }

        // 通常起動
        float stgTime = StageManager.Ins.StageTime;

        // 生産工場
        for (int lineNo = 0; lineNo < (int)GLine.Num; ++lineNo)
        {
            if (gDataTbl[lineNo] != null)
            {
                GenerateData data = gDataTbl[lineNo];
                if (data.start <= stgTime && stgTime <= data.end)
                {
                    gLineTimer[lineNo] -= Time.deltaTime;
                    if (gLineTimer[lineNo] <= 0)
                    {
                        generateEnemy(data);
                        gLineTimer[lineNo] = data.interval;
                    }
                }
                else
                {
                    // 解除
                    gDataTbl[lineNo] = null;
                }
            }
        }


        // 全てのテーブルをチェック
        for (int tblNo = 0; tblNo < geneTblData.Length; ++tblNo)
        {
            GenerateData data = geneTblData[tblNo];
            // 生産時間チェック
            if (data.start <= stgTime && stgTime <= data.end) {
                // 生産ラインに載せる
                GenerateData chkData = gDataTbl[data.lineNo];

                // 既にラインが登録されているかチェック
                if (chkData != null)
                {
                    if (chkData != data)
                    {
                        if (chkData.lineNo == data.lineNo)
                        {
                            Debug.Assert(false, "Already Line Used!! line " + data.lineNo + "tblNo = " + tblNo);
                        }
                    }
                }

                // ない場合は登録できる
                if ( chkData == null ){
                    // 起動
                    generateEnemy(data);

                    gDataTbl[data.lineNo] = data;
                    gLineTimer[data.lineNo] = data.interval;
                }
            }
        }

        // 特殊起動
        if (geneSPTblNo < geneSPTblData.Length)
        {
            if (stgTime >= geneSPTblData[geneSPTblNo].start)
            {
                GenerateSPData spData = geneSPTblData[geneSPTblNo];
                generateSPEnemy(spData);
                // テーブル変更
                geneSPTblNo++;
            }
        }
    }

    // 敵起動
    void generateEnemy(GenerateData data)
    {
        GeneType gType = GeneType.Default;
        float sclX = 1280 / 2;
        float sclY = 720 / 2;
        float offset = 40;
        float radius = Mathf.Sqrt(sclX * sclX + sclY * sclY) + offset;
        float limitX = sclX + offset;
        float limitY = sclY + offset;
        int tNo = (int)data.emNo;
        Vector2 pos;

        switch (gType)
        {
            case GeneType.Default:
            case GeneType.Boss:
                // 基本
                {
                    // ランダム座標に起動
                    float degree = UnityEngine.Random.Range(0, 360.0f);
                    float radian = Mathf.Deg2Rad * degree;

                    float setX = radius * Mathf.Cos(radian);
                    if (Mathf.Abs(setX) > limitX) { setX = limitX * Mathf.Sign(setX); }
                    float setY = radius * Mathf.Sin(radian);
                    if (Mathf.Abs(setY) > limitY) { setY = limitY * Mathf.Sign(setY); }

                    pos.x = plObj.transform.position.x + setX;
                    pos.y = plObj.transform.position.y + setY;

                    InsEnemy(tNo, pos);
                }
                break;
        }
    }

    // 敵起動
    void generateSPEnemy(GenerateSPData spData)
    {
        GeneType gType = (GeneType)spData.geneType;
        float sclX = 1280 / 2;
        float sclY = 720 / 2;
        float offset = 40;
        float radius = Mathf.Sqrt(sclX * sclX + sclY * sclY) + offset;
        float limitX = sclX + offset;
        float limitY = sclY + offset;
        int tNo = (int)spData.emNo;
        Vector2 pos;

        switch (gType){

            case GeneType.Default:
            case GeneType.Boss:
                // 基本
                // ランダム座標に起動
                {
                    float degree = UnityEngine.Random.Range(0, 360.0f);
                    float radian = Mathf.Deg2Rad * degree;

                    float setX = radius * Mathf.Cos(radian);
                    if (Mathf.Abs(setX) > limitX) { setX = limitX * Mathf.Sign(setX); }
                    float setY = radius * Mathf.Sin(radian);
                    if (Mathf.Abs(setY) > limitY) { setY = limitY * Mathf.Sign(setY); }

                    pos.x = plObj.transform.position.x + setX;
                    pos.y = plObj.transform.position.y + setY;

                    InsEnemy(tNo, pos);
                }
                break;

            case GeneType.Circle:
            case GeneType.CircleTitan:
                // 円状に配置
                {
                    int loopNum = (int)spData.fTemp0;
                    float degInterval = 360 / loopNum;
                    float shotRadius = radius / 1.25f;
                    float longRadius = radius;
                    if (gType == GeneType.CircleTitan)
                    {
                        shotRadius *= 1.2f;
                        longRadius *= 1.2f;
                    }

                    for (int loopNo = 0; loopNo < loopNum; ++loopNo)
                    {
                        float radian = Mathf.Deg2Rad * (degInterval * loopNo);

                        float setX = longRadius * Mathf.Cos(radian);
                        float setY = shotRadius * Mathf.Sin(radian);

                        pos.x = plObj.transform.position.x + setX;
                        pos.y = plObj.transform.position.y + setY;

                        InsEnemy(tNo, pos);
                    }
                }
                break;

            case GeneType.Group:
                // 集団で起動
                {
                    int loopNum = (int)(spData.fTemp0);
                    float degree = UnityEngine.Random.Range(0, 360.0f);
                    float radian = Mathf.Deg2Rad * degree;
                    float setX = radius * Mathf.Cos(radian);
                    if (Mathf.Abs(setX) > limitX) { setX = limitX * Mathf.Sign(setX); }
                    float setY = radius * Mathf.Sin(radian);
                    if (Mathf.Abs(setY) > limitY) { setY = limitY * Mathf.Sign(setY); }

                    float degInterval = 360 / loopNum;
                    float groupOffset = 40.0f;

                    pos.x = plObj.transform.position.x + setX;
                    pos.y = plObj.transform.position.y + setY;
                    GameObject obj = InsEnemy(tNo, pos);

                    for (int loopNo = 0; loopNo < loopNum; ++loopNo)
                    {
                        float offsetX = groupOffset * Mathf.Cos(degInterval * loopNo);
                        float offsetY = groupOffset * Mathf.Sin(degInterval * loopNo);

                        pos.x = plObj.transform.position.x + setX + offsetX;
                        pos.y = plObj.transform.position.y + setY + offsetY;

                        InsEnemy(tNo, pos);
                    }
                }
                break;


            case GeneType.Vertical:
                // 縦に配置
                {
                    int bootNum = (int)spData.fTemp0;
                    float dir = spData.fTemp1;
                    float width = BootWidth;

                    float setX = plObj.transform.position.x + sclX * dir;
                    float setY = plObj.transform.position.y;
                    setY += width * bootNum / 2;

                    for (int loopNo = 0; loopNo < bootNum; ++loopNo)
                    {
                        pos.x = setX;
                        pos.y = setY - width * loopNo;

                        GameObject obj = InsEnemy(tNo, pos);
                        // 移動タイプを強制変更
                        obj.GetComponent<EnemyCmm>().moveType =
                            (dir < 0 ? EnemyCmm.MoveType.LToR : EnemyCmm.MoveType.RToL);
                    }
                }
                break;

            case GeneType.Horizontal:
                // 横に配置
                {
                    int bootNum = (int)spData.fTemp0;
                    float dir = spData.fTemp1;
                    float width = BootWidth;

                    float setY = plObj.transform.position.y + sclY * dir;
                    float setX = plObj.transform.position.x;
                    setX -= width * bootNum / 2;

                    for (int loopNo = 0; loopNo < bootNum; ++loopNo)
                    {
                        pos.x = setX + width * loopNo;
                        pos.y = setY;

                        GameObject obj = InsEnemy(tNo, pos);
                        // 移動タイプを強制変更
                        obj.GetComponent<EnemyCmm>().moveType =
                            (dir < 0 ? EnemyCmm.MoveType.DToU : EnemyCmm.MoveType.UToD);
                    }
                }
                break;

        }
    }


    // 敵生成
    public GameObject InsEnemy(int sobjID, Vector2 pos)
    {
        EnemySOBJData sobj = EnemyManager.Ins.GetEnemySOBJData(sobjID);

        // データから名前を取得
        string path = "Prefabs/EnemyP/" + sobj.prefab;
        GameObject prefab = Resources.Load<GameObject>(path);
        // 起動
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        obj.GetComponent<EnemyCmm>().SetSOBJData(sobj);
        return obj;
    }


    // データ読み込み
    public JSonGeneData loadTextData(string path)
    {
        string datastr = "";

        //if (false)
        //{
        //    // デバッグの場合はアプリケーションパスの直下のsvedata.jsonを読み込む
        //    // 実行ファイル作成後もファイル名_dataフォルダの中に入れればOK
        //    path = Application.dataPath + debugPath + debugJson;
        //    Debug.Log(path);
        //    StreamReader reader;
        //    reader = new StreamReader(path);

        //    // 文字列を読み込み
        //    datastr = reader.ReadToEnd();
        //    reader.Close();
        //}
        //else
        {
            // 通常ファイル
            // テキストファイルとしてジェイソンを読み込み
            Debug.Log(path);
            datastr = Resources.Load<TextAsset>(path).ToString();
        }
        // ジェイソンに変換
        return JsonUtility.FromJson<JSonGeneData>(datastr);
    }
}