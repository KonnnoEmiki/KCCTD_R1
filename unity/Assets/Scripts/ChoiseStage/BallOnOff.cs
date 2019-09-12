using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOnOff : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private GameObject BallLuncher = null;
    
    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("master"))
            if (RoomManager.IsHost == true && NetworkGUI.roommaster == true)
                if (monobitView.isMine == true)
                    monobitView.RPC("Ballonoff", MonobitEngine.MonobitTargets.All, null);

    }

    [MunRPC]
    private void Ballonoff()
    {
        if (NetworkGUI.Ballmode == true)
            NetworkGUI.Ballmode = false;
        else if (NetworkGUI.Ballmode == false)
                NetworkGUI.Ballmode = true;
    }
}