using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PLShellShot2 : MonoBehaviour
{
    public HitAtkData atkData;

    HitBase hb;
    SpriteRenderer spComp;

    Player plScr;

    float radian;
    Vector3 velocity = Vector3.zero;
    float moveSpeed;
    int hitNum = 1;

    void Start()
    {
        hb = GetComponent<HitBase>();
        spComp = GetComponent<SpriteRenderer>();

        // 速度設定
        velocity.x = moveSpeed * Mathf.Cos(radian);
        velocity.y = moveSpeed * Mathf.Sin(radian);

        float dirRadian = Mathf.Atan2(velocity.y, velocity.x);

        Vector3 localRot = transform.localEulerAngles;
        localRot.z = Mathf.Rad2Deg * dirRadian;
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

        transform.position += velocity * Time.deltaTime;
        // 現在の座標から、表示優先を決定させてみる
        spComp.sortingOrder = IObject.GetSpriteOrder(transform.position);

        hb.PostUpdate();
    }

    // パラメータ設定
    public void SetShotPara(GameObject pl, Player.Weapon wep, Player.Ability abi, float setRad)
    {
        plScr = pl.GetComponent<Player>();
        WeaponInitSOBJData initData = WeaponManager.Ins.InitTable.data[(int)WeaponInitSOBJEnum.HandGun];

        // 攻撃力
        atkData.AtkPow = plScr.GetWeaponAtkPow(initData.atk, wep.atkRate, abi.dmgRate);
        atkData.CriticalHit = plScr.GetWeaponCriHitRate(wep.criHitRate, abi.criHitRate);
        atkData.CriticalDmg = plScr.GetWeaponCriDmgRate(wep.criDmgRate, abi.criDmgRate);
        // 拡大率
        transform.localScale *= plScr.GetWeaponScaleRate(wep.areaRate, abi.atkAreaRate);

        // 弾速
        moveSpeed = plScr.GetWeaponMoveSpd(initData.speed, wep.speedRate, abi.atkMoveRate);

        // 武器別処理
        if (wep.flag1) { hitNum++; }            // フラグ１が立っていたら貫通＋１
        radian = setRad;                        // 角度
    }
}
