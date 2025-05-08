using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData", menuName = "Scriptable Object/SpawnData")]
public class SpawnData : ScriptableObject
{
    public enum SpawnType{Enemy,Item}
    public SpawnType type;

    public Vector3 position;
    public int id;
}
