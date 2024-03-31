using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShellTurretBody : MonoBehaviour
{
    public GameObject shotObj;
    public GameObject bombObj;

    GameObject plObj;
    Player plScr;

    Player.Weapon weapon;
    Player.Ability ability;

    const float shotInterval = 0.2f;

    float liveCnt = 4;
    float shotCnt = 0;
    int level;

    // Start is called before the first frame update
    void Start()
    {
        if (plObj == null)
        {
            plObj = StageManager.Ins.PlObj;
            plScr = StageManager.Ins.PlScr;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManager.Ins.CheckStop()) { return; }
        if (WeaponDefine.CameraOut(gameObject)) { Destroy(gameObject); return; }

        shotCnt -= Time.deltaTime;
        if (shotCnt <= 0) {
            GameObject obj = 
                Instantiate(shotObj, transform.position, Quaternion.identity);
            float radian = Random.Range(0, 360) * Mathf.Deg2Rad ;
            obj.GetComponent<PLShellTurretShell>().SetTurretShellPara(plObj, weapon, ability, radian);
            shotCnt = shotInterval;
            SeManager.Instance.Play("TShot", 5);
        }

        liveCnt -= Time.deltaTime;
        if ( liveCnt <= 0 ){
            GameObject obj = Instantiate(bombObj, transform.position, Quaternion.identity);
            obj.GetComponent<PLShellTurretBomb>().SetTurretBombPara(plObj, weapon, ability);
            SeManager.Instance.Play("TBomb", 0);
            // 
            Destroy(gameObject);
        }
    }

    public void SetTurretBodyPara(GameObject pl, Player.Weapon wep, Player.Ability abi)
    {
        // ‹¤’Ê
        plObj = pl;
        plScr = pl.GetComponent<Player>();

        weapon = wep;
        ability = abi;

        WeaponInitSOBJData initData = WeaponManager.Ins.InitTable.data[(int)WeaponInitSOBJEnum.TurretBomb];
        liveCnt = initData.time;
        liveCnt = plScr.GetWeaponAtkTime(liveCnt, wep.timeRate, abi.atkTimeRate);
    }
}
