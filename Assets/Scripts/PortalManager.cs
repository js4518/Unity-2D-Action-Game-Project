using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public Portal left, right, up, down;

    public void Init(){
        left.Init();
        right.Init();
        up.Init();
        down.Init();
    }
}
