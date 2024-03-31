using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShellFire : MonoBehaviour
{
    public HitAtkData atkData;
    HitBase hb;
    SpriteRenderer spComp;

    float radian;
    float hitTime = 30;
    Vector3 vel = Vector3.zero;
    float moveSpd = 3.5f * 60;
    int level;

    // Start is called before the first frame update
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

        hitTime -= Time.deltaTime;
        if (hitTime <= 0) {
            hb.AtkActive = false;
        }

        transform.position += vel * Time.deltaTime;
        // 現在の座標から、表示優先を決定させてみる
        spComp.sortingOrder = IObject.GetSpriteOrder(transform.position);

        hb.PostUpdate();
    }

    public void SetFirePara(GameObject pl, Player.Weapon wep, Player.Ability abi, float setRad, int type)
    {
        Player plScr = pl.GetComponent<Player>();
        WeaponInitSOBJData initData = 
            (type == 0) ?
            WeaponManager.Ins.InitTable.data[(int)WeaponInitSOBJEnum.FrameThrower] :
            WeaponManager.Ins.InitTable.data[(int)WeaponInitSOBJEnum.BioBlast];

        // 攻撃力
        atkData.AtkPow = plScr.GetWeaponAtkPow(initData.atk, wep.atkRate, abi.dmgRate);
        atkData.CriticalHit = plScr.GetWeaponCriHitRate(wep.criHitRate, abi.criHitRate);
        atkData.CriticalDmg = plScr.GetWeaponCriDmgRate(wep.criDmgRate, abi.criDmgRate);
        // 拡大率
        transform.localScale *= plScr.GetWeaponScaleRate(wep.areaRate, abi.atkAreaRate);

        // 弾速
        moveSpd = plScr.GetWeaponMoveSpd(initData.speed, wep.speedRate, abi.atkMoveRate);

        // 専用
        radian = setRad;
        // バイオブラスト専用
        if (type == 1) {
            moveSpd *= 0.75f;
        }
    }

    public void HitEnd() {
        hb.SetAtkActive(false);
    }

    public void AnimEnd() {
        Destroy(this.gameObject);
    }
}
