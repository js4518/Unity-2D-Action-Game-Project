using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;

public class Spear : MonoBehaviour
{
    // 컴포넌트
    SpriteRenderer sprite;
    Player player;

    // 데이터
    WeaponData.WeaponType weaponType;
    public int weaponId;
    string weaponName;
    float damage;
    float shotCooltime;
    float projSpeed;
    float knockBack;


    // 차징, 샷 관련 변수
    bool isCharging = false;
    bool canShot = true;
    public float chargedTime = 0f;
    public float maxChargeTime;

    void Awake(){
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    void Update(){
        if(transform.CompareTag("InactiveWeapon")) return;

        if(player.attackVec != Vector2.zero && !isCharging && canShot) StartCoroutine(Shot());
        if(player.attackVec == Vector2.zero) isCharging = false;
    }

    public void Init(WeaponData data){
        weaponId = data.weaponId;
        weaponType = data.weaponType;
        weaponName = data.weaponName;
        damage = data.weaponDmg;
        shotCooltime = data.weaponCooltime;
        projSpeed = data.weaponProjSpeed;
        knockBack = data.weaponKnockBack;
        player = GameManager.inst.player;
        player.attackAxisH = "SpearHorizontal";
        player.attackAxisV = "SpearVertical";
    }

    IEnumerator Shot(){
        isCharging = true;
        canShot = false;
        chargedTime = 0f;
        sprite.enabled = true;
        sprite.color = Color.white;
        
        while(chargedTime < maxChargeTime || isCharging){
            if(!isCharging) break;
            chargedTime += Time.deltaTime;
            sprite.color = Color.Lerp(Color.white,Color.blue,Mathf.Min(1f,chargedTime/maxChargeTime));
            yield return null;
        }
    
        Transform proj = GameManager.inst.pool.GetWeapon(0).transform;
        SpearProj projComp = proj.GetComponent<SpearProj>();

        proj.localPosition = player.transform.position;
        proj.localRotation = Quaternion.identity;
        proj.rotation = Quaternion.FromToRotation(Vector3.up,player.lastAttackDir);
        proj.Rotate(45f * Vector3.forward);
        proj.Translate(player.lastAttackDir * 1f, Space.World);

        projComp.Init(projSpeed,damage,player.lastAttackDir,knockBack);
        projComp.damage *= 1 + Mathf.Min(1f,chargedTime/maxChargeTime);
        projComp.speed *= 1 + Mathf.Min(1f,chargedTime/maxChargeTime);
        
        proj.GetComponent<SpriteRenderer>().color = sprite.color;
        sprite.enabled = false;

        StartCoroutine(ShotCoolDown());
    }

    IEnumerator ShotCoolDown(){
        yield return new WaitForSeconds(shotCooltime);
        canShot = true;
    }
}