using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShellShotControl : MonoBehaviour
{
    public GameObject shotObj;
    GameObject plObj;
    Player plScr;
    int shotNum = 1;

    float radian;
    float shotCtr = 0;

    Player.Weapon weapon;
    Player.Ability ability;

    const float shotInterval = 0.08f;@  // ”­ËŠÔŠu

    void Start()
    {
        if( plObj == null ){
            plObj = StageManager.Ins.PlObj;
            plScr = StageManager.Ins.PlScr;
        }
    }

    void Update()
    {
        if (plScr.CheckDie()) { Destroy(gameObject); return; }

        shotCtr -= Time.deltaTime;
        if (shotCtr <= 0) {
            GameObject obj = Instantiate(shotObj, plScr.GetCenterPos(), Quaternion.identity);
            obj.GetComponent<PLShellShot>().SetShotPara(plObj, weapon, ability, radian);

            SeManager.Instance.Play("Shot");

            shotNum--;
            if ( shotNum <= 0 ) {
                Destroy(gameObject);
            }
            shotCtr = shotInterval;
        }
    }

    public void SetShotCtlPara(GameObject pl, Player.Weapon wep, Player.Ability abi, float setRad)
    {
        // ‹¤’Ê
        plObj = pl;
        plScr = pl.GetComponent<Player>();
        weapon = wep;
        ability = abi;

        // ƒAƒrƒŠƒeƒB”½‰f
        shotNum = plScr.GetWeaponShotNum(wep.atkNum, abi.addShotNum);   // ’e”‰ÁZ

        // •ŠíŒÂ•Ê
        radian = setRad;
    }

}
