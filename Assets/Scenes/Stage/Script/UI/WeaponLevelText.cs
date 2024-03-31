using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponLevelText : MonoBehaviour
{
    int equipNo;
    Player plScr;
    Text wLevelText;

    void Start()
    {
        plScr = StageManager.Ins.PlScr;
        wLevelText = GetComponent<Text>();
        equipNo = transform.parent.GetComponent<WeaponIcon>().EquipNo;
    }

    // Update is called once per frame
    void Update()
    {
        if (plScr.WeaponTbl[equipNo].initFlag == false)
        { 
            wLevelText.text = "";
        }
        else {
            wLevelText.text = "LV" + plScr.WeaponTbl[equipNo].level.ToString();
        }
    }
}
