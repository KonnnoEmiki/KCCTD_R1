using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonobitEngine.MonoBehaviour
{
    public static int stageselect = 1;
    public static int gamemode = 0;

    private static readonly int BaseGUIWidth = 75;

    [SerializeField]
    private GameObject Select = null;

    private void Start()
    {
        var roomData = MonobitEngine.MonobitNetwork.room;
        string playerInfo = "(" + roomData.playerCount + "/" + ((roomData.maxPlayers == 0) ? "-" : roomData.maxPlayers.ToString()) + ")";
        GUILayout.Label("Num Players : " + roomData.name + playerInfo, GUILayout.Width(BaseGUIWidth * 3));
        monobitView.RPC("LoadInGameScene", MonobitEngine.MonobitTargets.OthersBuffered);
        roomData.visible = false; // ゲーム開始後はルームが他プレイヤーから見えないように
    }

    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
            if (RoomManager.IsHost == true)
                if (monobitView.isMine == true)
                {
                    Select.gameObject.SetActive(false);
                    Destroy(gameObject);
                    var roomData = MonobitEngine.MonobitNetwork.room;
                    string playerInfo = "(" + roomData.playerCount + "/" + ((roomData.maxPlayers == 0) ? "-" : roomData.maxPlayers.ToString()) + ")";
                    GUILayout.Label("Num Players : " + roomData.name + playerInfo, GUILayout.Width(BaseGUIWidth * 3));
                    monobitView.RPC("LoadInGameScene", MonobitEngine.MonobitTargets.OthersBuffered);
                    roomData.visible = true;
                }
    }
    

}