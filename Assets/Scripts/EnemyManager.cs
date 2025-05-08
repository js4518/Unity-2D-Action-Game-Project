using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public EnemyData[] dataArr;
    public List<SpawnList> spawnLists;
    public int listID;
    int _enemyCount = 0;
    public int enemyCount{
        get{return _enemyCount;}
        set{
            if(_enemyCount != value){
                _enemyCount = value;
                if(_enemyCount == 0){
                    GameManager.inst.map[GameManager.inst.curPosX][GameManager.inst.curPosY].isCleared = true;
                    RewardRoom();
                    clearEvent.Invoke();
                }
            }
        }
    }
    public UnityEvent clearEvent;

    void Update(){
        if(Input.GetKeyDown(KeyCode.Alpha5))
            foreach(var data in spawnLists[listID].list)
                SummonEnemy(data.position,data.id);
    }

    public void InitRoom(bool isCleared){
        if(!isCleared){
            foreach(var data in spawnLists[listID].list){
                if(data.type == SpawnData.SpawnType.Enemy) SummonEnemy(data.position,data.id);
                else if(data.type == SpawnData.SpawnType.Item) SummonItem(data.position,data.id);
            }
        }else{
            clearEvent.Invoke();
            GameManager.inst.pool.LoadMapObject();
        }
        
        Instantiate(spawnLists[listID].terrainPrefab,GameManager.inst.tilemap.transform.parent);
    }

    void RewardRoom(){
        SummonItem(Vector3.zero,Random.Range(0,2)); // 임시
    }

    public void SummonEnemy(Vector3 pos, int enemyId){
        Transform newEnemy = GameManager.inst.pool.GetEnemy(0).transform;
        newEnemy.localPosition = pos;
        newEnemy.localRotation = Quaternion.identity;
        newEnemy.GetComponent<Enemy>().Init(dataArr[enemyId]);
        enemyCount++;
    }

    public void SummonItem(Vector3 pos, int itemID){
        Transform newItem = GameManager.inst.pool.GetMapObject(0).transform;
        newItem.localPosition = pos;
        newItem.localRotation = Quaternion.identity;
        newItem.GetComponent<Chest>().id = itemID;
        newItem.GetComponent<Chest>().SetSprite();
    }

    void OnDrawGizmos(){
        if(spawnLists[listID] == null) return;

        foreach(var data in spawnLists[listID].list){
            if(data.type == SpawnData.SpawnType.Enemy)
                Gizmos.color = Color.red;
            else if(data.type == SpawnData.SpawnType.Item)
                Gizmos.color = Color.blue;
            Gizmos.DrawSphere(data.position, 0.2f);
        }
    }
}
