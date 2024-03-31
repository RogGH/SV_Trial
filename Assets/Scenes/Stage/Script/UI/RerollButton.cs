using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollButton : MonoBehaviour
{
    public LevelUpManager LvupMng;
    Player plScr;

    void Start()
    {
        plScr = StageManager.Ins.PlScr;
    }

    void Update()
    {
    }

    // ƒŠƒ[ƒ‹
    public void PushRerollButton()
    {
        // ƒMƒ‹Šm”F
        if (plScr.Money >= ItemDefine.ReRollCost) {
            // Äİ’è
            LvupMng.SetUp();
            // ƒMƒ‹Á”ï
            plScr.Money -= ItemDefine.ReRollCost;

            SeManager.Instance.Play("Buy");
        }
        else
        {
            // ƒMƒ‹•s‘«
            SeManager.Instance.Play("Beap");
        }
    }


}
