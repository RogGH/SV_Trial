using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    // プレイヤーの処理
    void playerCore() 
    {
        // ここにキー制御命令を入力
        KeyInputUpdate();

        // ここに武器制御命令を入力
        WeaponControl();
    }

    // キー入力
    void KeyInputUpdate()
    {
        float lr = Input.GetAxisRaw("Horizontal");
        float ud = Input.GetAxisRaw("Vertical");

        if (lr != 0 || ud != 0)
        {

            float spd = CalcRateToValue(MoveSpeed, ability.moveSpdRate);
            float rate = 1.0f;
            if (lr != 0 && ud != 0) { rate = 0.71f; }   // 斜め移動対策
            spd *= rate * 60 * Time.deltaTime;
            transform.position += new Vector3(spd * lr, spd * ud, 0);

            ChangeAnim(AnimNo.Move);
        }
        else
        {
            ChangeAnim(AnimNo.Idle);
        }

        if (lr != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * lr;
            scale.x *= defaultDir;
            transform.localScale = scale;
        }
    }

    // 武器制御
    void WeaponControl()
    {
        for (int i = 0; i < weaponTblNum; ++i)
        {
            if (WeaponTbl[i].initFlag == true)
            {                    // 初期化チェック
                WeaponTbl[i].recastCounter -= Time.deltaTime;       // タイマーチェック
                if (WeaponTbl[i].recastCounter <= 0)
                {              // リキャストチェック
                    int sNo = WeaponTbl[i].serialNo;
                    WeaponBoot(sNo, ref WeaponTbl[i]);            // 起動

                    // リキャスト時間設定
                    float setRecast = WeaponManager.Ins.GetWeaponRecastMin(sNo);    // 最低リキャストを設定
                    float calcRate = 1 - (WeaponTbl[i].recastRate + ability.recastRate) / 100;  // レート計算
                    if (calcRate <= 0) { calcRate = 0; }                            // 

                    float calcRecast = WeaponManager.Ins.GetWeaponRecastCalc(sNo);  // 最大-最小の値を取得
                    setRecast += calcRecast * calcRate;
                    WeaponTbl[i].recastCounter = setRecast;
                    WeaponTbl[i].recastTime = WeaponTbl[i].recastCounter;
                }
            }
        }
    }


    void WeaponBoot(int sNo, ref Weapon wep)
    {

        int lv = wep.level;
        GameObject wepObj = WeaponManager.Ins.WeaponObjTbl[sNo];

        switch (sNo)
        {
            case 0:
                // ハンドガン
                {
                    GameObject obj = Instantiate(wepObj, transform.position, Quaternion.identity);
                    obj.GetComponent<PLShellShotControl>().SetShotCtlPara(gameObject, wep, ability, GetAngleToPLMouse());
                }
                break;

            case 1:
                // ドリル
                {
                    GameObject obj = Instantiate(wepObj, GetCenterPos(), Quaternion.identity);
                    obj.GetComponent<PLShellDrill>().SetDrillPara(gameObject, wep, ability);
                    SeManager.Instance.Play("Drill");
                }
                break;

            case 2:
                // 回転のこぎり
                {
                    for (int i = 0; i < wep.atkNum; ++i)
                    {
                        GameObject obj = Instantiate(wepObj, GetCenterPos(), Quaternion.identity);
                        obj.GetComponent<PLShellSaw>().SetSawPara(gameObject, wep, ability, i);
                    }

                    SeManager.Instance.Play("Saw");
                }
                break;

            case 3:
                // 火炎放射
                {
                    GameObject obj = Instantiate(wepObj, transform.position, Quaternion.identity);
                    obj.GetComponent<PLShellFireControl>().SetFireCtlPara(gameObject, wep, ability);

                    SeManager.Instance.Play("Fire");
                }
                break;

            case 4:
                // タレット
                {
                    for (int i = 0; i < wep.atkNum; ++i)
                    {
                        Vector3 pos = transform.position;
                        float radian = GetAngleToPLMouse();
                        float ofs = 50 * (i == 0 ? -1 : 1);
                        pos.x += Mathf.Cos(radian) * ofs;
                        pos.y += Mathf.Sin(radian) * ofs;
                        GameObject obj = Instantiate(wepObj, pos, Quaternion.identity);
                        obj.GetComponent<PLShellTurretBody>().SetTurretBodyPara(gameObject, wep, ability);
                    }
                }
                break;

            case 5:
                // 検知
                {
                    SeManager.Instance.Play("Monitor");

                    for (int i = 0; i < wep.atkNum; ++i)
                    {
                        GameObject obj = Instantiate(wepObj, transform.position, Quaternion.identity);
                        float dir;
                        if (wep.atkNum == 1)
                        {
                            dir = (Random.Range(0, 2) == 0) ? -1 : 1;   // ランダムで左右どちらか
                        }
                        else
                        {
                            dir = (i == 0 ? -1 : 1);                    // 両方
                        }
                        obj.GetComponent<PLShellMonitor>().SetWaveMonitorPara(gameObject, wep, ability, dir);
                    }
                }
                break;
        }
    }

}
