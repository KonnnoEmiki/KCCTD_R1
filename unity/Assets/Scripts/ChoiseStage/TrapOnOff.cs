using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapOnOff : MonobitEngine.MonoBehaviour
{

    [SerializeField]
    private GameObject Trap = null;
    [SerializeField]
    private GameObject flag = null;

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
            Trap.gameObject.SetActive(true);
            flag.gameObject.SetActive(true);
            NetworkGUI.Trapflag = true;
        }
        else if (NetworkGUI.Trapflag == true)
        {
            Trap.gameObject.SetActive(false);
            flag.gameObject.SetActive(false);
            NetworkGUI.Trapflag = false;
        }
    }
}