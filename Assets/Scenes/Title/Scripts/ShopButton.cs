using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    TitleManager tMngScr;
    void Start()
    {
        tMngScr = GameObject.Find("Canvas").GetComponent<TitleManager>();
    }

    void onClicCmm() {
        SeManager.Instance.Play("Button1");
        tMngScr.shopButGrp.SetActive(false);
    }

    public void OnClickChar() {
        onClicCmm();
        tMngScr.charPanel.SetActive(true);
    }

    public void OnClickWep(){
        onClicCmm();
        tMngScr.wepPanel.SetActive(true);
    }

    public void OnClickItem() {
        onClicCmm();
        tMngScr.itemPanel.SetActive(true);
    }
}
