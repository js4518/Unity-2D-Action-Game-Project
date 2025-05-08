using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    RectTransform rect;
    Slider slider;
    public GameObject target;
    Enemy targetEnemy;

    void Awake(){
        rect = GetComponent<RectTransform>();
        slider = GetComponent<Slider>();
    }

    void OnGUI(){
        //rect.position = Camera.main.WorldToScreenPoint(target.transform.position + new Vector3(0,-0.2f,0));
        rect.position = target.transform.position + new Vector3(0,-0.2f,0);
        slider.value = targetEnemy.curHealth / targetEnemy.maxHealth;
    }


    public void Init(GameObject enemy){
        target = enemy;
        targetEnemy = target.GetComponent<Enemy>();
    }
}
