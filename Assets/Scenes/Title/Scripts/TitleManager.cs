using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
// �R���|�[�l���g
    public GameObject caution;
    public GameObject title;
    public GameObject text;
    public GameObject video;
    public GameObject option;
    public GameObject shop;
    public GameObject titleButGrp;
    // �V���b�v�֘A
    public GameObject shopButGrp;
    public GameObject charPanel;
    public GameObject wepPanel;
    public GameObject itemPanel;
    // �X�^�[�g�֘A
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
        // �t���X�N���[����
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

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
                // ���S���o��
                if (TitleDebugManager.Ins.LogoSkip == true) {
                    rno = 2;
                    break;
                }

                // �t�F�[�h�C��
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
                // ���S�\����
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
                // ���S�t�F�[�h�A�E�g
                {
                    Color col = cCaution.color;
                    col.a -= spd;
                    cCaution.color = col;

                    fctr0 -= Time.deltaTime;
                    if (fctr0 <= 0)
                    {
                        // �t�F�[�h�A�E�g�I��
                        BgmManager.Instance.Play("Title");
                        // �^�C�g���A�N�e�B�u��
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
                // �t�F�[�h�C��
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
                // �L�[���͑҂�
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
