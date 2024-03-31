using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeaponInitJsonDataTable {
    public WeaponInitJsonData[] table;
}

[System.Serializable]
public class WeaponInitJsonData {
    public int id;
    public string enumName;
    public int atk;
    public float recastDef;
    public float recastMin;
    public float speed;
    public int num;
    public float time;
}

[System.Serializable]
[CreateAssetMenu(menuName = "MyScriptable/Create WeaponInitData")]
public class WeaponInitSOBJData : ScriptableObject
{
    [SerializeField]public int id;
    [SerializeField] public string enumName;
    [SerializeField] public int atk;
    [SerializeField] public float recastDef;
    [SerializeField] public float recastMin;
    [SerializeField] public float speed;
    [SerializeField] public int num;
    [SerializeField] public float time;
}
