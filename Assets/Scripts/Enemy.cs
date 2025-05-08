using System.Collections;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 컴포넌트
    public Rigidbody2D rigid;
    CapsuleCollider2D coll;
    public SpriteRenderer sprite;
    EnemyTrait trait;

    // 데이터 변수
    public int enemyId;
    string enemyName;
    EnemyData.EnemyType enemyType;

    // 변수
    public float speed;
    public Vector2 nextDir;
    public bool isHit = false;
    public bool isDead = false;
    public bool isPlayerInRange = false;
    public float maxHealth;
    public float curHealth;
    public Color toColor;

    // UI 변수
    GameObject wui;

    // 컬러 변수
    
    
    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        trait = GetComponentInChildren<EnemyTrait>();

        rigid.freezeRotation = true;
        toColor = Color.white;
    }

    void FixedUpdate(){
        if(isHit) return;

        rigid.linearVelocity = Vector2.zero;
        nextDir = (GameManager.inst.player.rigid.position - rigid.position).normalized;

        switch(enemyType){
            case EnemyData.EnemyType.Melee:
                switch(enemyId){
                    case 2: // Wolf, Bird
                    case 3:
                         if(!trait.sprite.enabled) rigid.MovePosition(rigid.position + nextDir * speed * Time.fixedDeltaTime);
                        break;
                    default:
                        rigid.MovePosition(rigid.position + nextDir * speed * Time.fixedDeltaTime);
                        break;
                }
                break;
            case EnemyData.EnemyType.Range:
                if(!isPlayerInRange && !trait.sprite.enabled) rigid.MovePosition(rigid.position + nextDir * speed * Time.fixedDeltaTime);
                break;
        }
    }

    void LateUpdate(){
        sprite.color = toColor;
        if(!isDead) sprite.flipX = nextDir.x > 0? false : true;
    }

    public void Init(EnemyData data){
        enemyId = data.enemyId;
        enemyName = data.enemyName;
        enemyType = data.enemyType;
        sprite.sprite = data.sprite;
        speed = data.enemySpeed;
        maxHealth = data.enemyMaxHealth;
        curHealth = maxHealth;
        isHit = false;
        isDead = false;
        rigid.simulated = true;
        sprite.sortingOrder = 3;

        coll.offset = data.colliderOffset;
        coll.size = data.colliderSize;
        coll.direction = data.colliderDirection;
        trait.Init(data.enemyTraitTime, data.enemyTraitSpeed, data.enemyTraitCool);
        GetComponentInChildren<EnemyDetectionRange>().Init(data.detectionRangeOffset, data.detectionRangeRadius);

        wui = GameManager.inst.wuiManager.GetEnemyUI(gameObject);
    }

    public void HitHandle(float damage, Vector2 attackDir, float attackSpeed, float knockBack){
        if(isHit) return;

        curHealth -= damage;
        isHit = true;
        toColor = new Color(0.64f,0.13f,0.13f);
        rigid.AddForce(attackDir * knockBack,ForceMode2D.Impulse);
        if(curHealth > 0){
            StartCoroutine(Hit());     
        }else{
            StartCoroutine(Dead());
        }
    }

    IEnumerator Hit(){
        yield return new WaitForSeconds(0.15f);
        isHit = false;
        toColor = Color.white;
    }

    IEnumerator Dead(){
        Color curColor = Color.white;

        isDead = true;
        yield return new WaitForSeconds(0.25f);
        rigid.simulated = false;
        sprite.sortingOrder = 2;
        wui.SetActive(false);

        for(int i = 0; i < 2; i++){
            toColor = new Color(curColor.r,curColor.g,curColor.b,0.5f);
            yield return new WaitForSeconds(0.2f);
            toColor = new Color(curColor.r,curColor.g,curColor.b,1f);
            yield return new WaitForSeconds(0.2f);
        }
        isHit = false;
        trait.isOnTrait = false;
        GameManager.inst.enemyManager.enemyCount--;
        gameObject.SetActive(false);
    }
}

