using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.IO;
    
public class CreateSOBJ : EditorWindow
{
    //パスをまとめたスクリプタブルオブジェクトのパス
    private readonly string PathSOBJ = "Assets/Editor/CreateSOBJ/Resources/CreateSOBJPath.asset";
    private CreateSOBJPath pathSOBJ;

    const string EmDataPath = "EnemyData";
    const string WepInitDataPath = "WeaponInitData";
    const string WepLVUPDataPath = "WeaponLVUPData";

    // 初期化
    //[MenuItem("Tools/Create SObj")]
    //static void Init()
    //{
    //    CreateSOBJ window = (CreateSOBJ)EditorWindow.GetWindow(typeof(CreateSOBJ));
    //    window.Show();
    //}

    // 表示関連
    private void OnGUI()
	{
        GUILayout.Label("SObj Creator", EditorStyles.boldLabel);
        GUILayout.Label("");

        // パス設定を開く
        GUILayout.Label("■パス関連の設定");
        if (GUILayout.Button("Open PathSOBJ"))
        {
            OpenPath();
        }

        // URL開く
        GUILayout.Label("■スプレットシートを開く");
        if (GUILayout.Button("Open Sheets"))
        {
            OpenURL();
        }

        // 敵データをダウンロード
        GUILayout.Label("■敵データダウンロード");
        if (GUILayout.Button("Enemy Data Download"))
        {
            DownLoadData(EmDataPath);
        }

        // ボタンを押して ScriptableObject を作成
        GUILayout.Label("■敵データ生成");
        if (GUILayout.Button("Create EnemySObj ALL"))
        {
            CreateEnemySObj();
        }


        // 武器データをダウンロード
        GUILayout.Label("■PL武器初期パラメータダウンロード");
        if (GUILayout.Button("WeaponInitData Download"))
        {
            DownLoadData("WeaponInitData");
        }

        // ボタンを押して ScriptableObject を作成
        GUILayout.Label("■PL武器初期パラメータ生成");
        if (GUILayout.Button("Create WeaponInitSObj ALL"))
        {
            CreateWeaponInitObj();
        }

        // 武器レベルアップ関連をダウンロード
        GUILayout.Label("■PL武器レベルアップ設定ダウンロード");
        if (GUILayout.Button("Weapon LVUP Setting Download"))
        {
            DownLoadData("WeaponLVUPData");
        }

        // ボタンを押して ScriptableObject を作成
        GUILayout.Label("■PL武器レベルアップデータ生成");
        if (GUILayout.Button("Create Weapon LVUP SOBJ ALL"))
        {
            CreateWeaponLVUPObj();
        }
    }

    // パス再設定
    void ResetPath() {
        pathSOBJ = AssetDatabase.LoadAssetAtPath<CreateSOBJPath>(PathSOBJ);
    }

    // パスオブジェクト開く
    void OpenPath()
    {
        ResetPath();
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(pathSOBJ.SettingSObj_PATH);
        if (obj != null) {
            // ファイルを選択(Projectウィンドウでファイルが選択状態になる)
            Selection.activeObject = obj;
        }
    }

    // シートURL開く
    void OpenURL() {
        ResetPath();
        //パスのサイトを開く
        Application.OpenURL(pathSOBJ.Sheet_URL);
    }



    // データダウンロード
    void DownLoadData(string key)
    {
        ResetPath();

        string mystringValue = key;
        string requestUrl = pathSOBJ.GAS_URL + "?myString=" + UnityWebRequest.EscapeURL(mystringValue);

        //URLへアクセス

        UnityWebRequest req = UnityWebRequest.Get(requestUrl);

        req.SendWebRequest();

        while (!req.isDone)
        {
            // リクエストが完了するのを待機
        }

        if (req.result != UnityWebRequest.Result.ProtocolError && req.result != UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Success");
            string str = req.downloadHandler.text;
            Debug.Log(str);
            string filePath = pathSOBJ.CreateData_PATH;

            filePath += '/' + key + '/' + key + ".json";
            File.WriteAllText(filePath, str);
            Debug.Log(filePath + " Create!!");
        }
        else {
            Debug.Log("Error");
        }
    }


