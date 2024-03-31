using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    int rno = 0;

    void Start()
    {       
    }

    void Update()
    {
        if (rno == 0)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StageManager.Ins.Save();
                SeManager.Instance.Play("TitleDeside");
                FadeManager.Instance.LoadScene("Stage", 1.0f);
                rno++;
            }
        }
    }

    // 
    public void SetUp()
    {
        // アクティブに
        gameObject.SetActive(true);
    }
}
