using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponIcon : MonoBehaviour
{
    public int EquipNo;
    Player plScr;
    Image img;
    int iconNo = (int)IconNo.Invalid;

    void Start()
    {
        plScr = StageManager.Ins.PlScr;
        img = GetComponent<Image>();
    }

    void Update()
    {
        if (plScr.WeaponTbl[EquipNo].initFlag == false) 
        {
            // 非表示にする
            img.enabled = false;
        }
        else
        {
            // 各種アイコンを表示
            img.enabled = true;
            if ( iconNo != plScr.WeaponTbl[EquipNo].iconNo ) {
                iconNo = plScr.WeaponTbl[EquipNo].iconNo;
                img.sprite = ImageManager.Ins.GetSprite(iconNo);
            }
        }
    }
}
