using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Object/EnemyData")]
public class EnemyData : ScriptableObject
{
    public enum EnemyType{Melee, Range}

    [Header("# Main Info")]
    public EnemyType enemyType;
    public int enemyId;
    public string enemyName;
    public float enemyMaxHealth;
    public float enemySpeed;
    public float enemyTraitTime;
    public float enemyTraitSpeed;
    public float enemyTraitCool;
    public Sprite sprite;
    public Vector2 colliderOffset;
    public Vector2 colliderSize;
    public CapsuleDirection2D colliderDirection;
    public Vector2 detectionRangeOffset;
    public float detectionRangeRadius;

    // [Header("# Prefab")]
    // public GameObject enemyPrefab;
}
