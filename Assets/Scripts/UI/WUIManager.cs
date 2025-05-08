using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WUIManager : MonoBehaviour
{
    public GameObject GetEnemyUI(GameObject enemy){
        GameObject newEnemyUI = GameManager.inst.pool.GetUI(0);
        newEnemyUI.transform.SetParent(transform,false);
        newEnemyUI.GetComponent<EnemyUI>().Init(enemy);

        return newEnemyUI;
    }
}
