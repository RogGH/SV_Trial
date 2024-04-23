using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerateDataTable
{
    [Header("出現開始時間（秒）")]public float start;
    [Header("出現終了時間（秒）")]public float end;
    [Header("敵のプレファブ")]public GameObject enemy;
    [Header("出現間隔（秒）")]public float bootTimer;
    [HideInInspector]public float timer = 0;
}

public class EnemyGenerator3 : MonoBehaviour
{
    public GenerateDataTable[] tabel;
    GameObject plObj;
    Player plScr;

    void Start()
    {
        plObj = StageManager.Ins.PlObj;
        plScr = StageManager.Ins.PlScr;
    }

    void Update()
    {
        // プレイヤー死亡チェック
        if (plScr.CheckDie()) { return; }

        // 通常起動
        float stgTime = StageManager.Ins.StageTime;

        // 配列をチェック
        int num = tabel.Length;
        for (int i = 0; i < num; ++i) 
        {
            // データを取得
            GenerateDataTable gData = tabel[i];
            // 時間をチェック
            if (gData.start <= stgTime && stgTime <= gData.end)
            {
                // タイマーチェック
                gData.timer -= Time.deltaTime;
                if (gData.timer <= 0)
                {
                    // 再度時間を設定
                    gData.timer = gData.bootTimer;

                    float sclX = 1280 / 2;
                    float sclY = 720 / 2;
                    float offset = 40;
                    float radius = Mathf.Sqrt(sclX * sclX + sclY * sclY) + offset;
                    float limitX = sclX + offset;
                    float limitY = sclY + offset;
                    Vector2 pos;
                    float degree = UnityEngine.Random.Range(0, 360.0f);
                    float radian = Mathf.Deg2Rad * degree;

                    float setX = radius * Mathf.Cos(radian);
                    if (Mathf.Abs(setX) > limitX) { setX = limitX * Mathf.Sign(setX); }
                    float setY = radius * Mathf.Sin(radian);
                    if (Mathf.Abs(setY) > limitY) { setY = limitY * Mathf.Sign(setY); }

                    pos.x = plObj.transform.position.x + setX;
                    pos.y = plObj.transform.position.y + setY;

                    // 
                    Instantiate(gData.enemy, pos, Quaternion.identity);
                }
            }
        }
    }
}
