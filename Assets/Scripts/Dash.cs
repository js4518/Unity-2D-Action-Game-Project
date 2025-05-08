using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    // 컴포넌트
    Player player;
    SpriteRenderer sprite;

    // 변수
    public float dashSpeed;
    public float dashTime;
    float chargedTime;
    public float dashCooltime;
    bool isDashing = false;
    public bool canDash = true;
    float movePressedTime = 0f;
    KeyCode[] moveKeyCode = {KeyCode.W,KeyCode.A,KeyCode.S,KeyCode.D};
    KeyCode lastMove = KeyCode.None;
    
    void Awake(){
        player = GetComponentInParent<Player>();
        sprite = GetComponent<SpriteRenderer>();

        
    }

    void Update(){
        foreach(KeyCode cur in moveKeyCode){
            if(Input.GetKeyDown(cur)){
                //Debug.Log(cur);
                if(Time.time - movePressedTime <= 0.25f && lastMove == cur && canDash && !player.isActing){
                    //Debug.Log("Dashed");
                    StartCoroutine(DashStart()); 
                }
                movePressedTime = Time.time;
                lastMove = cur; 
            }
        }
        // if(Input.GetKeyDown(KeyCode.Space) && canDash && !player.isActing) StartCoroutine(DashStart());
    }

    void LateUpdate(){
        sprite.enabled = canDash ? true : false;

        if(isDashing) player.sprite.color = new Color(0.4f,0.4f,0.4f);
        else player.sprite.color = Color.white;
    }

    IEnumerator DashStart(){
        isDashing = player.isActing = true;
        canDash = false;
        player.coll.gameObject.layer = LayerMask.NameToLayer("DashingPlayer");

        chargedTime = 0f;
        while(chargedTime < dashTime){
            chargedTime += Time.deltaTime;
            player.rigid.MovePosition(player.rigid.position + player.lastDir * Time.fixedDeltaTime * dashSpeed);
            yield return null;
        }
        //player.rigid.AddForce(player.lastDir * dashSpeed, ForceMode2D.Impulse);
        //yield return new WaitForSeconds(dashTime);

        isDashing = player.isActing = false;
        player.coll.gameObject.layer = LayerMask.NameToLayer("Default");

        StartCoroutine(DashCoolDown());
    }

    IEnumerator DashCoolDown(){
        yield return new WaitForSeconds(dashCooltime);
        canDash = true;
    }
}

