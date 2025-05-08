using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

public class EnemyDetectionRange : MonoBehaviour
{
    CircleCollider2D coll;
    Enemy enemy;
    EnemyTrait trait;

    void Awake(){
        coll = GetComponent<CircleCollider2D>();
        enemy = transform.parent.GetComponent<Enemy>();
        trait = transform.parent.GetComponentInChildren<EnemyTrait>();
    }

    public void Init(Vector2 offset, float radius){
        coll.offset = offset;
        coll.radius = radius;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")) enemy.isPlayerInRange = true;
        
        switch(enemy.enemyId){
            case 3:
                if(other.CompareTag("PlayerAttack") && !trait.isOnTrait) trait.TraitHandle(other);
                break;
        }
    }

    void OnTriggerStay2D(Collider2D other){
        switch(enemy.enemyId){
            case 1: // Rogue
            case 2: // Wolf
                if(other.CompareTag("Player") && !trait.isOnTrait) trait.TraitHandle(other);
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")) enemy.isPlayerInRange = false;
    }
}
