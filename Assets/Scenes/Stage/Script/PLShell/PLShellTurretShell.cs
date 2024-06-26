using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShellTurretShell : MonoBehaviour
{
    public HitAtkData atkData;
    SpriteRenderer spComp;
    HitBase hb;
    float radian;
    Vector3 vel = Vector3.zero;
    float moveSpd;

    void Start()
    {
        hb = GetComponent<HitBase>();
        spComp = GetComponent<SpriteRenderer>();

        float setSpd = moveSpd;
        vel.x = setSpd * Mathf.Cos(radian);
        vel.y = setSpd * Mathf.Sin(radian);

        Destroy(gameObject, WeaponDefine.LiveCountDef);
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManager.Ins.CheckStop()) { return; }
        if (WeaponDefine.CameraOut(gameObject)) { Destroy(gameObject); return; }

        hb.PreUpdate();

        if (hb.CheckAttack()) { Destroy(gameObject); }

        transform.position += vel * Time.deltaTime;
        // 現在の座標から、表示優先を決定させてみる
        spComp.sortingOrder = IObject.GetSpriteOrder(transform.position);

        hb.PostUpdate();
    }

    public void SetTurretShellPara(GameObject pl, Player.Weapon wep, Player.Ability abi, float setRad)
    {
        Player plScr = pl.GetComponent<Player>();

        WeaponInitSOBJData initData =
            WeaponManager.Ins.InitTable.data[(int)WeaponInitSOBJEnum.TurretShell];

        // 攻撃力
        atkData.AtkPow = plScr.GetWeaponAtkPow(initData.atk, wep.atkRate, abi.dmgRate);
        atkData.CriticalHit = plScr.GetWeaponCriHitRate(wep.criHitRate, abi.criHitRate);
        atkData.CriticalDmg = plScr.GetWeaponCriDmgRate(wep.criDmgRate, abi.criDmgRate);
        // 拡大率
        transform.localScale *= plScr.GetWeaponScaleRate(wep.areaRate, abi.atkAreaRate);

        // 弾速
        moveSpd = plScr.GetWeaponMoveSpd(initData.speed, wep.speedRate, abi.atkMoveRate);

        // 専用処理
        radian = setRad;
    }
}
