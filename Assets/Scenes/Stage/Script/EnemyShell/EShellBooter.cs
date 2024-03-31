using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EShellBooter : MonoBehaviour
{
    public GameObject bootShl;

    Vector3 basePos;

    float bootDeg = 0;
    float bootlength = 100;
    float bootInterval = 0.03f;
    float bootNum = 12;
    float bootNo = 0;
    float bootCounter = 0;


    void Start()
    {
        basePos = transform.position;
        SeManager.Instance.Play("LandSlide");
    }

    void Update()
    {
        bootCounter -= Time.deltaTime;
        if (bootCounter <= 0) {
            // ‹N“®
            Vector3 bootPos = basePos;
            float radian = bootDeg * Mathf.Deg2Rad;
            float ofs = bootlength * bootNo;
            bootPos.x += ofs * Mathf.Cos(radian);
            bootPos.y += ofs * Mathf.Sin(radian);
            Instantiate(bootShl, bootPos, Quaternion.identity);

            bootNo++;
            if (bootNo >= bootNum) {
                Destroy(gameObject);
                return;
            }

            bootCounter = bootInterval;
        }
    }

    public void SetUp(float deg) {
        bootDeg = deg;
    }
}
