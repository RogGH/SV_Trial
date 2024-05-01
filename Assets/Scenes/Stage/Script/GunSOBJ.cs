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
		  "Key‚ÍDmg,Recast,Speed,CriHit,Num‚©‚ç“ü—Í\n"
		+ "value‚Í”’l‚ğ“ü—Í\n"
		+ "string‚Íà–¾‚Ì•¶Í‚ğ“ü—Í"
		+ "¡“ü—Í—á¡\n"
		+ "UŒ‚—Í50%UPFkey = Dmg, value = 50\n"
		+ "Äg—p‘¬“x25%UPFkey = Recast, value =25\n"
		+ "’e”‚Q”­Fkey = Num, value = 2\n"
		+ "’e‘¬50%UPFkey = Speed, value = 50\n"
		+ "ƒNƒŠ—¦50%UPFkey = CriHit, value = 50\n"
		;

	[Header("LV2İ’è")]
	[SerializeField] public string key2 = "Dmg";
	[SerializeField] public float value2 = 50;
	[SerializeField] public string string2 = "ƒŒƒxƒ‹‚Q‚Ìà–¾";

	[Header("LV3İ’è")]
	[SerializeField] public string key3 = "Recast";
	[SerializeField] public float value3 = 25;
	[SerializeField] public string string3 = "ƒŒƒxƒ‹‚R‚Ìà–¾";

	[Header("LV4İ’è")]
	[SerializeField] public string key4 = "CriHit";
	[SerializeField] public float value4 = 50;
	[SerializeField] public string string4 = "ƒŒƒxƒ‹‚S‚Ìà–¾";

	[Header("LV5İ’è")]
	[SerializeField] public string key5 = "Num";
	[SerializeField] public float value5 = 2;
	[SerializeField] public string string5 = "ƒŒƒxƒ‹‚T‚Ìà–¾";

	[Header("LV6İ’è")]
	[SerializeField] public string key6 = "Speed";
	[SerializeField] public float value6 = 50;
	[SerializeField] public string string6 = "ƒŒƒxƒ‹‚U‚Ìà–¾";

	[Header("LV7İ’è")]
	[SerializeField] public string key7 = "Num";
	[SerializeField] public float value7 = 3;
	[SerializeField] public string string7 = "ƒŒƒxƒ‹‚V‚Ìà–¾";
}
