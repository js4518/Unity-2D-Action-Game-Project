using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnList", menuName = "Scriptable Object/SpawnList")]
public class SpawnList : ScriptableObject
{
    public List<SpawnData> list;
    public GameObject terrainPrefab;
    public int spawnListID;
}
