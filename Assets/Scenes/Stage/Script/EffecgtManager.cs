using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectNo
{
    AtkSign,
    Num
}

public class EffecgtManager : MonoBehaviour
{
    private static EffecgtManager ins;
    public static EffecgtManager Ins
    {
        get
        {
            if (ins == null)
            {
                ins = (EffecgtManager)FindObjectOfType(typeof(EffecgtManager));
                if (ins == null) { Debug.LogError(typeof(EffecgtManager) + "is nothing"); }
            }
            return ins;
        }
    }

    public GameObject[] EffectTbl;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // エフェクト
    public GameObject BootEffect(EffectNo eNo) {
        return Instantiate(EffectTbl[(int)eNo]); 
    }
    // エフェクト
    public GameObject BootEffect(EffectNo eNo, Vector3 pos)
    {
        return Instantiate(EffectTbl[(int)eNo], pos, Quaternion.identity);
    }

}
