using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    CinemachineCamera cineCam;
    Camera cam;
    public CinemachinePositionComposer cineComp;

    void Awake(){
        cineCam = GetComponentInChildren<CinemachineCamera>();
        cam = GetComponentInChildren<Camera>();
        cineComp = GetComponentInChildren<CinemachinePositionComposer>();
    }

    void Start()
    {
        cineCam.Follow = GameManager.inst.player.transform;
    }

    void LateUpdate(){
        // Vector3 desiredPos = cineCam.transform.position;

        // float clampX = Mathf.Clamp(desiredPos.x,minX+cam.orthographicSize*cam.aspect,maxX-cam.orthographicSize*cam.aspect);
        // float clampY = Mathf.Clamp(desiredPos.y,minY+cam.orthographicSize,maxY-cam.orthographicSize);

        //cineComp.TargetOffset = new Vector3(clampX-desiredPos.x,clampY-desiredPos.y,cineComp.TargetOffset.z);
    }
}
