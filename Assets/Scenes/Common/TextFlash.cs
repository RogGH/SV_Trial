using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFlash : MonoBehaviour
{
    public float spd = 3;
    private Text text;
    private float time;

    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        time += spd;

        Color col = text.color;
        col.a = Mathf.Cos(time * Mathf.Deg2Rad);
        text.color = col;  
    }
}
