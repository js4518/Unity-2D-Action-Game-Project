using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class SUI : MonoBehaviour
{
    public enum SUIType{HealthBar,Minimap}
    public SUIType type;

    // HealthBar
    public Sprite[] heartSprite;
    List<Heart> heartList;
    public Player player;

    // Minimap
    public GameObject room;
    GridLayoutGroup grid;
    
    int mapSizeX, mapSizeY;
    int curPosX, curPosY;
    
    public void Init(){
        player = GameManager.inst.player;
        switch(type){
            case SUIType.HealthBar:
                heartList = new List<Heart>();
                // 최초 생성 : 플레이어 최대체력/2 + 최대체력%2
                ChangeHeartCount(player.maxHealth/2 + player.maxHealth%2);
                player.onHit.AddListener(OnHit);
                break;
            case SUIType.Minimap:
                grid = GetComponent<GridLayoutGroup>();
                mapSizeX = GameManager.inst.mapSizeX; mapSizeY = GameManager.inst.mapSizeY;
                curPosX = GameManager.inst.curPosX; curPosY = GameManager.inst.curPosY;
                grid.GetComponent<RectTransform>().sizeDelta = new Vector2(10*mapSizeX,10*mapSizeY);
                grid.constraintCount = mapSizeY;
                break;
        }
    }

    public void OnHit(){
        int fullHeart = player.curHealth / 2, halfHeart = player.curHealth % 2, emptyHeart = player.maxHealth/2 - fullHeart - halfHeart;
        int heartSum = fullHeart+halfHeart+emptyHeart;
        int i = 0;

        // 하트 개수 처리
        ChangeHeartCount(heartSum - heartList.Count);

        // 하트 스프라이트, 색상 변경
        while(i < fullHeart+halfHeart+emptyHeart){
            if(i < fullHeart) heartList[i].image.sprite = heartSprite[0];
            else if(i >= fullHeart && i < fullHeart+halfHeart) heartList[i].image.sprite = heartSprite[1];
            else heartList[i].image.sprite = heartSprite[2];
            heartList[i++].ChangeColor();
        }
    }

    void ChangeHeartCount(int toChange){
        Heart toRemove;

        if(toChange >= 0){
            for(int i = 0; i < toChange; i++){
                Heart newHeart = GameManager.inst.pool.GetUI(1).GetComponent<Heart>();
                newHeart.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
                heartList.Add(newHeart);
                newHeart.transform.SetParent(transform,false);
            }
        }else{
            for(int i = 0; i < toChange * -1; i++){
                if(heartList.Count == 0){
                    Debug.Log("잘못된 heartList 인덱스 참조");
                    return;
                }

                toRemove = heartList[heartList.Count-1];
                heartList.RemoveAt(heartList.Count-1);
                toRemove.gameObject.SetActive(false);
            }
        }
    }
    
    public void DrawMinimap(GameManager.MapData[][] map){
        for(int j = 0; j < mapSizeY; j++)
            for(int i = 0; i < mapSizeX; i++){
                GameObject newRoom = Instantiate(room,transform);
                Image roomImg = newRoom.GetComponent<Image>();

                if(i == curPosX && j == curPosY) roomImg.color = Color.green;
                else if(map[i][j].isCleared == true) roomImg.color = new Color(0.4f,0.4f,0.4f);
                else if(map[i][j].map == true) roomImg.color = new Color(0.1f,0.1f,0.1f);
                else roomImg.color = new Color(0.65f,0.65f,0.65f);
            }
    }
}
