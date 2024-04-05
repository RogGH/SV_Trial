using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PLShellShot
{
    // 速度設定
    public void SetSpeed()
    {
        // ここにｘ速度を設定する
        velocity.x = moveSpeed * Mathf.Cos(radian);
        // ここにｙ速度を設定する
        velocity.y = moveSpeed * Mathf.Sin(radian);
    }
}
