using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceIcon : MonoBehaviour
{
    public Sprite[] spIconTbl;

    void Awake()
    {
        // スプライト変更
        int selCharNo = (int)SystemManager.Ins.selCharNo;

        // 警告
        if (spIconTbl.Length != (int)CharNo.Num)
        {
            Debug.LogError("palyer animator tbl num is different" + "tbl length = "
                + spIconTbl.Length + "charNum = " + CharNo.Num);
        }

        Image imgCmp = GetComponent<Image>();
        imgCmp.sprite = spIconTbl[(int)selCharNo];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
