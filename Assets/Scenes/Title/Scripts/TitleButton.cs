using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{
    public TitleManager titleMngScr;
    public OptionManager optMngScr;
    public ShopManager shopMngScr;

    void Start()
    {
    }

    void Update()
    {        
    }

    // スタートボタン（キャラセレ）
    public void OnClickStart()
    {
        if (TitleDebugManager.Ins.trialVersion == true) {
            SeManager.Instance.Play("TitleDeside");
            FadeManager.Instance.LoadScene("Stage", 1.0f);
            BgmManager.Instance.Stop();

            Button btn = GetComponent<Button>();
            btn.interactable = false;

            // ステージを設定
            SystemManager.Ins.selCharNo = (CharNo)0;
            SystemManager.Ins.selStgNo = (StageNo)0;
        }
        else {
            SeManager.Instance.Play("Button1");
            titleMngScr.charSel.SetActive(true);
        }
    }
    // キャラセレ
    public void OnClickCharaSelDeside() {
        SeManager.Instance.Play("Button1");
        titleMngScr.stageSel.SetActive(true);
        titleMngScr.charSel.SetActive(false);
        // キャラクターを設定
        SystemManager.Ins.selCharNo = (CharNo)transform.GetSiblingIndex();
    }
    public void OnClickCharaSelBack()
    {
        SeManager.Instance.Play("Button1");
        titleMngScr.charSel.SetActive(false);
    }

    // ステージセレクト
    public void OnClickStageSelDeside() {
        SeManager.Instance.Play("TitleDeside");
        FadeManager.Instance.LoadScene("Stage", 1.0f);
        BgmManager.Instance.Stop();

        Button btn = GetComponent<Button>();
        btn.interactable = false;

        // ステージを設定
        if (TitleDebugManager.Ins.trialVersion == true)
        {
            SystemManager.Ins.selStgNo = StageNo.Trial;
        }
        else {
            SystemManager.Ins.selStgNo = (StageNo)transform.GetSiblingIndex();
        }
    }
    public void OnClickStageSelBack() {
        SeManager.Instance.Play("Button1");
        titleMngScr.stageSel.SetActive(false);
        titleMngScr.charSel.SetActive(true);
    }


    // オプションに入る
    public void OnClickOption()
    {
        SeManager.Instance.Play("Button1");
        titleMngScr.option.SetActive(true);
        optMngScr.SetPara();
        titleMngScr.titleButGrp.SetActive(false);
    }
    // オプションから戻る
    public void OnClickOptionBack()
    {
        SeManager.Instance.Play("Button1");
        optMngScr.ExitOptoin();                 // オプション消す
        titleMngScr.option.SetActive(false);    // 画面消す
        titleMngScr.titleButGrp.SetActive(true);
    }

    // ショップへ
    public void OnClickShop()
    {
        SeManager.Instance.Play("Button1");
        titleMngScr.shop.SetActive(true);
        shopMngScr.SetPara();
        titleMngScr.titleButGrp.SetActive(false);
    }
    // ショップから戻る
    public void OnClickShopBack()
    {
        // ショップ画面から戻る
        SeManager.Instance.Play("Button1");

        // ショップ選択中かチェック
        if ( titleMngScr.shopButGrp.activeInHierarchy )
        {
            // ショップから元の画面に
            shopMngScr.ExitShop();                 // ショップ消す
            titleMngScr.shop.SetActive(false);      // 画面消す
            titleMngScr.titleButGrp.SetActive(true);
        }
        else {
            // ショップのさらに奥なので、ショップ選択に戻る
            titleMngScr.shopButGrp.SetActive(true);
            titleMngScr.charPanel.SetActive(false);
            titleMngScr.wepPanel.SetActive(false);
            titleMngScr.itemPanel.SetActive(false);

            // とりあえずここでセーブ
            SystemManager.Ins.SaveSystemData();
        }
    }

    public void OnClickExit()
    {
        SeManager.Instance.Play("Button1");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
    }


}
