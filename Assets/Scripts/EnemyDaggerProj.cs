using System.Collections;
using UnityEngine;

public class EnemyDaggerProj : MonoBehaviour
{
    // 컴포넌트
    Rigidbody2D rigid;
    Collider2D coll;
    public SpriteRenderer sprite;

    // 방향, 속도, 데미지 변수
    public Vector2 dir;
    public float speed;
    public int damage;

    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate(){
        rigid.MovePosition(rigid.position + dir * speed * Time.fixedDeltaTime);
    }

    public void Init(float projSpeed, int enemyDamage, Vector2 attackDir){
        speed = projSpeed;
        damage = enemyDamage;
        dir = attackDir;
        StartCoroutine(Remove());
    }

    IEnumerator Remove(){
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            other.GetComponent<Player>().HitHandle(dir,damage);
            StopCoroutine(Remove());
            gameObject.SetActive(false);
        }
    }
}
