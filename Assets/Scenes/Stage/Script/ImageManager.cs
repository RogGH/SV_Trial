using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイコン番号
public enum IconNo
{
    // アイテム
    ItemTop,
    MaxHPUp = ItemTop,      // 最大HPアップ
    AutoHealUp,             // 自動回復アップ
    DefenseUp,              // 防御力アップ
    MoveSpdUp,              // 移動速度アップ

    MagnetUp,               // 収集範囲アップ
    ExpUp,                  // 経験値アップ
    GoldUp,                 // 金アップ
    LuckUp,                 // 運アップ

    DamageUp,               // 与ダメージアップ    
    CriticalUp,             // クリティカルアップ
    AtkTimeUp,              // 攻撃時間アップ
    ShotNumUp,              // 弾数アップ

    AtkAreaUp,              // 攻撃範囲アップ
    AtkMoveSpdUp,           // 弾速アップ
    RecastUp,               // リキャストアップ

    ItemEnd,
    ItemNum = ItemEnd - ItemTop,


    // 武器
    WeaponTop = ItemEnd,
    MShot = WeaponTop,      // ショット
    MDrill,                 // ドリル
    MSaw,                   // 回転のこぎり
    MFrameThrower,          // 火炎放射
    MTurret,                // タレット

    // オリジナル：
    TVWeaponPima,           // ぴま：検知

    WeaponEnd,
    WeaponNum = WeaponEnd - WeaponTop,

    // 取得
    UseItemTop = WeaponEnd,
    Money = UseItemTop,     // 金
    Potion,                 // ポーション

    UseItemEnd,
    UseItemNum = UseItemEnd - UseItemTop,


    // その他定義
    LevelUpItemNum = ItemNum + WeaponNum,


    Invalid = -1,
    end
};


// イメージ管理
public class ImageManager : MonoBehaviour
{
    private static ImageManager ins;
    public static ImageManager Ins
    {
        get
        {
            if (ins == null)
            {
                ins = (ImageManager)FindObjectOfType(typeof(ImageManager));
                if (ins == null) { Debug.LogError(typeof(ImageManager) + "is nothing"); }
            }
            return ins;
        }
    }

    public Sprite[] iconTbl;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    // アイコン番号から武器かチェック
    public bool CheckIconNoIsWeapon(int iNo)
    {
        if ((int)IconNo.WeaponTop <= iNo && iNo < (int)IconNo.WeaponEnd) { return true; }
        return false;
    }
    // アイコン番号がドロップアイテムかチェック
    public bool CheckIconNoIsUseItem(int iNo)
    {
        if ((int)IconNo.UseItemTop <= iNo && iNo < (int)IconNo.UseItemEnd) { return true; }
        return false;
    }


    // アイコン番号から武器番号（連番）へ
    public int ConvIconNoToWeaponSerialNo(int iNo)
    {
        if (CheckIconNoIsWeapon(iNo)) { 
            return (iNo - (int)IconNo.WeaponTop);
        }
        return (int)IconNo.Invalid;
    }

    // アイテム連番からアイコン番号へ
    public int ConvItemSerialNoToIconNo(int iNo) {
        return iNo;
    }

    // 武器連番からアイコン番号へ
    public int ConvWeaponSerialNoToIconNo(int wsNo)
    {
        return wsNo + (int)IconNo.WeaponTop;
    }


    // スプライト取得
    public Sprite GetSprite(int no)
    {
        if (iconTbl.Length < no) {
            Debug.Log("Error!! iconLength = " + iconTbl.Length + ": no = " + no);
        }
        return iconTbl[no];
    }
}