    // データ配列文字列の作成
    string CreateDataTableString(string original)
    {
        string table = "{\"table\":";
        table += original;
        table += '}';
        return table;
    }
    // enumのスクリプトを生成するメソッド
    public static void GenerateEnum(string enumName, List<string> values, string path)
    {
        string enumContent = $"public enum {enumName}\n{{\n";

        // string配列から取得した値を列挙型の要素として追加
        foreach (var value in values)
        {
            enumContent += $"    {value},\n";
        }

        enumContent += "}";

        // enumの定義をスクリプトとして保存
        Debug.Log(enumContent);
        // 既存のファイルが存在する場合、上書き
        System.IO.File.WriteAllText(path, enumContent);
        // 生成したenumをリフレッシュ
        UnityEditor.AssetDatabase.Refresh();
    }


    // 敵ＳＯＢＪ作成
    void CreateEnemySObj()
    {
        string dataPath = "Editor/" + EmDataPath  + '/' + EmDataPath;
        string dataStr = CreateDataTableString(Resources.Load<TextAsset>(dataPath).ToString());
        EnemyJsonDataTable jData = JsonUtility.FromJson<EnemyJsonDataTable>(dataStr);

        // enumの文字列を取得
        List<string> enumNameList = new List<string>();
        // 配列オブジェクトも作ってしまおう
        EnemySOBJDataTable table = ScriptableObject.CreateInstance<EnemySOBJDataTable>(); 

        // JSonからScriptableObject を作成
        for (int no = 0; no < jData.table.Length; ++no)
        {
            EnemySOBJData newObj = ScriptableObject.CreateInstance<EnemySOBJData>();
            // データのコピー
            newObj.id = jData.table[no].id;
            newObj.enumName = jData.table[no].enumName;
            newObj.sprite = jData.table[no].sprite;
            newObj.prefab = jData.table[no].prefab;
            newObj.hp = jData.table[no].hp;
            newObj.speed = jData.table[no].speed;
            newObj.attack = jData.table[no].attack;
            newObj.defense = jData.table[no].defense;

            if (Enum.TryParse(jData.table[no].dropType, out newObj.dropType)) {
            }
            else {
                Debug.LogError("ドロップタイプ変換ミス");
            }

            // 作成した ScriptableObject を保存
            string path = "Assets/Resources/Editor/"+ EmDataPath + '/' + EmDataPath;
            path += newObj.id.ToString();
            path += ".asset";

            AssetDatabase.CreateAsset(newObj, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("ScriptableObject created and saved at: " + path);

            // 配列にも追加
            table.data.Add(newObj);

            // enum用リストに追加
            string enumStr = newObj.enumName;
            enumStr = enumStr.Replace('-', '_');
            enumNameList.Add(enumStr);
        }

        // enum作成
        GenerateEnum("EnemySOBJEnum", enumNameList, $"Assets/Scenes/Stage/Script/CreateFiles/EnemySOBJEnum.cs");

        // テーブルオブジェクトも作成
        string tablePath = "Assets/Resources/Editor/" + EmDataPath + '/' + "EmDataTable.asset";
        AssetDatabase.CreateAsset(table, tablePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ScriptableObject created and saved at: " + tablePath);

        Debug.Log("End...");
    }

    // 武器データSOBJ作成
    void CreateWeaponInitObj()
    {
        string dataPath = "Editor/" + WepInitDataPath + '/' + WepInitDataPath;
        string dataStr = CreateDataTableString(Resources.Load<TextAsset>(dataPath).ToString());
        WeaponInitJsonDataTable jData = JsonUtility.FromJson<WeaponInitJsonDataTable>(dataStr);

        // enumの文字列を取得
        List<string> enumNameList = new List<string>();
        // 配列オブジェクトも作ってしまおう
        WeaponInitSOBJDataTable table = ScriptableObject.CreateInstance<WeaponInitSOBJDataTable>();

        // JSonからScriptableObject を作成
        for (int no = 0; no < jData.table.Length; ++no)
        {
            WeaponInitSOBJData newObj = ScriptableObject.CreateInstance<WeaponInitSOBJData>();
            // データのコピー
            newObj.id = jData.table[no].id;
            newObj.enumName = jData.table[no].enumName;
            newObj.atk = jData.table[no].atk;
            newObj.recastDef = jData.table[no].recastDef;
            newObj.recastMin = jData.table[no].recastMin;
            newObj.speed = jData.table[no].speed;
            newObj.num = jData.table[no].num;
            newObj.time = jData.table[no].time;

            // 作成した ScriptableObject を保存
            string path = "Assets/Resources/Editor/" + WepInitDataPath + '/' + WepInitDataPath;
            path += newObj.id.ToString();
            path += ".asset";

            AssetDatabase.CreateAsset(newObj, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("ScriptableObject created and saved at: " + path);

            // enum用リストに追加
            string enumStr = newObj.enumName;
            enumStr = enumStr.Replace('-', '_');
            enumNameList.Add(enumStr);

            // 配列にも追加
            table.data.Add(newObj);
        }

        // enum作成
        GenerateEnum("WeaponInitSOBJEnum", enumNameList, $"Assets/Scenes/Stage/Script/CreateFiles/WeaponInitSOBJEnum.cs");

        // テーブルオブジェクトも作成
        string tablePath = "Assets/Resources/Editor/" + WepInitDataPath + '/' + "WeaponInitDataTable.asset";
        AssetDatabase.CreateAsset(table, tablePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ScriptableObject created and saved at: " + tablePath);

        Debug.Log("End...");
    }

    // 武器データSOBJ作成
    void CreateWeaponLVUPObj()
    {
        string dataPath = "Editor/" + WepLVUPDataPath + '/' + WepLVUPDataPath;
        string dataStr = CreateDataTableString(Resources.Load<TextAsset>(dataPath).ToString());
        WeaponLVUPJsonDataTable jData = JsonUtility.FromJson<WeaponLVUPJsonDataTable>(dataStr);

        // enumの文字列を取得
        List<string> enumNameList = new List<string>();
        // 配列オブジェクトも作ってしまおう
        WeaponLVUPSOBJDataTable table = ScriptableObject.CreateInstance<WeaponLVUPSOBJDataTable>();

        // JSonからScriptableObject を作成
        for (int no = 0; no < jData.table.Length; ++no)
        {
            WeaponLVUPSOBJData newObj = ScriptableObject.CreateInstance<WeaponLVUPSOBJData>();
            // データのコピー
            newObj.id = jData.table[no].id;
            newObj.weaponName = jData.table[no].weaponName;
            newObj.key = new string[7];
            newObj.key[0] = jData.table[no].key1;
            newObj.key[1] = jData.table[no].key2;
            newObj.key[2] = jData.table[no].key3;
            newObj.key[3] = jData.table[no].key4;
            newObj.key[4] = jData.table[no].key5;
            newObj.key[5] = jData.table[no].key6;
            newObj.key[6] = jData.table[no].key7;
            newObj.value = new float[7];
            newObj.value[0] = jData.table[no].value1;
            newObj.value[1] = jData.table[no].value2;
            newObj.value[2] = jData.table[no].value3;
            newObj.value[3] = jData.table[no].value4;
            newObj.value[4] = jData.table[no].value5;
            newObj.value[5] = jData.table[no].value6;
            newObj.value[6] = jData.table[no].value7;

            // 作成した ScriptableObject を保存
            string path = "Assets/Resources/Editor/" + WepLVUPDataPath + '/' + WepLVUPDataPath;
            path += newObj.id.ToString();
            path += ".asset";

            AssetDatabase.CreateAsset(newObj, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("ScriptableObject created and saved at: " + path);

            // enum用リストに追加
            string enumStr = newObj.weaponName;
            enumStr = enumStr.Replace('-', '_');
            enumNameList.Add(enumStr);

            // 配列にも追加
            table.data.Add(newObj);
        }

        // enum作成
        GenerateEnum("WeaponLVUPSOBJEnum", enumNameList, $"Assets/Scenes/Stage/Script/CreateFiles/WeaponLVUPSOBJEnum.cs");

        // テーブルオブジェクトも作成
        string tablePath = "Assets/Resources/Editor/" + WepLVUPDataPath + '/' + "WeaopnLVUPDataTable.asset";
        AssetDatabase.CreateAsset(table, tablePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ScriptableObject created and saved at: " + tablePath);

        Debug.Log("End...");
    }
}
