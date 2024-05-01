using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFlag : MonoBehaviour
{
    private static WeaponFlag ins;
    public static WeaponFlag Ins
    {
        get
        {
            if (ins == null)
            {
                ins = (WeaponFlag)FindObjectOfType(typeof(WeaponFlag));
                if (ins == null) { Debug.LogError(typeof(WeaponFlag) + "is nothing"); }
            }
            return ins;
        }
    }

    [Header("ƒnƒ“ƒhƒKƒ“")] [SerializeField] bool Shot = false;
    [Header("ƒhƒŠƒ‹")] [SerializeField] bool Drill = false;
    [Header("‰ñ“]‚Ì‚±‚¬‚è")] [SerializeField] bool Saw = false;
    [Header("‰Î‰Š•úŽË")] [SerializeField] bool Frame = false;
    [Header("ƒ^ƒŒƒbƒg")] [SerializeField] bool Turret = false;
    [Header("”g“®–C")] [SerializeField] bool WaveCannon = false;

    [HideInInspector]
    public List<bool> FlagList;

    void Start()
    {
        FlagList.Add(Shot);
        FlagList.Add(Drill);
        FlagList.Add(Saw);
        FlagList.Add(Frame);
        FlagList.Add(Turret);
        FlagList.Add(WaveCannon);
    }
}
