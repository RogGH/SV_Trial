using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitDispText : MonoBehaviour
{
    [SerializeField]float liveCnt = 1.0f;
    const float alphaCnt = 0.5f;
    Vector3 spd = new Vector3(0, 2.0f * 60, 0);
    float alphaSpd = 1 / 15.0f * 60.0f;

    TextMesh textCmp;
    float time;

    void Start()
    {
        textCmp = GetComponent<TextMesh>();
        time = liveCnt;
    }

    void Update()
    {
        transform.position += spd * Time.deltaTime;

        time -= Time.deltaTime;
        if (time <= alphaCnt) {
            Color setCol = textCmp.color;
            setCol.a -= alphaSpd * Time.deltaTime;
            textCmp.color = setCol;
        }

        if( time <= 0) {
            Destroy(this.gameObject);
        }
    }

    public void SetString(string setText, Color setCol)
    {
        textCmp = GetComponent<TextMesh>();
        textCmp.text = setText;
        textCmp.color = setCol;
    }
}
