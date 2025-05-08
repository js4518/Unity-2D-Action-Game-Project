using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    public Player player;
    public PoolManager pool;
    public EnemyManager enemyManager;
    public WUIManager wuiManager;
    public SUIManager suiManager;
    public PortalManager portalManager;
    public Weapon weapon;
    public Tilemap tilemap;

    bool isCool = false;

    // 맵 관련 변수
    bool isMapCreated = false;
    public int mapSizeX = 4, mapSizeY = 4;
    int roomCount = 7;
    public MapData[][] map;
    int[][] visited;
    public int curPosX, curPosY;

    void Awake(){
        if(inst == null){
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Alpha1)) enemyManager.SummonEnemy(Vector3.zero,0);
        if(Input.GetKeyDown(KeyCode.Alpha2)) enemyManager.SummonEnemy(Vector3.zero,1);
        if(Input.GetKeyDown(KeyCode.Alpha3)) enemyManager.SummonEnemy(Vector3.zero,2);
        if(Input.GetKeyDown(KeyCode.Alpha4)) enemyManager.SummonEnemy(Vector3.zero,3);

        if(Input.GetKeyDown(KeyCode.Z)) weapon.GetWeapon(0);
        if(Input.GetKeyDown(KeyCode.X)) weapon.GetWeapon(1);
        if(Input.GetKeyDown(KeyCode.C)) pool.SaveMapObject();
        if(Input.GetKeyDown(KeyCode.V)) enemyManager.InitRoom(true);

        if(Input.GetKeyDown(KeyCode.Space) && !isCool){
            weapon.ChangeWeapon();
            StartCoroutine(ChangeCooldown());
        }
    }

    IEnumerator ChangeCooldown(){
        isCool = true;
        yield return new WaitForSeconds(0.5f);
        isCool = false;
    }

    public struct MapData{
        public bool map;
        public int spawnIdx;
        public bool isCleared;
        public List<SavePair>[] savedData;
        public MapData(bool m, int i, bool c){
            map = m; spawnIdx = i; isCleared = c; savedData = null;
        }
    }

    public struct SavePair{
        public Vector3 pos;
        public int id;
        public SavePair(Vector3 p, int i){
            pos = p; id = i;
        }
    }

    void CreateMap(){
        map = new MapData[mapSizeX][];
        visited = new int[mapSizeX][];
        for(int i = 0; i < mapSizeX; i++){
            map[i] = new MapData[mapSizeY];
            visited[i] = new int[mapSizeY];
        }
        
        RandomMap();
        // for(int i = 0; i < mapSizeX; i++)
        //         for(int j = 0; j < mapSizeY; j++)
        //             Debug.Log(map[i][j]);
    }

    void InitMap(){
        for(int i = 0; i < mapSizeX; i++)
            for(int j = 0; j < mapSizeY; j++){
                map[i][j] = new MapData(false,0,false);
                visited[i][j] = 0;
            }
    }
                

    void RandomMap(){
        int assignedCount;
        int randX, randY, saveX = -1, saveY = -1;
        bool connectedFlag;

        do{
            InitMap();
            assignedCount = 0;
            connectedFlag = true;
            while(assignedCount < roomCount){ // 랜덤 roomCount칸 할당
                randX = Random.Range(0,mapSizeX); randY = Random.Range(0,mapSizeY);
                if(map[randX][randY].map == false){
                    map[randX][randY].map = true;
                    saveX = randX; saveY = randY;
                    assignedCount++;
                    map[randX][randY].spawnIdx = Random.Range(0,enemyManager.spawnLists.Count);
                    map[randX][randY].isCleared = false;
                }
            }
            DFS(saveX,saveY);
            for(int i = 0; i < mapSizeX; i++)
                for(int j = 0; j < mapSizeY; j++)
                    if(map[i][j].map == true && visited[i][j] == 0) connectedFlag = false;
        }while(!connectedFlag);

        curPosX = saveX; curPosY = saveY;
    }
    

    void DFS(int x, int y){
        visited[x][y] = 1;
        if(x>0 && map[x-1][y].map == true && visited[x-1][y] == 0) DFS(x-1,y);
        if(x<mapSizeX-1 && map[x+1][y].map == true && visited[x+1][y] == 0) DFS(x+1,y);
        if(y>0 && map[x][y-1].map == true && visited[x][y-1] == 0) DFS(x,y-1);
        if(y<mapSizeY-1 && map[x][y+1].map == true && visited[x][y+1] == 0) DFS(x,y+1);
    }

    void SetPortal(){
        portalManager.left.gameObject.SetActive(curPosX>0 && map[curPosX-1][curPosY].map == true? true : false);
        portalManager.right.gameObject.SetActive(curPosX<mapSizeX-1 && map[curPosX+1][curPosY].map == true? true : false);
        portalManager.up.gameObject.SetActive(curPosY<mapSizeY-1 && map[curPosX][curPosY+1].map == true? true : false);
        portalManager.down.gameObject.SetActive(curPosY>0 && map[curPosX][curPosY-1].map == true? true : false);
    }

    public void MoveMap(Portal.PortalType type, Vector3 nextPos){
        pool.SaveMapObject();
        switch(type){
            case Portal.PortalType.Left: curPosX--; break;
            case Portal.PortalType.Right: curPosX++; break;
            case Portal.PortalType.Up: curPosY++; break;
            case Portal.PortalType.Down: curPosY--; break;
        }

        SceneManager.LoadScene(0);
        player.transform.position = nextPos;
        // if(map[curPosX][curPosY].savedPool == null) Debug.Log("풀 저장 안됨");
    }

    void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        // 컴포넌트들 새로 등록
        pool = GameObject.Find("PoolManager").GetComponent<PoolManager>();
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        wuiManager = GameObject.Find("WUIManager").GetComponent<WUIManager>();
        suiManager = GameObject.Find("SUIManager").GetComponent<SUIManager>();
        portalManager = GameObject.Find("PortalManager").GetComponent<PortalManager>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        //weapon = GameObject.Find("Weapon").GetComponent<Weapon>();

        if(!isMapCreated){
            CreateMap();
            isMapCreated = true;
        }

        SetPortal();
        // Debug.Log(string.Format("X : {0}, Y : {1}",curPosX,curPosY));

        portalManager.Init();

        enemyManager.listID = map[curPosX][curPosY].spawnIdx;
        enemyManager.InitRoom(map[curPosX][curPosY].isCleared);

        suiManager.ChangeWeaponUI();
        suiManager.health.Init();
        suiManager.health.OnHit();
        suiManager.minimap.Init();
        suiManager.minimap.DrawMinimap(map);
    }
}