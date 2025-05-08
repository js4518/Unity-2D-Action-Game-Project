using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTrait : MonoBehaviour
{
    //컴포넌트
    public Sprite[] projSprites;
    Enemy enemy;
    public SpriteRenderer sprite;


    // 변수
    public bool isOnTrait = false;
    float traitSpeed;
    float traitCool;
    float chargedTime = 0f;
    float maxChargeTime;
    float randChargeTime;
    Quaternion rightRot = Quaternion.Euler(0,0,10f);
    Quaternion leftRot = Quaternion.Euler(0,0,-10f);

    void Awake(){
        enemy = transform.parent.GetComponent<Enemy>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Init(float enemyTraitTime, float enemyTraitSpeed, float enemyTraitCool){
        maxChargeTime = enemyTraitTime;
        traitSpeed = enemyTraitSpeed;
        traitCool = enemyTraitCool;

        switch(enemy.enemyId){
            case 1: // Rogue
                sprite.sprite = projSprites[0];
                break;
            default:
                sprite.sprite = null;
                break;
        }
    }

    public void TraitHandle(Collider2D other){ // other : 에너미의 탐지영역에 들어온 오브젝트(플레이어 or 플레이어의 공격)
        
        switch(enemy.enemyId){
            case 1: // Rogue
                if(!enemy.isHit) StartCoroutine(ShotDagger());
                break;
            case 2: // Wolf
                StartCoroutine(Rush());
                break;
            case 3: // Bird
                StartCoroutine(Dodge(other.transform.position,other.GetComponent<Proj>().dir));
                break;
        }
    }

    IEnumerator ShotDagger(){ // EnemyID 1 : Rogue / projSprites[0]
        isOnTrait = true;
        sprite.enabled = true;
        sprite.color = Color.white;

        transform.localPosition = new Vector3(0.88f,0.6f,0);

        chargedTime = 0f;
        randChargeTime = maxChargeTime + Random.Range(-0.2f,0.2f);

        while(chargedTime < randChargeTime){
            if(enemy.isHit) break;

            chargedTime += Time.deltaTime;
            sprite.color = Color.Lerp(Color.white,Color.blue,Mathf.Min(1f,chargedTime/randChargeTime));
            if(enemy.sprite.flipX) enemy.transform.rotation = Quaternion.Lerp(Quaternion.identity,leftRot,chargedTime / randChargeTime * 2.5f);
            else enemy.transform.rotation = Quaternion.Lerp(Quaternion.identity,rightRot,chargedTime / randChargeTime * 2.5f);
            yield return null;
        }

        if(!enemy.isHit){
            // 투사체 생성 등
            Transform proj = GameManager.inst.pool.GetEnemy(1).transform; // 1 : Dagger
            EnemyDaggerProj projComp = proj.GetComponent<EnemyDaggerProj>();

            proj.localPosition = enemy.transform.position + new Vector3(0,0.4f,0);
            proj.localRotation = Quaternion.identity;
            proj.rotation = Quaternion.FromToRotation(Vector3.up,enemy.nextDir);
            proj.Rotate(45f * Vector3.forward);

            projComp.Init(Random.Range(traitSpeed-3,traitSpeed+3),1,(enemy.nextDir * 7 + (new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f))).normalized).normalized);
            proj.GetComponent<SpriteRenderer>().color = sprite.color;
        }
        sprite.enabled = false;
        enemy.transform.rotation = Quaternion.identity;
        StartCoroutine(Cooldown(traitCool));
    }

    IEnumerator Rush(){ // EnemyID 2 : Wolf / no sprite
        if(enemy.isHit || enemy.isDead) yield break;

        Vector2 rushDir;

        isOnTrait = true;
        sprite.enabled = true; // 스프라이트 없지만 돌진 후 쿨타임 돌기 전 상태를 표현
        chargedTime = 0f;
        randChargeTime = maxChargeTime + Random.Range(-0.2f,0.2f);

         while(chargedTime < randChargeTime){
            chargedTime += Time.deltaTime;
            if(!enemy.isHit) enemy.toColor = Color.Lerp(Color.white,Color.yellow,Mathf.Min(1f,chargedTime/randChargeTime * 2.5f));
            yield return null;
        }

        rushDir = enemy.nextDir;
        chargedTime = 0f;
        while(chargedTime < 0.8f){
            if(enemy.isDead) break;
            chargedTime += Time.deltaTime;
            enemy.rigid.MovePosition(enemy.rigid.position + rushDir * Time.fixedDeltaTime * Mathf.Lerp(traitSpeed,0,chargedTime/0.8f));
            if(!enemy.isHit) enemy.toColor = Color.yellow;
            yield return null;
        }
        
        if(!enemy.isHit && !enemy.isDead) enemy.toColor = Color.white;
        sprite.enabled = false;
        StartCoroutine(Cooldown(traitCool));
    }

    IEnumerator Dodge(Vector3 attackPos, Vector2 attackDir){ // EnemyID 3 : Bird / no sprite
        // Vector2 dodgeDir = Vector2.Dot(new Vector2(-attackDir.y,attackDir.x),enemy.rigid.position) >= 0? new Vector2(attackDir.y,-attackDir.x) : new Vector2(-attackDir.y,attackDir.x);
        // if(enemy.rigid.position.x >= 0){
        //     if(dodgeDir.x > 0) dodgeDir.x = -dodgeDir.x;
        // }else{
        //     if(dodgeDir.x < 0) dodgeDir.x = -dodgeDir.x;
        // }
        // if(enemy.rigid.position.y >= 0){
        //     if(dodgeDir.y > 0) dodgeDir.y = -dodgeDir.y;
        // }else{
        //     if(dodgeDir.y < 0) dodgeDir.y = -dodgeDir.y;
        // }
        Vector2 dodgeDir;
        if(Mathf.Pow(attackDir.x,2)-Mathf.Pow(attackDir.y,2) != 0){
            float factor = (attackDir.x*(enemy.rigid.position.x-attackPos.x)+attackDir.y*(attackPos.y-enemy.rigid.position.y))/(Mathf.Pow(attackDir.x,2)-Mathf.Pow(attackDir.y,2));
            dodgeDir = (new Vector2(enemy.rigid.position.x-factor*attackDir.x-attackPos.x,enemy.rigid.position.y-factor*attackDir.y-attackPos.y)).normalized;
        }else{
            dodgeDir = (new Vector2(enemy.rigid.position.x-attackPos.x,enemy.rigid.position.y-attackPos.y)).normalized;
        }
        isOnTrait = true;
        sprite.enabled = true;

        chargedTime = 0f;


        while(chargedTime < 0.5f){
            chargedTime += Time.deltaTime;
            enemy.rigid.MovePosition(enemy.rigid.position + dodgeDir * Time.fixedDeltaTime * Mathf.Lerp(traitSpeed,0,chargedTime/0.5f));
            yield return null;
        }

        sprite.enabled = false;
        StartCoroutine(Cooldown(traitCool));
    }

    IEnumerator Cooldown(float cool){
        yield return new WaitForSeconds(cool);
        isOnTrait = false;
    }
}
