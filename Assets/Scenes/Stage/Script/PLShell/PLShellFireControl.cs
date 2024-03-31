using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShellFireControl : MonoBehaviour
{
    public GameObject fire;
    public GameObject poison;

    GameObject plObj;
    Player plScr;

    Player.Weapon weapon;
    Player.Ability ability;

    float time = 0;
    int shotMaxNum;
    int shotNum = 0;
    float shotInterval = 0.1f;
    float angleInterval = 20;
    float initRadian;

    void Start()
    {
        if (plObj == null)
        {
            plObj = StageManager.Ins.PlObj;
            plScr = StageManager.Ins.PlScr;
        }
        initRadian = plScr.GetAngleToPLMouse();
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManager.Ins.CheckStop()) { return; }
        if (plObj == null) { Destroy(gameObject); }

        // à íuï‚ê≥
        transform.position = plObj.transform.position;

        // 
        time -= Time.deltaTime;
        if (time <= 0) {
            float radian = initRadian;
            float degree = radian * Mathf.Rad2Deg;

            degree += (shotNum - (shotMaxNum / 2)) * -angleInterval;
            float setRad = degree * Mathf.Deg2Rad;

            Vector3 bootPos = transform.position;
            bootPos.y += 20.0f;
            GameObject obj = Instantiate(fire, bootPos, Quaternion.identity);
            obj.GetComponent<PLShellFire>().SetFirePara(plObj, weapon, ability, setRad, 0);

            if (weapon.flag1 == true)
            {
                // Ç≥ÇÁÇ…ì≈ï˙éÀÇ‡
                obj = Instantiate(poison, bootPos, Quaternion.identity);
                obj.GetComponent<PLShellFire>().SetFirePara(plObj, weapon, ability, setRad, 1);
            }

            time = shotInterval;
            shotNum++;
            if (shotNum >= shotMaxNum) {
                Destroy(gameObject);
            }
        }
    }

    public void SetFireCtlPara(GameObject pl, Player.Weapon wep, Player.Ability abi) 
    {
        // ã§í 
        plObj = pl;
        plScr = pl.GetComponent<Player>();

        weapon = wep;
        ability = abi;

        // ÉAÉrÉäÉeÉBîΩâf
        shotMaxNum = plScr.GetWeaponShotNum(wep.atkNum, abi.addShotNum);   // íeêîâ¡éZ
    }
}
