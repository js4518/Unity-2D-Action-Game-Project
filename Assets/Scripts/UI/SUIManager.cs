using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SUIManager : MonoBehaviour
{
    public Image weaponImage;
    public Text weaponName;
    public SUI health;
    public SUI minimap;

    void Awake(){
        // weaponImage = transform.GetChild(1).GetChild(0).GetChild(0);
        // weaponName = transform.GetChild(1).GetChild(1);
        // health = transform.GetChild(0).GetChild(1).GetComponent<SUI>();
    }

    public void ChangeWeaponUI(){
        Weapon w = GameManager.inst.weapon;
        if(w.curWeapon == -1) return;

        if(!weaponImage.enabled) weaponImage.enabled = true;
        weaponImage.sprite = w.weaponData[w.activeWeapon[w.curWeapon].weaponId].sprite;
        weaponName.text = w.weaponData[w.activeWeapon[w.curWeapon].weaponId].name;
    }
}
