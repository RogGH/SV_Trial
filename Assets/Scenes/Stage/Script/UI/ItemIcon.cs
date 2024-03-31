using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour
{
    public int EquipNo;
    Player plScr;
    Image img;
    int iconNo = (int)IconNo.Invalid;

    // Start is called before the first frame update
    void Start()
    {
        plScr = StageManager.Ins.PlScr;
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (plScr.ItemTbl[EquipNo] == null)
        {
            // 非表示にする
            img.enabled = false;
        }
        else
        {
            // 各種アイコンを表示
            img.enabled = true;
            if (iconNo != plScr.ItemTbl[EquipNo].iconNo)
            {
                iconNo = plScr.ItemTbl[EquipNo].iconNo;
                img.sprite = ImageManager.Ins.GetSprite(iconNo);
            }
        }
    }
}
