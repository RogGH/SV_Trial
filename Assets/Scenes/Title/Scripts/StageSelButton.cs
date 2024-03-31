using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelButton : MonoBehaviour
{
    string[] stageNameTbl = {
       "木人討滅戦",
       "タイタン討滅戦",
        "ガルーダ討滅戦",
        "イフリート討滅戦",
        "アルテマウェポン破壊作戦",
    };
    Text childText;

    void Start()
    {
        int butNo = transform.GetSiblingIndex();

        childText = GetComponentInChildren<Text>();
        childText.text = stageNameTbl[butNo];
    }

    void Update()
    {        
    }
}
