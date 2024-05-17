using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    public string moveSceneName = "Stage";
    int rno = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rno == 0)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StageManager.Ins.Save();
                SeManager.Instance.Play("TitleDeside");
                FadeManager.Instance.LoadScene(moveSceneName, 1.0f);
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
