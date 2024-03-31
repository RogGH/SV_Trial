using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShellMonitorBeam : MonoBehaviour
{
    public HitAtkData atkData;

    public GameObject effObj;
    public GameObject atkObj;

    public static float DelCount = 0.25f;        // è¡Ç¶ÇÈéûä‘

    HitBase hb;
    ParticleSystem pSys;
    Material pMat;

    float time;
    float liveCnt;
    float scaleRate;

    void Start()
    {
        hb = GetComponent<HitBase>();

        pSys = effObj.GetComponent<ParticleSystem>();
        pMat = pSys.GetComponent<Renderer>().material;

        effObj.GetComponent<PLShellMonitorEffect>().SetEffPara(time, scaleRate);

        Vector3 temp = atkObj.transform.localScale;
        temp.x *= scaleRate;
        atkObj.transform.localScale = temp;

        Color col = pMat.color;
        col.a = 0.5f;
        pMat.color = col;

        liveCnt = time + DelCount;

        SeManager.Instance.Play("Beam", 0);
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0) {
            hb.SetAtkActive(false);
        }

        liveCnt -= Time.deltaTime;
        if (liveCnt <= 0) {
            Destroy(gameObject);
        }
    }

    public void SetWaveBeamPara(GameObject pl, Player.Weapon wep, Player.Ability abi) {

        Player plScr = pl.GetComponent<Player>();
        WeaponInitSOBJData initData =
            WeaponManager.Ins.InitTable.data[(int)WeaponInitSOBJEnum.WaveCannon];

        // çUåÇóÕ
        atkData.AtkPow = plScr.GetWeaponAtkPow(initData.atk, wep.atkRate, abi.dmgRate);
        atkData.CriticalHit = plScr.GetWeaponCriHitRate(wep.criHitRate, abi.criHitRate);
        atkData.CriticalDmg = plScr.GetWeaponCriDmgRate(wep.criDmgRate, abi.criDmgRate);
        // ägëÂó¶
        scaleRate = plScr.GetWeaponScaleRate(wep.areaRate, abi.atkAreaRate);
        transform.localScale *= scaleRate;
        // éûä‘ê›íË
        time = plScr.GetWeaponAtkTime(initData.time, wep.timeRate, abi.atkTimeRate);
    }
}
