using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMoneyText : MonoBehaviour
{
    Text moneyText;
    void Start()
    {
        moneyText = GetComponent<Text>();
        moneyText.text = SystemManager.Ins.sData.money.ToString();
    }

    void Update()
    {
        moneyText.text = SystemManager.Ins.sData.money.ToString();
    }
}
