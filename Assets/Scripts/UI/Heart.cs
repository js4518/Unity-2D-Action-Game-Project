using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Heart : MonoBehaviour
{
    public Image image;
    RectTransform rect;

    float changeTime = 0.2f;
    float curTime = 0f;

    Quaternion targetRotation = Quaternion.Euler(new Vector3(0f,0f,10f));

    void Awake(){
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void ChangeColor(){
        StartCoroutine(ChangeColorCR());
    }

    public IEnumerator ChangeColorCR(){
        curTime = 0f;
        image.color = new Color(0.64f,0.13f,0.13f);

        while(curTime < changeTime/2){
            curTime += Time.deltaTime;
            rect.rotation = Quaternion.Lerp(Quaternion.identity,targetRotation,curTime / changeTime * 2);
            yield return null;
        }

        while(curTime < changeTime){
            curTime += Time.deltaTime;
            rect.rotation = Quaternion.Lerp(targetRotation,Quaternion.identity,curTime / changeTime * 2 - 1);
            yield return null;
        }
        image.color = Color.white;
    }
}
