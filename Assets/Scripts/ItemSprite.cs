using UnityEngine;

public class ItemSprite : MonoBehaviour
{
    Rigidbody2D rigid;

    float elapsedTime = 0f;
    float moveTime = 1f;
    bool isMovingUp = true;
    Vector3 startPos;

    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start(){
        startPos = transform.position;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        Vector3 targetPos;
        if(isMovingUp) targetPos = startPos + Vector3.up * 0.15f;
        else targetPos = startPos + Vector3.down * 0.15f;

        transform.position = new Vector3(transform.position.x,Mathf.SmoothStep(startPos.y,targetPos.y,elapsedTime/moveTime),transform.position.z);

        if(elapsedTime >= moveTime){
            elapsedTime = 0f;
            isMovingUp = !isMovingUp;
            startPos = transform.position;
        }
    }
}
