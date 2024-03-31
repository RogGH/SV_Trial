using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DmgCalcText : MonoBehaviour
{
    Text dmgText;

    void Start()
    {
        dmgText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float time = DmgCalcSys.Ins.time;
        int sec, min;
        sec = (int)time % 60;
        min = (int)time / 60;
        dmgText.text = "計測時間:" + min.ToString("00") + ":" + sec.ToString("00") + "\n";

        dmgText.text += "総ダメージ量:" + DmgCalcSys.Ins.totalDmg.ToString() + "\n";

        for (int no = 0; no < 3; ++no) {
            float chkTime = DmgCalcSys.Ins.checkTimeTbl[no];
            float dmg = DmgCalcSys.Ins.dmgTbl[no];
            dmgText.text += chkTime.ToString() + "sec/1sec:" + dmg + "/";
            dmgText.text += dmg / chkTime + "\n";
        }
    }
}
