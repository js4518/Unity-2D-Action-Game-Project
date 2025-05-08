using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public SpriteRenderer sprite;
    public enum PortalType{Up,Down,Left,Right}
    public PortalType type;
    public Portal oppositePortal;
    bool canMove;

    void Awake(){
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Init(){
        GameManager.inst.enemyManager.clearEvent.AddListener(ActivePortal);
        canMove = false;
    }

    void ActivePortal(){
        sprite.color = new Color(0f,0f,0f,0.8f);
        canMove = true;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player") && canMove){
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex == 0? 1 : 0);
            Vector3 nextPos = Vector3.zero;
            switch(type){
                case PortalType.Left: nextPos = oppositePortal.transform.position + 2*Vector3.left; break;
                case PortalType.Right: nextPos = oppositePortal.transform.position + 2*Vector3.right; break;
                case PortalType.Up: nextPos = oppositePortal.transform.position + 2*Vector3.up; break;
                case PortalType.Down: nextPos = oppositePortal.transform.position + 2*Vector3.down; break;
            }
            GameManager.inst.MoveMap(type,nextPos);
        }
    }
}
