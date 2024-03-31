using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    public GameObject stgMng;
    StageManager stgMngScr;
    Text timeText;

    void Start()
    {
        stgMngScr = stgMng.GetComponent<StageManager>();
        timeText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float time = stgMngScr.StageTime;
        int sec, min;

        sec = (int)time % 60;
        min = (int)time / 60;

        timeText.text = min.ToString("00") + ":" + sec.ToString("00");
    }
}
