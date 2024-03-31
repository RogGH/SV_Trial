using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharShopButton : MonoBehaviour
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
    const int CharPrice = 1000;

    void Start()
    {
        int butNo = transform.GetSiblingIndex();

        childText = GetComponentInChildren<Text>();
        childText.text = jobNameTbl[butNo];
    }

    void Update()
    {        
    }

    public void OnClickCharBuy() {
        // ƒMƒ‹Šm”F
        if (SystemManager.Ins.sData.money >= CharPrice )
        {
            SystemManager.Ins.sData.money -= 1000;
            SeManager.Instance.Play("Buy");
        }
        else
        {
            // ƒMƒ‹•s‘«
            SeManager.Instance.Play("Beap");
        }
    }
}
