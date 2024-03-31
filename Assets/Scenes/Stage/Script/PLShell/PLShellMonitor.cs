using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShellMonitor : MonoBehaviour
{
    public GameObject beamObj;

    GameObject plObj;
    Player plScr;

    Player.Weapon weapon;
    Player.Ability ability;

    float dir;
    float ofs = 40.0f;
    float time = 2.0f;
    float shellbootTime = 2.0f - 1.0f;
    bool bootFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        plObj = StageManager.Ins.PlObj;
        plScr = StageManager.Ins.PlScr;
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManager.Ins.CheckStop()) { return; }
        if (plObj == null) { Destroy(gameObject); }

        // 位置補正
        Vector3 setPos = plScr.GetCenterPos();
        setPos.x += ofs * dir;
        transform.position = setPos;

        time -= Time.deltaTime;
        if (bootFlag == false) {
            if (time <= shellbootTime)
            {
                // オブジェクト検索
                GameObject tempObj = null;
                foreach (GameObject chkObj in GameObject.FindGameObjectsWithTag("Enemy")) {

                    // 画面内にいるかどうかチェック
                    if (!plScr.CheckPLCameraIn(chkObj.transform.position)) {
                        continue;
                    }

                    if (dir < 0) {
                        // 左側をサーチ
                        if (chkObj.transform.position.x < plObj.transform.position.x) 
                        {
                            tempObj = chkObj;
                            break;
                        }
                    }
                    else {
                        // 右側をサーチ
                        if (chkObj.transform.position.x > plObj.transform.position.x)
                        {
                            tempObj = chkObj;
                            break;
                        }
                    }
                }

                // いなかったらランダムでええか
                if (tempObj == null)
                {
                    foreach (GameObject chkObj in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        // 画面内にいるかどうかチェック
                        if (!plScr.CheckPLCameraIn(chkObj.transform.position))
                        {
                            continue;
                        }

                        // シーン上に存在するオブジェクトならば処理.
                        if (chkObj.activeInHierarchy)
                        {
                            tempObj = chkObj;
                            break;
                        }
                    }
                }
                // さらにいなきゃプレイヤーでOK
                if (tempObj == null) { tempObj = StageManager.Ins.PlObj; }

                // ビーム起動
                GameObject obj = Instantiate(beamObj, tempObj.transform.position, Quaternion.identity);
                obj.GetComponent<PLShellMonitorBeam>().SetWaveBeamPara(plObj, weapon, ability);

                bootFlag = true;
            }
        }

        if (time <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetWaveMonitorPara(GameObject pl, Player.Weapon wep, Player.Ability abi, float setDir) {
        // 共通
        plObj = pl;
        plScr = pl.GetComponent<Player>();

        weapon = wep;
        ability = abi;

        // 専用
        dir = setDir;
    }
}
