using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleDebugManager : MonoBehaviour
{
    public bool LogoSkip = false;
    [HideInInspector] public bool trialVersion = true;

    private static TitleDebugManager ins;
    public static TitleDebugManager Ins
    {
        get
        {
            if (ins == null)
            {
                ins = (TitleDebugManager)FindObjectOfType(typeof(TitleDebugManager));
                if (ins == null) { Debug.LogError(typeof(TitleDebugManager) + "is nothing"); }
            }
            return ins;
        }
    }

    void Awake()
    {
        //シングルトンのためのコード
        if (this != Ins) {
            Destroy(this.gameObject); return;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
