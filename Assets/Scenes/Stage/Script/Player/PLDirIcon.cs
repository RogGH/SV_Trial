using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLDirIcon : MonoBehaviour
{
    GameObject plObj;
    Player plScr;
    float baseRotZ = 90.0f;
    float ofs = 40.0f;

    void Start()
    {
        plObj = StageManager.Ins.PlObj;
        plScr = StageManager.Ins.PlScr;
    }

    void Update()
    {
        if (StageManager.Ins.CheckStop()) { return; }
        if (plScr.DieFlag == true) { Destroy(gameObject); }

        Vector3 basePos = plScr.GetCenterPos();
        float radian = plScr.GetAngleToPLMouse();

        // ç¿ïWÇï‚ê≥
        Vector3 setPos = basePos;
        setPos.x += Mathf.Cos(radian) * ofs;
        setPos.y += Mathf.Sin(radian) * ofs;
        transform.position = setPos;

        // äpìxÇéwíË
        transform.localEulerAngles = new Vector3(0, 0, baseRotZ + Mathf.Rad2Deg * radian);
    }
}
