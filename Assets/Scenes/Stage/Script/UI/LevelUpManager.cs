using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    const int ChoiceNum = 4;
    public int[] choiceNoTbl = new int[ChoiceNum];
    public GameObject[] choiceButtonObj;
    public LevelUpChoiceButton[] choiceButtonScr;


    ImageManager img;
    Player plScr;

    bool initFlag = false;

    void Start()
    {
        img = ImageManager.Ins;
        plScr = StageManager.Ins.PlScr;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetUp()
    {
        if (initFlag == false)
        {
            initFlag = true;
            Start();
        }

        // アクティブに
        gameObject.SetActive(true);

        // ボタン関連設定
        ButtonSetUp();
    }


    public void ButtonSetUp()
    {
        // 一旦全部非表示に
        for (int i = 0; i < ChoiceNum; ++i){
            choiceButtonObj[i].SetActive(false);
        }

        // 選択可能リストを作成
        List<int> randList = CreateList();
        // ループ回数を決定
        int loopNum = ChoiceNum;
        // リストの数が少ない場合はリストの数に合わせる
        if (randList.Count < ChoiceNum) { loopNum = randList.Count; }

        // 選択肢設定
        for (int choiceNo = 0; choiceNo < loopNum; ++choiceNo)
        {
            if (randList.Count > 0)
            {
                int rand = Random.Range(0, randList.Count);
                int iconNo = randList[rand];
                int level = plScr.GetEquipLevel(iconNo);

                // ボタン設定
                choiceNoTbl[choiceNo] = iconNo;
                choiceButtonScr[choiceNo].ImageUpdate(level);
                choiceButtonObj[choiceNo].SetActive(true);

                // リストから削除
                randList.Remove(iconNo);
            }
        }

        // リストが０の場合は、アイテムを出す
        int activeNum = 0;
        for (int i = 0; i < ChoiceNum; ++i)
        {
            if (choiceButtonObj[i].activeInHierarchy)
            {
                activeNum++;
            }
        }
        if (activeNum == 0)
        {
            choiceButtonObj[0].SetActive(true);
            choiceNoTbl[0] = (int)IconNo.Money;
            choiceButtonScr[0].ImageUpdate(0);
            // 回復アイテム
            choiceButtonObj[1].SetActive(true);
            choiceNoTbl[1] = (int)IconNo.Potion;
            choiceButtonScr[1].ImageUpdate(0);
            return;
        }
    }

    public List<int> CreateList()
    {
        plScr = StageManager.Ins.PlScr;

        bool wEquipMaxFlag = plScr.WeaponEquipNum >= WeaponDefine.EquipMax;
        bool iEquipMaxFlag = plScr.ItemEquipNum >= ItemDefine.EquipMax;

        // 選択可能リストを作成
        List<int> randList = new List<int>();
        int chkNo = 0;
        int chkNum = (int)IconNo.LevelUpItemNum;
        // アイテムを全部装備してるかチェック
        if (plScr.CheckItemLevelAllMax()) { chkNo += (int)IconNo.ItemNum; }
        // 武器を全部装備しているかチェック
        if (plScr.CheckWeaponLevelAllMax()) { chkNum -= (int)IconNo.WeaponNum; }
        // 
        for (; chkNo < chkNum; ++chkNo)
        {
            int eNo = plScr.CheckEquip(chkNo);
            if (eNo != -1)
            {
                // 装備している場合、レベルが最大かチェック
                if (plScr.CheckEquipLevelMax(eNo, chkNo))
                {
                    continue;
                }
            }
            else
            {
                // 装備されていない場合、アイテムまたは武器の空き枠があるかチェック
                if (ImageManager.Ins.CheckIconNoIsWeapon(chkNo))
                {
                    // 武器
                    if (wEquipMaxFlag) { continue; }

                    // とりあえず武器は出なくする
                    int wNo = ImageManager.Ins.ConvIconNoToWeaponSerialNo(chkNo);
                    if (WeaponFlag.Ins.FlagList[wNo] == false) { continue; }
                }
                else
                {
                    // アイテムが全て装備されている場合（結構無駄がある処理だが、今のところ放置）
                    if (iEquipMaxFlag) { continue; }

                    // フラグチェック
                    if (ItemFlag.Ins.FlagList[chkNo] == false) { continue; }
                }
            }
            randList.Add(chkNo);        // リストに追加
        }
        return randList;
    }

}
