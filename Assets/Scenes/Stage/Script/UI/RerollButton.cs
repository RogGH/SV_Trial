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

    // リロール
    public void PushRerollButton()
    {
        // ギル確認
        if (plScr.Money >= ItemDefine.ReRollCost) {
            // 再設定
            LvupMng.SetUp();
            // ギル消費
            plScr.Money -= ItemDefine.ReRollCost;

            SeManager.Instance.Play("Buy");
        }
        else
        {
            // ギル不足
            SeManager.Instance.Play("Beap");
        }
    }


}
