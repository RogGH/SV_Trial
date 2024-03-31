using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemySOBJDataTable : ScriptableObject
{
    public List<EnemySOBJData> data = new List<EnemySOBJData>();
}