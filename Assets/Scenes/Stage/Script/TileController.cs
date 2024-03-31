using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    const float widthX = 3970;
    const float widthY = 2143;

    void Start()
    {
    }

    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 tilePos = transform.position;

        float distx = camPos.x - tilePos.x;
        float disty = camPos.y - tilePos.y;

        if (Mathf.Abs(distx) > widthX)
        {
            tilePos.x += widthX * 2 * (distx < 0 ? -1 : 1);
        }

        if (Mathf.Abs(disty) > widthY)
        {
            tilePos.y += widthY * 2 * (disty < 0 ? -1 : 1);
        }

        transform.position = tilePos;
    }
}
