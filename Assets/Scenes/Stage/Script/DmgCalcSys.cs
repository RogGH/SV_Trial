using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgCalcSys : MonoBehaviour
{
    // ƒVƒ“ƒOƒ‹ƒgƒ“ŽÀ‘•
    private static DmgCalcSys ins;
    public static DmgCalcSys Ins
    {
        get
        {
            if (ins == null)
            {
                ins = (DmgCalcSys)FindObjectOfType(typeof(DmgCalcSys));
                if (ins == null)
                {
                    Debug.LogError(typeof(DmgCalcSys) + "is nothing");
                }
            }
            return ins;
        }
    }

    public float time = 0;
    public int totalDmg = 0;

    public float[] checkTimeTbl = { 1, 4, 10 };
    float[] timeTbl = { 0, 0, 0};
    public float[] dmgTbl = { 0, 0, 0 };
    float[] prevDmgTbl = { 0, 0, 0 };

    private void Awake()
    {
        if (this != Ins)
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!StageManager.Ins.CheckStop()) {
            time += Time.deltaTime;

            int tblNum = checkTimeTbl.Length;
            for (int no = 0; no < tblNum; ++no) {
                timeTbl[no] += Time.deltaTime;
                if (timeTbl[no] >= checkTimeTbl[no]) {
                    timeTbl[no] = 0;
                    dmgTbl[no] = totalDmg - prevDmgTbl[no];
                    prevDmgTbl[no] = totalDmg;
                }
            }
        }        
    }


    public void AddTotalDmg(int val) {
        totalDmg += val;
    }
}
