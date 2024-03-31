using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    // ‚±‚ÌƒŒƒxƒ‹‚©‚ç‘S‚ÄŒˆ‚Ü‚é
    public int lv = 0;

    [SerializeField] Sprite[] spTbl;
    public Sprite GetSpriteNo() { return spTbl[lv]; }

    [SerializeField] int[] hpTbl;
    public int GetHP() { return hpTbl[lv]; }

    [SerializeField] float[] moveSpdTbl;
    public float GetMoveSpd() { return moveSpdTbl[lv]; }

    [SerializeField] int[] atkPowTbl;
    public int GetAtkPow() { return atkPowTbl[lv]; }

    [SerializeField] int[] defPowTbl;
    public int GetDefPow() { return defPowTbl[lv]; }

    [SerializeField] DropType[] dropTypeTbl;
    public DropType GetDropType() { return dropTypeTbl[lv]; }
}
