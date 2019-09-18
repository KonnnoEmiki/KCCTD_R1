using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSOnOff : MonobitEngine.MonoBehaviour
{

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
                    monobitView.RPC("TPSonoff", MonobitEngine.MonobitTargets.All, null);
        }
    }

    [MunRPC]
    private void TPSonoff()
    {
        if (NetworkGUI.TPSflag == false)
        {
            GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("Supply");
            foreach (GameObject obj in tagobjs)
            {
                obj.gameObject.SetActive(true);
            }
            flag.gameObject.SetActive(true);
            NetworkGUI.TPSflag = true;
            SupplyBrain.flag = true;
            SupplyBrain.Time = 0;
            PlayerController.shotCount = 6;
        }
        else if (NetworkGUI.TPSflag == true)
        {
            GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("Supply");
            foreach (GameObject obj in tagobjs)
            {
                obj.gameObject.SetActive(false);
            }
            flag.gameObject.SetActive(false);
            NetworkGUI.TPSflag = false;
        }
    }
}