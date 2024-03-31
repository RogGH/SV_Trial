using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
// コンポーネント
    public GameObject caution;
    public GameObject title;
    public GameObject text;
    public GameObject video;
    public GameObject option;
    public GameObject shop;
    public GameObject titleButGrp;
    // ショップ関連
    public GameObject shopButGrp;
    public GameObject charPanel;
    public GameObject wepPanel;
    public GameObject itemPanel;
    // スタート関連
    public GameObject charSel;
    public GameObject stageSel;

    // 
    Image cCaution;
    Image cTitle;

    const float FadeCount = 1.0f;
    const float CautionCount = 2.5f;

    int rno = 0;
    float fctr0 = FadeCount;
    float spd = FadeCount / 60f;

    void Start()
    {
        Screen.SetResolution(1280, 720, false);

        cCaution = caution.GetComponent<Image>();
        cTitle = title.GetComponent<Image>();

        Color col = Color.white;
        col.a = 0;
        cCaution.color = col;
        cTitle.color = col;

        caution.SetActive(true);
        title.SetActive(false);
        option.SetActive(false);
 
        shop.SetActive(false);
        charPanel.SetActive(false);
        wepPanel.SetActive(false);
        itemPanel.SetActive(false);

        charSel.SetActive(false);
        stageSel.SetActive(false);
    }

    void Update()
    {
        switch (rno) {
            case 0:
                // ロゴを出す
                if (TitleDebugManager.Ins.LogoSkip == true) {
                    rno = 2;
                    break;
                }

                // フェードイン
                {
                    Color col = cCaution.color;
                    col.a += spd;
                    cCaution.color = col;
                }

                fctr0 -= Time.deltaTime;
                if (fctr0 <= 0) {
                    fctr0 = CautionCount;
                    cCaution.color = Color.white;
                    rno++;
                }
                break;

            case 1:
                // ロゴ表示中
                fctr0 -= Time.deltaTime;

                if (Input.anyKey)
                {
                    fctr0 = 0;
                }

                if (fctr0 <= 0)
                {
                    fctr0 = FadeCount;
                    rno++;
                }
                break;

            case 2:
                // ロゴフェードアウト
                {
                    Color col = cCaution.color;
                    col.a -= spd;
                    cCaution.color = col;

                    fctr0 -= Time.deltaTime;
                    if (fctr0 <= 0)
                    {
                        // フェードアウト終了
                        BgmManager.Instance.Play("Title");
                        // タイトルアクティブに
                        title.SetActive(true);
                        video.SetActive(true);
                        fctr0 = FadeCount;

                        col.a = 0;
                        cCaution.color = col;
                        rno++;
                    }
                }
                break;
            case 3:
                // フェードイン
                {
                    Color col = cTitle.color;
                    col.a += spd;
                    cTitle.color = col;
                }

                fctr0 -= Time.deltaTime;
                if (fctr0 <= 0)
                {
                    //text.SetActive(true);
                    rno++;
                }
                break;

            case 4:
                // キー入力待ち
                //if ( Input.GetKeyDown(KeyCode.Return) ) {
                //    SeManager.Instance.Play("TitleDeside");
                //    FadeManager.Instance.LoadScene("Stage", 1.0f);
                //    BgmManager.Instance.Stop();
                //    rno++;
                //}
                break;
        }
    }
}
