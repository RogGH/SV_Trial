using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitTestPLInfo : MonoBehaviour
{
    public GameObject pl;
    PLHitTest plScr;
    HitBase plHB;
    Text texComp;

    void Start()
    {
        texComp = GetComponent<Text>();
        plScr = pl.GetComponent<PLHitTest>();
        plHB = pl.GetComponent<HitBase>();
    }

    void Update()
    {
        if (pl == null) {
            texComp.text = "PL死亡";
            return;
        }

        texComp.text = "PLHP:" + plScr.getHitBase().HP;

        // チェック用判定に受けた
        if (plHB.CheckDefCheckOnly() ) {
            texComp.text += "\nPLチェック用判定に攻撃を受けた";
        }
        // 接触はしている
        if (plHB.CheckDefHit() )
        {
            texComp.text += "\n防御で攻撃と接触した";
        }
        // ダメージを受けた
        if (plHB.CheckDamage())
        {
            texComp.text += "\nダメージを受けた";
        }
    }
}
