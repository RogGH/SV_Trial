using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresureManager : MonoBehaviour
{
    public GameObject openObj;
    public GameObject getButObj;
    public LevelUpManager levelUpScr;
    public LevelUpChoiceButton butScr;
    public int ChoiceNo;
    public GameObject cancelButton;
    public GameObject TresureImage;

    Player plScr;

    void Start()
    {
        plScr = StageManager.Ins.PlScr;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetUp()
    {
        // アクティブに
        gameObject.SetActive(true);
        openObj.SetActive(true);
        getButObj.SetActive(false);
        cancelButton.SetActive(false);

        TresureImage.GetComponent<TresureImage>().TresureClose();
        SeManager.Instance.Play("TresureScreen");
    }

    public void OpenButton()
    {
        openObj.SetActive(false);
        getButObj.SetActive(true);
        cancelButton.SetActive(true);

        TresureImage.GetComponent<TresureImage>().TresureOpen();

        SeManager.Instance.StopImmediately("TresureScreen");
        SeManager.Instance.Play("TresureOpen");


        // 武器もアイテムも取れない状況
        if (plScr.CheckWeaponLevelAllMax() && plScr.CheckItemLevelAllMax())
        {
            // 金
            // ボタン設定
            ChoiceNo = (int)IconNo.Money;
            // ボタン関連のアップデート
            butScr.ImageUpdate(0);
        }
        else
        {
            // 選択可能リストを作成
            List<int> randList = levelUpScr.CreateList();

            // とりあえず１つ
            for (int choiceNo = 0; choiceNo < 1; ++choiceNo)
            {
                // 選択肢設定
                if (randList.Count > 0)
                {
                    int rand = Random.Range(0, randList.Count);
                    int iconNo = randList[rand];
                    int level = plScr.GetEquipLevel(iconNo);

                    // ボタン設定
                    ChoiceNo = iconNo;
                    // ボタン関連のアップデート
                    butScr.ImageUpdate(level);
                    // リストから削除
                    randList.Remove(iconNo);
                }
            }
        }
    }
}
