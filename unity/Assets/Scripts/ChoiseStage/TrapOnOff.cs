using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapOnOff : MonobitEngine.MonoBehaviour
{

    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("master"))
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("Traponoff", MonobitEngine.MonobitTargets.All, null);
        }
    }

    [MunRPC]
    private void Traponoff()
    {
        if (NetworkGUI.Trapflag == false)
        {
            NetworkGUI.Trapflag = true;
        }
        else if (NetworkGUI.Trapflag == true)
        {
            NetworkGUI.Trapflag = false;
        }
    }
}