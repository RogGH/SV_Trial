using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShellDrill : MonoBehaviour
{
    public HitAtkData atkData;

    HitBase hb;
    SpriteRenderer spComp;

    float radian;
    Vector3 vel = Vector3.zero;
    float moveSpd;

    void Start()
    {
        hb = GetComponent<HitBase>();
        spComp = GetComponent<SpriteRenderer>();

        // オブジェクト検索
        GameObject tempObj = GameObject.FindGameObjectWithTag("Enemy");
        if (tempObj != null)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                // シーン上に存在するオブジェクトならば処理.
                if (obj.activeInHierarchy)
                {
                    // 長さを比較
                    Vector3 vec1 = tempObj.transform.position - transform.position;
                    Vector3 vec2 = obj.transform.position - transform.position;

                    if (vec1.sqrMagnitude > vec2.sqrMagnitude)
                    {
                        tempObj = obj;
                    }
                }
            }

            // 角度を設定
            Vector3 tagPos = tempObj.transform.position;
            tagPos.y += 20.0f;
            radian = Mathf.Atan2(tagPos.y - transform.position.y,
                tempObj.transform.position.x - transform.position.x);
        }

        vel.x = moveSpd * Mathf.Cos(radian);
        vel.y = moveSpd * Mathf.Sin(radian);

        Vector3 localRot = transform.localEulerAngles;
        localRot.z = Mathf.Rad2Deg * radian - 135;
        transform.localEulerAngles = localRot;

        Destroy(gameObject, WeaponDefine.LiveCountDef);
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManager.Ins.CheckStop()) { return; }
        if (WeaponDefine.CameraOut(gameObject)) { Destroy(gameObject); return; }

        hb.PreUpdate();

        transform.position += vel * Time.deltaTime;
        // 現在の座標から、表示優先を決定させてみる
        spComp.sortingOrder = IObject.GetSpriteOrder(transform.position);

        hb.PostUpdate();
    }

    public void SetDrillPara(GameObject pl, Player.Weapon wep, Player.Ability abi)
    {
        // 共通
        Player plScr = pl.GetComponent<Player>();
        WeaponInitSOBJData initData = WeaponManager.Ins.InitTable.data[(int)WeaponInitSOBJEnum.Drill];

        // 攻撃力関連
        atkData.AtkPow = plScr.GetWeaponAtkPow(initData.atk, wep.atkRate, abi.dmgRate);
        atkData.CriticalHit = plScr.GetWeaponCriHitRate(wep.criHitRate, abi.criHitRate);
        atkData.CriticalDmg = plScr.GetWeaponCriDmgRate(wep.criDmgRate, abi.criDmgRate);
        // 拡大率
        transform.localScale *= plScr.GetWeaponScaleRate(wep.areaRate, abi.atkAreaRate);

        // 弾速
        moveSpd = plScr.GetWeaponMoveSpd(initData.speed, wep.speedRate, abi.atkMoveRate);
    }
}
