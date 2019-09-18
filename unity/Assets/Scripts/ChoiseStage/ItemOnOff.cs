using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnOff : MonobitEngine.MonoBehaviour
{

    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("master"))
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("Itemonoff", MonobitEngine.MonobitTargets.All, null);
        }
    }

    [MunRPC]
    private void Itemonoff()
    {
        if (NetworkGUI.Itemflag == false)
        {
            NetworkGUI.Itemflag = true;
        }
        else if (NetworkGUI.Itemflag == true)
        {
            NetworkGUI.Itemflag = false;
        }
    }
}