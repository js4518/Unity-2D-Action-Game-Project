using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public enum ItemType{Weapon};
    public ItemType type;
    public int id;

    void Start(){
        SetSprite();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            switch(type){
                case ItemType.Weapon:
                    GetWeapon();
                    break;
            }
            gameObject.SetActive(false);
        }
    }

    public void SetSprite(){
        switch(type){
                case ItemType.Weapon:
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameManager.inst.weapon.GetWeaponSprite(id);
                    break;
            }
    }

    void GetWeapon(){
        GameManager.inst.weapon.GetWeapon(id);
    }
}
