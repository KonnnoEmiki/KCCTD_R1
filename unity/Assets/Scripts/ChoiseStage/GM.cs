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

    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
            if(NetworkGUI.roommaster == true)
                if (monobitView.isMine == true)
                {
                    Destroy(gameObject);
                }
    }

}