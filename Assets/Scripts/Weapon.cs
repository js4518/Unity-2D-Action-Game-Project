using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    public WeaponData[] weaponData;
    public int curWeapon = -1;
    public List<Pair> activeWeapon = new List<Pair>();

    public Sprite GetWeaponSprite(int id){
        return weaponData[id].sprite;
    }

    public void GetWeapon(int id){
        foreach(Pair p in activeWeapon)
            if(p.weaponId == id) return;

        Transform w = Instantiate(weaponData[id].weaponPrefab,transform).transform;
        switch(id){
            case 0:
                w.localPosition = weaponData[0].weaponPos;
                w.localRotation = Quaternion.identity;
                w.Rotate(Vector3.forward * 45);
                w.GetComponent<Spear>().Init(weaponData[0]);
                break;
            case 1:
                w.localPosition = weaponData[1].weaponPos;
                w.localRotation = Quaternion.identity;
                w.GetComponent<Sword>().Init(weaponData[1]);
                break;
        }
        if(curWeapon != -1) activeWeapon[curWeapon].weaponTransform.tag = "InactiveWeapon";
        activeWeapon.Add(new Pair(id,w));
        curWeapon = activeWeapon.Count-1;
        GameManager.inst.suiManager.ChangeWeaponUI();
    }
    
    public void ChangeWeapon(){
        activeWeapon[curWeapon].weaponTransform.tag = "InactiveWeapon";
        if(++curWeapon >= activeWeapon.Count) curWeapon = 0;
        activeWeapon[curWeapon].weaponTransform.tag = "ActiveWeapon";
        GameManager.inst.suiManager.ChangeWeaponUI();
    }
}

public struct Pair{
    public int weaponId;
    public Transform weaponTransform;

    public Pair(int id, Transform tr){
        weaponId = id;
        weaponTransform = tr;
    }
}
