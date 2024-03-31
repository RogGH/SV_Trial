using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelButton : MonoBehaviour
{
    string[] jobNameTbl = {
        "˜",
        "‹@Hm",
        "—x‚èq",
        "¢Š«m",
        "•–‚“±m",
        "”’–‚“±m",
    };

    Text childText;

    void Start()
    {
        int butNo = transform.GetSiblingIndex();

        childText = GetComponentInChildren<Text>();
        childText.text = jobNameTbl[butNo];
    }

    void Update()
    {        
    }
}
