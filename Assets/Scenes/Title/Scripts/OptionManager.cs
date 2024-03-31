using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public Slider bgmSlid;
    public Slider seSlid;

    float oldBgm;
    float oldSe;
    Lang oldLang = Lang.Japanese;

    void Start()
    {
    }

    void Update()
    {
        float newBgm = bgmSlid.value;
        if (oldBgm != newBgm) {
            if (BgmManager.Instance.CurrentAudioSource != null)
            {
                BgmManager.Instance.CurrentAudioSource.volume = newBgm;
            }
            BgmManager.Instance.TargetVolume = newBgm;
            oldBgm = newBgm;
        }


        float newSe = seSlid.value;
        if (oldSe != newSe) {
            float value = newSe;
            SeManager.Instance.Volume = newSe;
            SeManager.Instance.Play("Heal");
            oldSe = newSe;
        }
    }

    public void SetPara() {
        float bgmVol = SystemManager.Ins.sData.bgmVol;
        float seVol = SystemManager.Ins.sData.seVol;

        bgmSlid.value = bgmVol;
        seSlid.value = seVol;

        oldBgm = bgmSlid.value;
        oldSe = seSlid.value;
        oldLang = SystemManager.Ins.sData.lang;
    }

    public void ExitOptoin() {
        // ï€ë∂
        SystemManager.Ins.sData.bgmVol = oldBgm;
        SystemManager.Ins.sData.seVol = oldSe;
        SystemManager.Ins.sData.lang = oldLang;

        // Ç∆ÇËÇ†Ç¶Ç∏Ç±Ç±Ç≈ÉZÅ[Éu
        SystemManager.Ins.SaveSystemData();
    }
}
