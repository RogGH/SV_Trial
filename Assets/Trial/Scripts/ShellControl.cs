using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PLShellShot
{
    // ‘¬“xİ’è
    public void SetSpeed()
    {
        // ‚±‚±‚É‚˜‘¬“x‚ğİ’è‚·‚é
        velocity.x = moveSpeed * Mathf.Cos(radian);
        // ‚±‚±‚É‚™‘¬“x‚ğİ’è‚·‚é
        velocity.y = moveSpeed * Mathf.Sin(radian);
    }
}
