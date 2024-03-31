using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TresureImage : MonoBehaviour
{
    public Sprite closeSprite;
    public Sprite openSprite;
    Image img;

    void Start()
    {
    }

    public void TresureClose()
    {
        img = GetComponent<Image>();
        img.sprite = closeSprite;
    }

    public void TresureOpen() {
        img = GetComponent<Image>();
        img.sprite = openSprite;
    }
}
