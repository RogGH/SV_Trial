using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpText : MonoBehaviour
{
    public float colorChangeInterval = 1f; // 色が変わる間隔（秒）
    public Color[] colors; // アニメーションで使用する色の配列

    private Text textComponent;
    private int currentIndex = 0;

    float timer = 0;


    void Start()
    {
        textComponent = GetComponent<Text>();

        // 初期の色を設定
        textComponent.color = colors[currentIndex];
    }

    void Update()
    {
        timer += (1.0f / 60.0f );
        if (timer >colorChangeInterval ) {
            timer = 0;
            ChangeColor();
        }
    }

    void ChangeColor()
    {
        // 次の色へのインデックスを計算し、ループさせる
        currentIndex = (currentIndex + 1) % colors.Length;

        // 色を変更
        textComponent.color = colors[currentIndex];
    }
}
