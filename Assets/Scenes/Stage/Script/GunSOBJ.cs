using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "MyScriptable/Create GunSOBJ")]
public class GunSOBJ : ScriptableObject
{
	[HideInInspector] public int id = 0;
	[HideInInspector] public string wName = "HundGun";

	[HideInInspector]public string key1 = "Get";
	[HideInInspector]public float value1 = 1;

	[TextArea(10, 10)]
	public string comment =
		  "KeyはDmg,Recast,Speed,CriHit,Numから入力\n"
		+ "valueは数値を入力\n"
		+ "stringは説明の文章を入力"
		+ "■入力例■\n"
		+ "攻撃力50%UP：key = Dmg, value = 50\n"
		+ "再使用速度25%UP：key = Recast, value =25\n"
		+ "弾数２発：key = Num, value = 2\n"
		+ "弾速50%UP：key = Speed, value = 50\n"
		+ "クリ率50%UP：key = CriHit, value = 50\n"
		;

	[Header("LV2設定")]
	[SerializeField] public string key2 = "Dmg";
	[SerializeField] public float value2 = 50;
	[SerializeField] public string string2 = "レベル２の説明";

	[Header("LV3設定")]
	[SerializeField] public string key3 = "Recast";
	[SerializeField] public float value3 = 25;
	[SerializeField] public string string3 = "レベル３の説明";

	[Header("LV4設定")]
	[SerializeField] public string key4 = "CriHit";
	[SerializeField] public float value4 = 50;
	[SerializeField] public string string4 = "レベル４の説明";

	[Header("LV5設定")]
	[SerializeField] public string key5 = "Num";
	[SerializeField] public float value5 = 2;
	[SerializeField] public string string5 = "レベル５の説明";

	[Header("LV6設定")]
	[SerializeField] public string key6 = "Speed";
	[SerializeField] public float value6 = 50;
	[SerializeField] public string string6 = "レベル６の説明";

	[Header("LV7設定")]
	[SerializeField] public string key7 = "Num";
	[SerializeField] public float value7 = 3;
	[SerializeField] public string string7 = "レベル７の説明";
}
