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

        // �A�N�e�B�u��
        gameObject.SetActive(true);

        // �{�^���֘A�ݒ�
        ButtonSetUp();
    }


    public void ButtonSetUp()
    {
        // ��U�S����\����
        for (int i = 0; i < ChoiceNum; ++i){
            choiceButtonObj[i].SetActive(false);
        }

        // ���A�C�e��
        // �{�^���ݒ�
        choiceButtonObj[0].SetActive(true);
        choiceNoTbl[0] = (int)IconNo.Money;
        // �{�^���֘A�̃A�b�v�f�[�g
        choiceButtonScr[0].ImageUpdate(0);

        // �񕜃A�C�e��
        // �{�^���ݒ�
        choiceButtonObj[1].SetActive(true);
        choiceNoTbl[1] = (int)IconNo.Potion;
        // �{�^���֘A�̃A�b�v�f�[�g
        choiceButtonScr[1].ImageUpdate(0);

        // ������A�C�e�������Ȃ���
        if (plScr.CheckWeaponLevelAllMax() && plScr.CheckItemLevelAllMax())
        {
        }
        else
        {
            // �I���\���X�g���쐬
            List<int> randList = CreateList();

            // �I�������e����
            int[] choiceTbl = new int[ChoiceNum] { -1, -1, -1, -1 };
            int loopNum = ChoiceNum;
            if (randList.Count < ChoiceNum) { loopNum = randList.Count; }

            // �I�����ݒ�
            for (int choiceNo = 0; choiceNo < loopNum; ++choiceNo)
            {
                if (randList.Count > 0)
                {
                    int rand = Random.Range(0, randList.Count);
                    int iconNo = randList[rand];
                    int level = plScr.GetEquipLevel(iconNo);

                    // �{�^���ݒ�
                    choiceNoTbl[choiceNo] = iconNo;
                    // �{�^���֘A�̃A�b�v�f�[�g
                    choiceButtonScr[choiceNo].ImageUpdate(level);
                    // �ԍ��ۑ�
                    choiceTbl[choiceNo] = iconNo;
                    choiceButtonObj[choiceNo].SetActive(true);

                    // ���X�g����폜
                    randList.Remove(iconNo);
                }
            }
        }
    }

    public List<int> CreateList()
    {
        plScr = StageManager.Ins.PlScr;

        bool wEquipMaxFlag = plScr.WeaponEquipNum >= WeaponDefine.EquipMax;
        bool iEquipMaxFlag = plScr.ItemEquipNum >= ItemDefine.EquipMax;

        // �I���\���X�g���쐬
        List<int> randList = new List<int>();
        int chkNo = 0;
        int chkNum = (int)IconNo.LevelUpItemNum;
        // �A�C�e����S���������Ă邩�`�F�b�N
        if (plScr.CheckItemLevelAllMax()) { chkNo += (int)IconNo.ItemNum; }
        // �����S���������Ă��邩�`�F�b�N
        if (plScr.CheckWeaponLevelAllMax()) { chkNum -= (int)IconNo.WeaponNum; }
        // 
        for (; chkNo < chkNum; ++chkNo)
        {
            int eNo = plScr.CheckEquip(chkNo);
            if (eNo != -1)
            {
                // �������Ă���ꍇ�A���x�����ő傩�`�F�b�N
                if (plScr.CheckEquipLevelMax(eNo, chkNo))
                {
                    continue;
                }
            }
            else
            {
                // ��������Ă��Ȃ��ꍇ�A�A�C�e���܂��͕���̋󂫘g�����邩�`�F�b�N
                if (ImageManager.Ins.CheckIconNoIsWeapon(chkNo))
                {
                    // ����
                    if (wEquipMaxFlag) { continue; }

                    // �Ƃ肠��������͏o�Ȃ�����
                    int wNo = ImageManager.Ins.ConvIconNoToWeaponSerialNo(chkNo);
                    if (WeaponFlag.Ins.FlagList[wNo] == false) { continue; }
                }
                else
                {
                    // �A�C�e�����S�đ�������Ă���ꍇ�i���\���ʂ����鏈�������A���̂Ƃ�����u�j
                    if (iEquipMaxFlag) { continue; }

                    // �t���O�`�F�b�N
                    if (ItemFlag.Ins.FlagList[chkNo] == false) { continue; }
                }
            }
            randList.Add(chkNo);        // ���X�g�ɒǉ�
        }
        return randList;
    }

}
