using System.Collections;
using UnityEngine;

public class SwordProj : MonoBehaviour
{
    // 컴포넌트
    Rigidbody2D rigid;
    Collider2D coll;
    public SpriteRenderer sprite;
    Animator anim;
    Proj commonProj;

    // 방향, 속도, 데미지 변수
    public Vector2 dir;
    public float speed;
    public float damage;
    public float knockBack;

    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        commonProj = GetComponent<Proj>();
    }

    void FixedUpdate(){
        rigid.MovePosition(rigid.position + dir * speed * Time.fixedDeltaTime);
    }

    void LateUpdate(){
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            gameObject.SetActive(false);
    }

    public void Init(float projSpeed, float weaponDamage, Vector2 lastDir, float weaponKnockBack){
        speed = projSpeed;
        damage = weaponDamage;
        dir = lastDir;
        commonProj.dir = dir;
        knockBack = weaponKnockBack;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy")) other.GetComponent<Enemy>().HitHandle(damage,dir,speed,knockBack);
    }
}
