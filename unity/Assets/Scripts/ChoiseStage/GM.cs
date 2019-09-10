using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public static int stageselect = 1;
    public static int gamemode = 0;

    [SerializeField]
    private GameObject Select = null;

    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
            if (RoomManager.IsHost == true)
                Select.gameObject.SetActive(false);
    }
    

}