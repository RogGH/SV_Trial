using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemFlag : MonoBehaviour
{
    private static ItemFlag ins;
    public static ItemFlag Ins
    {
        get
        {
            if (ins == null)
            {
                ins = (ItemFlag)FindObjectOfType(typeof(ItemFlag));
                if (ins == null) { Debug.LogError(typeof(ItemFlag) + "is nothing"); }
            }
            return ins;
        }
    }

    [Header("最大HPアップ")][SerializeField] bool MaxHPUp = false;
    [Header("自動回復アップ")] [SerializeField] bool AutoHealUp = false;
    [Header("防御力アップ")] [SerializeField] bool DefenseUp = false;
    [Header("移動速度アップ")] [SerializeField] bool MoveSpdUp = false;
    [Header("収集範囲アップ")] [SerializeField] bool MagnetUp = false;
    [Header("経験値アップ")] [SerializeField] bool ExpUp = false;
    [Header("金アップ")] [SerializeField] bool GoldUp = false;
    [Header("運アップ")] [SerializeField] bool LuckUp = false;
    [Header("与ダメージアップ")] [SerializeField] bool DamageUp = false;
    [Header("クリティカルアップ")] [SerializeField] bool CriticalUp = false;
    [Header("攻撃時間アップ")] [SerializeField] bool AtkTimeUp = false;
    [Header("弾数アップ")] [SerializeField] bool ShotNumUp = false;
    [Header("攻撃範囲アップ")] [SerializeField] bool AtkAreaUp = false;
    [Header("弾速アップ")] [SerializeField] bool AtkMoveSpdUp = false;
    [Header("リキャストアップ")] [SerializeField] bool RecastUp = false;

    [HideInInspector]
    public List<bool> FlagList;

	private void Start()
	{
        FlagList.Add(MaxHPUp);
        FlagList.Add(AutoHealUp);
        FlagList.Add(DefenseUp);
        FlagList.Add(MoveSpdUp);
        FlagList.Add(MagnetUp);
        FlagList.Add(ExpUp);
        FlagList.Add(GoldUp);
        FlagList.Add(LuckUp);
        FlagList.Add(DamageUp);
        FlagList.Add(CriticalUp);
        FlagList.Add(AtkTimeUp);
        FlagList.Add(ShotNumUp);
        FlagList.Add(AtkAreaUp);
        FlagList.Add(AtkMoveSpdUp);
        FlagList.Add(RecastUp);
    }
};