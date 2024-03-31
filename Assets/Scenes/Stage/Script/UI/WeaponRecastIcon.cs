using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponRecastIcon : MonoBehaviour
{
    Player plScr;
    Image img;
    int equipNo;

    void Start()
    {
        plScr = StageManager.Ins.PlScr;
        img = GetComponent<Image>();
        equipNo = transform.parent.GetComponent<WeaponIcon>().EquipNo;
    }

    void Update()
    {
        if (plScr.WeaponTbl[equipNo].initFlag == false) {
            img.enabled = false;
        }
        else {
            img.enabled = true;
            float rate = plScr.WeaponTbl[equipNo].recastCounter / plScr.WeaponTbl[equipNo].recastTime;
            img.fillAmount = rate;
        }
    }
}
