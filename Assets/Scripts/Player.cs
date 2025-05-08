using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    // 컴포넌트
    public Rigidbody2D rigid;
    public Collider2D coll;
    public SpriteRenderer sprite;
    public Animator anim;

    // 이동,공격 관련 변수
    public float moveSpeed;
    public Vector2 inputVec;
    public Vector2 attackVec;
    public Vector3 nextPos;
    public Vector2 lastDir = Vector2.right;
    public Vector2 lastAttackDir = Vector2.right;
    public string attackAxisH;
    public string attackAxisV;
    public bool isActing = false; // 이동에 영향을 주는 어떠한 특수 행동을 할 때

    float minX,maxX,minY,maxY; // 타일맵 범위

    // 체력 관련 변수
    public int maxHealth;
    public int curHealth;
    public UnityEvent onHit;
    public bool isInvincible = false;

    bool isInitialized = false;


    void Awake(){
        if(isInitialized) return;

        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        curHealth = maxHealth;
        attackAxisH = "SwordHorizontal";
        attackAxisV = "SwordVertical";

        isInitialized = true;
    }

    void Start(){
        Tilemap t = GameManager.inst.tilemap;
        t.CompressBounds();
        minX = t.localBounds.min.x;
        minY = t.localBounds.min.y;
        maxX = t.localBounds.max.x;
        maxY = t.localBounds.max.y;
    }

    void Update(){
        if(curHealth > maxHealth) curHealth = maxHealth;

        if(Input.GetKeyDown(KeyCode.B)){curHealth++; onHit.Invoke();}
        if(Input.GetKeyDown(KeyCode.N)){maxHealth += 2; onHit.Invoke();}
        if(Input.GetKeyDown(KeyCode.M)){maxHealth -= 2; onHit.Invoke();}
    }

    void FixedUpdate(){
        if(!isActing) rigid.linearVelocity = Vector2.zero;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        attackVec.x = Input.GetAxis(attackAxisH);
        attackVec.y = Input.GetAxis(attackAxisV);
        attackVec.x = Mathf.Abs(attackVec.x) >= 0.01f? Mathf.Sign(attackVec.x):0;
        attackVec.y = Mathf.Abs(attackVec.y) >= 0.01f? Mathf.Sign(attackVec.y):0;

        if(attackVec.normalized != Vector2.zero) lastAttackDir = attackVec.normalized;
        if(inputVec.normalized != Vector2.zero) lastDir = inputVec.normalized;
        if(isActing) return;
        nextPos = rigid.position + inputVec.normalized * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(new Vector3(Mathf.Clamp(nextPos.x,minX+sprite.size.x/2,maxX-sprite.size.x/2),Mathf.Clamp(nextPos.y,minY+sprite.size.y/2,maxY-sprite.size.y/2),nextPos.z));
    }

    void LateUpdate(){
        if(!isActing){if(inputVec.x != 0) sprite.flipX = inputVec.x > 0 ? false : true;} 
    }

    void OnCollisionStay2D(Collision2D coll){
        if(coll.gameObject.CompareTag("Enemy"))
            HitHandle(coll.relativeVelocity.normalized, 1);
    }

    public void HitHandle(Vector2 attackDir, int damage){
        if(isInvincible) return;

        curHealth -= damage;
        onHit.Invoke();
        isActing = isInvincible = true;
        anim.SetBool("isOnHit",true);

        rigid.AddForce(attackDir * 15,ForceMode2D.Impulse);

        StartCoroutine(Hit());
        // if(curHealth >= 0){
        //     StartCoroutine(Hit());     
        // }else{
            
        // }
    }

    IEnumerator Hit(){
        yield return new WaitForSeconds(0.2f);
        isActing = false;
        anim.SetBool("isOnHit",false);
        StartCoroutine(DisableInvincible());
    }

    IEnumerator DisableInvincible(){
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("endOfInvincible");
        isInvincible = false;
    }

     void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        GameManager.inst.player = this;
    }
}
