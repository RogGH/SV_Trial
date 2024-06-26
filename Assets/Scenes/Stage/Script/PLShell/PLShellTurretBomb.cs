using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShellTurretBomb : MonoBehaviour
{
    public HitAtkData atkData;
    HitBase hb;
    SpriteRenderer spComp;

    void Start()
    {
        hb = GetComponent<HitBase>();
        spComp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 現在の座標から、表示優先を決定させてみる
        spComp.sortingOrder = IObject.GetSpriteOrder(transform.position);
    }

    public void HitEnd()
    {
        Destroy(gameObject);
    }

    public void AnimEnd() {
        Destroy(gameObject);
    }

    public void SetTurretBombPara(GameObject pl, Player.Weapon wep, Player.Ability abi) {
        Player plScr = pl.GetComponent<Player>();

        WeaponInitSOBJData initData =
            WeaponManager.Ins.InitTable.data[(int)WeaponInitSOBJEnum.TurretBomb];

        // 攻撃力
        atkData.AtkPow = plScr.GetWeaponAtkPow(initData.atk, wep.atkRate, abi.dmgRate);
        atkData.CriticalHit = plScr.GetWeaponCriHitRate(wep.criHitRate, abi.criHitRate);
        atkData.CriticalDmg = plScr.GetWeaponCriDmgRate(wep.criDmgRate, abi.criDmgRate);
        // 拡大率
        transform.localScale *= plScr.GetWeaponScaleRate(wep.areaRate, abi.atkAreaRate);

    }
}
