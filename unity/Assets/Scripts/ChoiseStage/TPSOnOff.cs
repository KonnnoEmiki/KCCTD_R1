using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSOnOff : MonobitEngine.MonoBehaviour
{

    [SerializeField]
    private GameObject TPS = null;

    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("master"))
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("TPSonoff", MonobitEngine.MonobitTargets.All, null);
        }
    }

    [MunRPC]
    private void TPSonoff()
    {
        if (NetworkGUI.TPSflag == false)
        {
            TPS.gameObject.SetActive(true);
            NetworkGUI.TPSflag = true;
            SupplyBrain.flag = true;
            SupplyBrain.Time = 0;
            PlayerController.shotCount = 6;
        }
        else if (NetworkGUI.TPSflag == true)
        {
            TPS.gameObject.SetActive(false);
            NetworkGUI.TPSflag = false;
        }
    }
}