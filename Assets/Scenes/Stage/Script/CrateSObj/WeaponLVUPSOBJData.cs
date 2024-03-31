using UnityEngine;

[System.Serializable]
public class WeaponLVUPJsonDataTable
{
    public WeaponLVUPJsonData[] table;
}

[System.Serializable]
public class WeaponLVUPJsonData
{
    public int id;
    public string weaponName;
    public string key1;
    public string key2;
    public string key3;
    public string key4;
    public string key5;
    public string key6;
    public string key7;
    public float value1;
    public float value2;
    public float value3;
    public float value4;
    public float value5;
    public float value6;
    public float value7;
}

// スクリプタブルオブジェクト
[System.Serializable]
[CreateAssetMenu(menuName = "MyScriptable/Create WeaponLVUPData")]
public class WeaponLVUPSOBJData : ScriptableObject
{
    [SerializeField] public int id;
    [SerializeField] public string weaponName;
    [SerializeField] public string[] key;
    [SerializeField] public float[] value;
}
