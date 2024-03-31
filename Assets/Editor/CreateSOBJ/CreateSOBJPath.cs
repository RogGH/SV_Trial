using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/CreateSOBJPath")]
public class CreateSOBJPath : ScriptableObject
{
    [Header("GSSのURL")]
    public string Sheet_URL;

    [Header("GASのURL")]
    public string GAS_URL;

    [Header("パス設定SOBJ：path")]
    public string SettingSObj_PATH;

    [Header("作成データ保存場所：path")]
    public string CreateData_PATH;
}