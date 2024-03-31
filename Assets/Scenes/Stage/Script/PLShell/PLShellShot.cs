using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShellShot : MonoBehaviour
{
    public HitAtkData atkData;

    HitBase hb;
    SpriteRenderer spComp;

    Player plScr;

    float radian;
    Vector3 initScl;
    Vector3 vel = Vector3.zero;
    float moveSpd;
    int hitNum = 1;

    void Start()
    {
        hb = GetComponent<HitBase>();
        spComp = GetComponent<SpriteRenderer>();

        // 
        vel.x = moveSpd * Mathf.Cos(radian);
        vel.y = moveSpd * Mathf.Sin(radian);

        Vector3 localRot = transform.localEulerAngles;
        localRot.z = Mathf.Rad2Deg * radian;
        transform.localEulerAngles = localRot;

        Destroy(gameObject, WeaponDefine.LiveCountDef);
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManager.Ins.CheckStop()) { return; }
        if (WeaponDefine.CameraOut(gameObject)) { Destroy(gameObject); return; }

        hb.PreUpdate(); 
        
        if (hb.CheckAttack()) {
            --hitNum;
            if (hitNum <= 0) {
                Destroy(gameObject);
            }
        }

        transform.position += vel * Time.deltaTime;
        // åªç›ÇÃç¿ïWÇ©ÇÁÅAï\é¶óDêÊÇåàíËÇ≥ÇπÇƒÇ›ÇÈ
        spComp.sortingOrder = IObject.GetSpriteOrder(transform.position);

        hb.PostUpdate();
    }

    // ÉpÉâÉÅÅ[É^ê›íË
    public void SetShotPara(GameObject pl, Player.Weapon wep, Player.Ability abi, float setRad)
    {
        plScr = pl.GetComponent<Player>();
        WeaponInitSOBJData initData = WeaponManager.Ins.InitTable.data[(int)WeaponInitSOBJEnum.HandGun];

        // çUåÇóÕ
        atkData.AtkPow = plScr.GetWeaponAtkPow(initData.atk, wep.atkRate, abi.dmgRate);
        atkData.CriticalHit = plScr.GetWeaponCriHitRate(wep.criHitRate, abi.criHitRate);
        atkData.CriticalDmg = plScr.GetWeaponCriDmgRate(wep.criDmgRate, abi.criDmgRate);
        // ägëÂó¶
        transform.localScale *= plScr.GetWeaponScaleRate(wep.areaRate, abi.atkAreaRate);

        // íeë¨
        moveSpd = plScr.GetWeaponMoveSpd(initData.speed, wep.speedRate, abi.atkMoveRate);

        // ïêäÌï èàóù
        if (wep.flag1) { hitNum++; }            // ÉtÉâÉOÇPÇ™óßÇ¡ÇƒÇ¢ÇΩÇÁä—í Å{ÇP
        radian = setRad;                        // äpìx
    }
}
