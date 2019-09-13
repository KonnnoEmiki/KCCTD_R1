using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOnOff : MonobitEngine.MonoBehaviour
{

    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("master"))
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("Ballonoff", MonobitEngine.MonobitTargets.All, null);
        }
    }

    [MunRPC]
    private void Ballonoff()
    {
        if(NetworkGUI.Ballflag==false)
        NetworkGUI.Ballflag = true;
        else if (NetworkGUI.Ballflag == true)
            NetworkGUI.Ballflag = false;
    }
}