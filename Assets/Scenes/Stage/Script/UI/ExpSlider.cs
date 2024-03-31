using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpSlider : MonoBehaviour
{
    Player plScr;
    Slider expSlider;
    // Start is called before the first frame update
    void Start()
    {
        plScr = StageManager.Ins.PlScr;
        expSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if( plScr == null) { return; }

        float rate = (float)plScr.Exp / plScr.NextExp;
        expSlider.value = rate;
    }
}
