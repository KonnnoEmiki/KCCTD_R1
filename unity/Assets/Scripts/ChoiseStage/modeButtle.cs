using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modeButtle : MonobitEngine.MonoBehaviour
{
    public static bool Flag = false;
    [SerializeField]
    private GameObject BallLuncher = null;

    // トリガーとの接触時に呼ばれるコールバック
    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("master"))
            if (RoomManager.IsHost == true)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                {
                    monobitView.RPC("Triggerchange", MonobitEngine.MonobitTargets.All, null);
                }
    }
    [MunRPC]
    void Triggerchange(Collider hit)
    {
        var obj = transform.Find("PlaneSupply").gameObject;
        var obj1 = transform.Find("PlaneSupply/Supply/cartridge").gameObject;
        NetworkGUI.gamemode = 2;
        if (!Flag)
        {
            Flag = true;
            obj.SetActive(true);
            obj1.SetActive(true);
            SupplyBrain.flag = true;
        }
        else
        {
            Flag = false;
            obj.SetActive(false);
        }
    }

}