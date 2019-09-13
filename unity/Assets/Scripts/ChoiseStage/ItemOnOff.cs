using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnOff : MonobitEngine.MonoBehaviour
{

    [SerializeField]
    private GameObject Item = null;

    [MunRPC]
    void Update()
    {
        var roomData = MonobitEngine.MonobitNetwork.room;
        if (NetworkGUI.stageselect == 0)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("Itemonoff", MonobitEngine.MonobitTargets.All, null);
    }

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
            Item.gameObject.SetActive(true);
        else if (NetworkGUI.Itemflag == true)
            Item.gameObject.SetActive(false);
    }
}