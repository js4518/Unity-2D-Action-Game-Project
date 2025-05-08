using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // 컴포넌트
    SpriteRenderer sprite;
    Player player;

    // 데이터
    WeaponData.WeaponType weaponType;
    public int weaponId;
    string weaponName;
    Vector3 weaponPos,weaponPosRev;
    float damage;
    float shotCooltime;
    float projSpeed;
    float knockBack;

    // 공격 대시 모션 변수
    public float dashSpeed;
    public float dashTime;
    float chargedTime = 0f;

    // 샷 관련 변수
    bool canShot = true;
    bool isOnShot= false;
    Vector2 shotDir;
    int curShot = 0;
    float lastAttackTime = 0f;

    // 입력키 변수
    KeyCode[] attackKeyCode = {KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.DownArrow,KeyCode.RightArrow};

    void Awake(){
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update(){
        if(transform.CompareTag("InactiveWeapon")) return;

        if(player.attackVec != Vector2.zero && canShot && !isOnShot){
            if(Time.time - lastAttackTime >= 0.8f) curShot = 0;
            StartCoroutine(Shot());
        }
        if(player.attackVec == Vector2.zero) isOnShot = false;
    }

    void LateUpdate(){
        sprite.enabled = transform.CompareTag("ActiveWeapon");
        sprite.flipX = player.sprite.flipX;
        if(sprite.flipX) transform.localPosition = weaponPosRev;
        else transform.localPosition = weaponPos;
    }

    public void Init(WeaponData data){
        weaponId = data.weaponId;
        weaponType = data.weaponType;
        weaponName = data.weaponName;
        weaponPos = data.weaponPos;
        weaponPosRev = new Vector3(-weaponPos.x,weaponPos.y,0);
        damage = data.weaponDmg;
        shotCooltime = data.weaponCooltime;
        projSpeed = data.weaponProjSpeed;
        knockBack = data.weaponKnockBack;
        player = GameManager.inst.player;
        player.attackAxisH = "SwordHorizontal";
        player.attackAxisV = "SwordVertical";
    }

    IEnumerator Shot(){
        isOnShot = player.isActing = true;
        canShot = false;
        
        while(isOnShot){
            if(player.isInvincible) break;

            shotDir = player.lastAttackDir;
            player.sprite.flipX = shotDir.x >= 0? false : true;

            if(++curShot % 3 != 0){ // 1, 2타
                chargedTime = 0f;
                while(chargedTime < dashTime){
                    chargedTime += Time.deltaTime;
                    player.rigid.MovePosition(player.rigid.position + shotDir * Time.fixedDeltaTime * dashSpeed);
                    transform.localRotation = Quaternion.Euler(0,0,(!sprite.flipX?1:-1) * Mathf.Lerp(30,-30,chargedTime/dashTime));
                    yield return null;
                }
                transform.localRotation = Quaternion.identity;

                CreateProj(1);
                lastAttackTime = Time.time;

                chargedTime = 0f;
                while(chargedTime < shotCooltime){
                    if(!isOnShot) break;
                    chargedTime += Time.deltaTime;
                    yield return null;
                }
                if(!isOnShot) break;
            }else{ // 3타
                curShot = 0;
                chargedTime = 0f;
                while(chargedTime < dashTime){
                    chargedTime += Time.deltaTime;
                    player.rigid.MovePosition(player.rigid.position + shotDir * Time.fixedDeltaTime * dashSpeed * 2);
                    transform.localRotation = Quaternion.Euler(0,0,(!sprite.flipX?1:-1) * Mathf.Lerp(30,-30,chargedTime/dashTime));
                    yield return null;
                }
                transform.localRotation = Quaternion.identity;

                CreateProj(2);
                lastAttackTime = Time.time;

                chargedTime = 0f;
                while(chargedTime < shotCooltime * 5){
                    if(!isOnShot) break;
                    chargedTime += Time.deltaTime;
                    yield return null;
                }
                if(!isOnShot) break;
            }
        }

        isOnShot = player.isActing = false;
        StartCoroutine(ShotCoolDown(shotCooltime * (curShot == 0? 5 : 1)));
    }

    IEnumerator ShotCoolDown(float cool){
        yield return new WaitForSeconds(cool);
        canShot = true;
    }

    void CreateProj(int mag){
        Transform proj = GameManager.inst.pool.GetWeapon(1).transform;
        SwordProj projComp = proj.GetComponent<SwordProj>();

        proj.localScale = mag * Vector3.one;
        proj.localPosition = player.transform.position;
        proj.localRotation = Quaternion.identity;
        proj.rotation = Quaternion.FromToRotation(Vector3.up,player.lastAttackDir);
        proj.Rotate(-28f * Vector3.forward);
        proj.Translate(player.lastAttackDir * 2f, Space.World);

        projComp.Init(projSpeed,damage*mag,player.lastAttackDir,knockBack*mag);
    }
}
