using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// セーブデータ
[System.Serializable]
public class SaveData {
    public int money = 1000;
    public float seVol = 0.5f;
    public float bgmVol = 0.5f;
    public Lang lang = Lang.Japanese;       // 日本語固定
};

// 現状プレイヤープレファスでの実装
// 暗号化はまだ
// 消すときはEdit>Clear All PlayerPrefs


public class SystemManager : MonoBehaviour
{
    // シングルトン実装
    private static SystemManager ins;
    public static SystemManager Ins
    {
        get {
            if (ins == null) {
                ins = (SystemManager)FindObjectOfType(typeof(SystemManager));
                if (ins == null) {
                    Debug.LogError(typeof(SystemManager) + "is nothing");
                }
            }
            return ins;
        }
    }

    // セーブデータ
    public SaveData sData;

    // ゲーム用
    public StageNo selStgNo;
    public CharNo selCharNo;


    // プレイヤープレファスのキー
   const string SaveKey = "SaveJsonKey";

    private void Awake()
    {
        if (this != Ins) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
    }

    void Update()
    {
    }

    // システムデータロード
    void LoadSystemData(string json)
    {
    }


    public void SaveSystemData()
    {
    }
}
