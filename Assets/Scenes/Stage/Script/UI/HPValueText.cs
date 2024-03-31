using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPValueText : MonoBehaviour
{
    HitBase plHB;
    Text hpText;

    void Start()
    {
        plHB = StageManager.Ins.PlHB;
        hpText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = plHB.HP.ToString() + "/" + plHB.MaxHP.ToString();
    }
}
