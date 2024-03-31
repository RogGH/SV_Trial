using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PLSlider : MonoBehaviour
{
    HitBase plHB;
    Slider plSlider;

    // Start is called before the first frame update
    void Start()
    {
        plHB = StageManager.Ins.PlHB;
        plSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (plHB == null) { return; }

        float rate = (float)(plHB.HP) / plHB.MaxHP;
        plSlider.value = rate;
    }
}
