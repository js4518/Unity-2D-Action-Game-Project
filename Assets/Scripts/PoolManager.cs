using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PoolManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    List<GameObject>[] enemyPools;

    public GameObject[] weaponPrefabs;
    List<GameObject>[] weaponPools;

    public GameObject[] uiPrefabs;
    List<GameObject>[] uiPools;

    public GameObject[] mapObjectPrefabs;
    List<GameObject>[] mapObjectPools;


    void Awake(){
        enemyPools = new List<GameObject>[enemyPrefabs.Length];
        weaponPools = new List<GameObject>[weaponPrefabs.Length];
        uiPools = new List<GameObject>[uiPrefabs.Length];
        mapObjectPools = new List<GameObject>[mapObjectPrefabs.Length];

        for(int i = 0; i < enemyPools.Length; i++)
            enemyPools[i] = new List<GameObject>();
        for(int i = 0; i < weaponPools.Length; i++)
            weaponPools[i] = new List<GameObject>();
        for(int i = 0; i < uiPools.Length; i++)
            uiPools[i] = new List<GameObject>();
        for(int i = 0; i < mapObjectPools.Length; i++)
            mapObjectPools[i] = new List<GameObject>();
    }

    public GameObject GetEnemy(int idx){
        GameObject select = null;

        foreach(GameObject item in enemyPools[idx])
            if(!item.activeSelf){
                select = item;
                select.SetActive(true);
                break;
            }

        if(!select){
            select = Instantiate(enemyPrefabs[idx],transform);
            enemyPools[idx].Add(select);
        }

        return select;
    }

    public GameObject GetWeapon(int idx){
        GameObject select = null;

        foreach(GameObject item in weaponPools[idx])
            if(!item.activeSelf){
                select = item;
                select.SetActive(true);
                break;
            }

        if(!select){
            select = Instantiate(weaponPrefabs[idx],transform);
            weaponPools[idx].Add(select);
        }

        return select;
    }

    public GameObject GetUI(int idx){
        GameObject select = null;

        foreach(GameObject item in uiPools[idx])
            if(!item.activeSelf){
                select = item;
                select.SetActive(true);
                break;
            }

        if(!select){
            select = Instantiate(uiPrefabs[idx],transform);
            uiPools[idx].Add(select);
        }

        return select;
    }

    public GameObject GetMapObject(int idx){
        GameObject select = null;

        foreach(GameObject item in mapObjectPools[idx])
            if(!item.activeSelf){
                select = item;
                select.SetActive(true);
                break;
            }

        if(!select){
            select = Instantiate(mapObjectPrefabs[idx],transform);
            mapObjectPools[idx].Add(select);
        }

        return select;
    }

    public void SaveMapObject(){
        GameManager.inst.map[GameManager.inst.curPosX][GameManager.inst.curPosY].savedData = new List<GameManager.SavePair>[mapObjectPools.Length];
        List<GameManager.SavePair>[] target = GameManager.inst.map[GameManager.inst.curPosX][GameManager.inst.curPosY].savedData;
        //Debug.Log(string.Format("targetPool 할당 / {0},{1}",GameManager.inst.curPosX,GameManager.inst.curPosY));
        for(int i = 0; i < mapObjectPools.Length; i++){
            target[i] = new List<GameManager.SavePair>();
            switch(i){
                case 0: // Chest
                    foreach(var data in mapObjectPools[i])
                        if(data.activeSelf) target[i].Add(new GameManager.SavePair(data.transform.position,data.GetComponent<Chest>().id));
                    break;
            }
        }
    }

    public void LoadMapObject(){
        EnemyManager enemyManager = GameManager.inst.enemyManager;
        List<GameManager.SavePair>[] target = GameManager.inst.map[GameManager.inst.curPosX][GameManager.inst.curPosY].savedData;

        if(target == null) return;

        for(int i = 0; i < mapObjectPools.Length; i++){
            switch(i){
                case 0: // Chest
                    foreach(var data in target[i])
                        enemyManager.SummonItem(data.pos,data.id);
                    break;
            }
        }

        // 삭제
        for(int i = 0; i < mapObjectPools.Length; i++){
            target[i].Clear();
            target[i] = null;
        }
        GameManager.inst.map[GameManager.inst.curPosX][GameManager.inst.curPosY].savedData = null;
        //Debug.Log("현재 맵의 풀 삭제됨(정상 로드됨)");
    }
}
