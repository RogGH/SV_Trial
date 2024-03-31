using UnityEngine;
using static EnemySOBJDataTable;

[System.Serializable]
public class EnemyJsonDataTable
{
    public EnemyJsonData[] table;
}

[System.Serializable]
public class EnemyJsonData
{
    public int id = 0;
    public string enumName = "";
    public string sprite = "";
    public string prefab = "";
    public int hp = 0;
    public int speed = 0;
    public int attack = 0;
    public int defense = 0;
    public string dropType = "";
}


// スクリプタブルオブジェクト
[System.Serializable]
[CreateAssetMenu(menuName = "MyScriptable/Create EnemyData")]
public class EnemySOBJData : ScriptableObject
{
    [SerializeField] public int id;
    [SerializeField] public string enumName;
    [SerializeField] public string sprite;
    [SerializeField] public string prefab;
    [SerializeField] public int hp;
    [SerializeField] public int speed;
    [SerializeField] public int attack;
    [SerializeField] public int defense;
    [SerializeField] public DropType dropType;
}

