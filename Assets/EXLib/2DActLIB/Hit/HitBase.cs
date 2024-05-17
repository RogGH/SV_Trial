using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HitResult;
/*
	ヒットの仕様

	HitBaseを親にアタッチする。
	最大体力を設定Unityで設定する。

	子供としてHitAtkDataやHitDefDataを持たせたオブジェクトを生成する


	プレイヤーのRigidBody2DのSleepModeをNeverSleepにしておいた方が良い
	（接触し続けると当たらなくなる）
 */

public class HitBase : MonoBehaviour
{
	[SerializeField] int maxHp = 1;         // 最大体力
	[SerializeField] int hp = 0;            // 現在の体力

	[SerializeField] bool atkActive = true; // 攻撃有効
	[SerializeField] bool defActive = true; // 防御有効

	HitResult result = new HitResult();     // ヒット結果

	// ダメージ時関数
	public delegate void DamageFunc();
	private DamageFunc dmgFunc;
	// 死亡時関数
	public delegate bool DieFunc();
	private DieFunc dieFunc;
	// 接触時関数
	public delegate void AtkReaction(HitAtkData atk, HitDefData def);
	private AtkReaction setAtkReactFunc;

	// 開始
	private void Awake() {
		hp = maxHp;
	}

	// ---------------------------------------------------------------
	// 外部サービスここから
	// ---------------------------------------------------------------
	// プロパティ
	public int HP { get { return hp; } set { hp = value; } }			// HP
	public int MaxHP { get { return maxHp; } set { maxHp = value; } }	// 最大HP
	public HitResult Result {											// ヒット結果
		get { return result; } set { result = value; }
	}
	public AtkReaction AtkReactFunc {                               // 攻撃時の関数
		get { return setAtkReactFunc; } set { setAtkReactFunc = value; }
	}

	// 旧サービス（そのうち消すかも）
	public bool AtkActive { get { return atkActive; } set { atkActive = value; } }
	public bool DefActive { get { return defActive; } set { defActive = value; } }

	// ---------------------------------------------------------------
	// 必須関数
	// ---------------------------------------------------------------
	// ヒットベース初期化
	public void Setup(DamageFunc damage, DieFunc die) {
		dmgFunc = damage;
		dieFunc = die;
	}

	// アップデート先処理
	public bool PreUpdate()
	{
		// 死亡時関数
		if (CheckDie()) {
			if (dieFunc != null) { return dieFunc(); }
		}
		// ダメージ時関数
		else if (CheckDamage()) {
			if (dmgFunc != null) { dmgFunc(); }
		}
		return false;       // 
	}

	// アップデート後処理
	public void PostUpdate() {
		result.ClearAllFlag();
	}


	// ---------------------------------------------------------------
	// その他サービス
	// ---------------------------------------------------------------
	// 死亡したか
	public bool CheckDie() { return HP <= 0 ? true : false; }


	// 攻撃でダメージを与えたか
	public bool CheckAttackDamage() { return Result.CheckAtkFlag(AtkFlag.AtkDoneDamage); }
	// 攻撃で何かしらに接触したか
	public bool CheckAttackHit() { return Result.CheckAtkFlag(AtkFlag.AtkHit); }


	// 防御でダメージを受けたか
	public bool CheckDefDamage() { return Result.CheckDefFlag(DefFlag.DefDoneDamage); }
	// 防御チェック判定をしたか
	public bool CheckDefCheckOnly() { return Result.CheckDefFlag(DefFlag.DefDoneCheck); }
	// 防御で何かしらに接触したか
	public bool CheckDefHit() { return Result.CheckDefFlag(DefFlag.DefHit); }



	// 古いヤツ
	public bool CheckDamage() { return Result.CheckDefFlag(DefFlag.DefDoneDamage); }
	public bool CheckAttack() { return Result.CheckAtkFlag(AtkFlag.AtkDoneDamage); }


	// ---------------------------------------------------------------
	// 判定一括設定
	// ---------------------------------------------------------------
	/*	
	 	子供が持つ攻撃、防御当たりを一括で設定出来るような設定
		個別で設定する場合は自前で行う
	*/

	// 判定有効化（接触判定自体を行わなくなる）
	public void SetAtkActive(bool value) { atkActive = value; }
	public void SetDefActive(bool value) { defActive = value; }
}
