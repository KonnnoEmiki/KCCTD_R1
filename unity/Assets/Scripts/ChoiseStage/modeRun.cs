using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class modeRun : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private GameObject BallLuncher = null;
    public Text ModeLabel;

    [MunRPC]
    void Update()
    {
        var roomData = MonobitEngine.MonobitNetwork.room;
        if (NetworkGUI.gamemode == 0)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("BallLuncherOn", MonobitEngine.MonobitTargets.All, null);
    }

    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("master"))
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("BallLuncherOn", MonobitEngine.MonobitTargets.All, null);
        }
    }

    [MunRPC]
    private void BallLuncherOn()
    {
        NetworkGUI.gamemode = 0;
        BallLuncher.gameObject.SetActive(true);
        ModeLabel.text = "BallLuncher On";
    }
}