using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// インターフェース的なクラス
public class IObject
{
    // スプライトの優先関連
    // 手前に出る系のエフェクト　1000
    // 配置物 720
    //  PL　0
    // 配置物 -720
    // 予兆 -1000
    // 背景 -2000
    // 
    // 配置物はシェルやエフェクトが手前に来るように値を＋してある
    // 

    public static int GetSpriteOrder(Vector3 pos)
    {
        // 現在の座標から、表示優先を決定させてみる
        Vector3 camPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
        float distY = camPos.y - pos.y;
        return (int)(Mathf.Clamp(distY, -720, 720));
    }
}
