using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Object/WeaponData")]
public class WeaponData : ScriptableObject
{
    public enum WeaponType{Melee, Range}

    [Header("# Main Info")]
    public WeaponType weaponType;
    public int weaponId;
    public string weaponName;
    public float weaponDmg;
    public float weaponCooltime;
    public float weaponProjSpeed;
    public float weaponKnockBack;
    public Vector3 weaponPos;
    public Sprite sprite;

    [Header("# Prefab")]
    public GameObject weaponPrefab;
}
