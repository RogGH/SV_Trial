using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShellMonitorEffect : MonoBehaviour
{
    public GameObject mas;
    float time = 1.0f;

    float scaleX;
    float spd;

    void Start()
    {
        spd = transform.localScale.x / PLShellMonitorBeam.DelCount;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0) {
            Vector3 scale = transform.localScale;
            scale.x -= spd * Time.deltaTime;
            transform.localScale = scale;
            if( scale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetEffPara(float setTime, float scaleRate) {
        time = setTime;

        //
        Vector3 setScale = transform.localScale;
        setScale.x *= scaleRate;
        transform.localScale = setScale;
    }
}
