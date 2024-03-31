using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLvelText : MonoBehaviour
{
    int equipNo;
    Player plScr;
    Text wLevelText;

    void Start()
    {
        plScr = StageManager.Ins.PlScr;
        wLevelText = GetComponent<Text>();
        equipNo = transform.parent.GetComponent<ItemIcon>().EquipNo;
    }

    // Update is called once per frame
    void Update()
    {
        if (plScr.ItemTbl[equipNo] == null)
        {
            wLevelText.text = "";
        }
        else
        {
            wLevelText.text = "LV" + plScr.ItemTbl[equipNo].level.ToString();
        }
    }
}
