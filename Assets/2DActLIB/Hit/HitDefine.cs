using UnityEngine;

/*
    ヒットの状態について

    Active状態
    アタリ判定を行うか行わないか。
    接触判定を行わなくなる
    （簡単に無敵判定を行うならコレ）
 */ 



// ーーーーーーーーーーーー共通ーーーーーーーーーーーーーーーー
// ヒット定義
static class HitDefine{
    public const float ComboInterval = 0.3f;        // コンボ間隔（今は一律で設定）

    public const float CriDmgRateDef = 1.5f;        // クリティカルダメージ（基本）

    public const bool DebugDisp = false;             // デバッグ表示（まだ拡縮非対応）


    // ヒットベース取得
    public static HitBase getHitBase(GameObject obj) {
        HitBase hb = obj.transform.root.GetComponent<HitBase>();
        if (hb == null) { Debug.Log("HitBase is None!"); }
        return hb;
    }


    // 矩形表示
    public static void RectDisp(float dir, Vector3 offset, Vector3 size, Vector3 pos, Color col)
    {
        Vector3 center = pos;

        center.x += offset.x * (dir < 0 ? -1 : 1);
        center.y += offset.y;

        float hx = size.x / 2;
        float hy = size.y / 2;

        Vector3 ul = center; ul.x -= hx; ul.y += hy;
        Vector3 ur = center; ur.x += hx; ur.y += hy;
        Vector3 dl = center; dl.x -= hx; dl.y -= hy;
        Vector3 dr = center; dr.x += hx; dr.y -= hy;

        Debug.DrawLine(ul, ur, col);
        Debug.DrawLine(ul, dl, col);
        Debug.DrawLine(dl, dr, col);
        Debug.DrawLine(ur, dr, col);
    }
}

// タイプ（攻撃か防御か）
public enum HitType{ ATK, DEF, };
// レイヤー（PL,敵、中立）
public enum HitLayer{ Player, Enemy, Neutral, };

// 属性（各ゲームにて個別設定）
// 一つのみ設定
public enum HitElement { 
    None,
    Fire,
    Thunder,
    Ice,
};

// 形状
public enum HitShape {
    Rect,               // 矩形
    Circle,             // 円

    Num
};


// ーーーーーーーーーーーー共通ーーーーーーーーーーーーーーーー


// ーーーーーーーーーーーー攻撃専用ーーーーーーーーーーーーーー
// 攻撃種類
public enum HitAtkKind {
    Body,   // 体（一度触れた後、離れてから触れるともう一度当たる）
    Shell,  // 弾（一度触れた後、離れてから触れても当たらない）
};

// 攻撃付加（吹き飛ばしとか、貫通とか、その辺を設定したりする予定）
// 複数設定可能
public enum HitAtkSP {
    None    = 0,            // 無し
    Blow    = (1 << 0),     // ふきとばし
    Through = (1 << 1),     // 貫通
};

// ーーーーーーーーーーーー攻撃専用ーーーーーーーーーーーーーー


// ーーーーーーーーーーーー防御専用ーーーーーーーーーーーーーー
// 防御種類（反射とか、そういうのを設定したりする予定）
public enum HitDefKind {
    None,               // なし
    Invincivle,         // 無敵（当たるけどダメージ計算しない）
    Avoid,              // 回避（当たるけどダメージ計算しない）
    CheckOnly,          // チェック用（無効中だろうが常にチェックされる）
};

// 防御追加効果（）
public enum HitDefPlus {
    None,
    Refrect,
};

// ーーーーーーーーーーーー防御専用ーーーーーーーーーーーーーー


// ヒット結果
public class HitResult
{
    public enum AtkFlag {
        None = 0,
        AtkDoneDamage       = (1 << 1),     // ダメージを与えた 
        AtkDoneHeal         = (1 << 2),     // 回復させた 
        AtkDoneNoDamage     = (1 << 3),     // ノーダメだった
        AtkDoneInvincible   = (1 << 4),     // 無敵中の相手に攻撃した
        AtkDoneAboid        = (1 << 5),     // 回避中の相手に攻撃した
        AtkDoneCheck        = (1 << 6),     // チェック判定に攻撃した
        AtkHit              = (1 << 7),     // 防御と接触している
        AtkDoneCritical     = (1 << 8),     // クリティカルした
        End
    };
    public enum DefFlag
    {
        None = 0,
        DefDoneDamage       = (1 << 1),     // ダメージを受けた
        DefDoneHeal         = (1 << 2),     // 回復を受けた
        DefDoneNoDamage     = (1 << 3),     // ノーダメージを受けた
        DefDoneInvincible   = (1 << 4),     // 無敵中に受けた
        DefDoneAvoid        = (1 << 5),     // 回避中に受けた
        DefDoneCheck        = (1 << 6),     // チェック判定をした
        DefHit              = (1 << 7),     // 攻撃と接触している

        End
    };

    int damage;
    Vector3 hitPos;
    AtkFlag atkFlag;
    DefFlag defFlag;

    // プロパティ
    public int Damage { get { return damage; } set { damage = value; } }
    public Vector3 HitPos { get { return hitPos; } set { hitPos = value; } }

    // フラグ関連
    bool checkFlag(int flag) { return flag != 0 ? true : false; }
    // 攻撃フラグ関連
    public bool CheckAtkFlag(AtkFlag flag) { return checkFlag((int)(atkFlag & flag)); }
    public void SetAtkFlag(AtkFlag setFlag) { atkFlag |= setFlag; }
    public void ClearAtkFlag() { atkFlag = 0; }
    // 防御フラグ関連
    public bool CheckDefFlag(DefFlag flag) { return checkFlag((int)(defFlag & flag)); }
    public void SetDefFlag(DefFlag setFlag) { defFlag |= setFlag; }
    public void ClearDefFlag() { defFlag = 0; }
    // 全フラグ
    public void ClearAllFlag() { ClearAtkFlag(); ClearDefFlag(); }
};
