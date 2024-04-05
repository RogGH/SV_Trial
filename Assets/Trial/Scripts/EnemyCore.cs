using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyCmm
{
    // 敵の動作を設定する
    public void EnemyMoveControl()
    {
        // PLと敵との距離を計算
        float distX = plObj.transform.position.x - transform.position.x;
        float distY = plObj.transform.position.y - transform.position.y;

        float radian = 0;
        // ここにプレイヤーへの角度を計算する処理を記入
        radian = Mathf.Atan2(distY, distX);

        // 移動速度を設定
        if (radian != 0) 
        {
            moveVec.x = moveSpd * Mathf.Cos(radian);
            moveVec.y = moveSpd * Mathf.Sin(radian);
        }

        // 移動処理
        transform.position += moveVec * Time.deltaTime;
    }
}
