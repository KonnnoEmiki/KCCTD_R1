using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    public GameObject Menu;
    public static bool MenuOn = true;
    private bool flag = false;

    void Start()
    {
        
    }

    void Update()
    {
        // Eキー押しっぱなしでも連続で切り替わらないように
        if (Input.GetKey(KeyCode.E) && flag == true)
            return;

        flag = false;

        // メニュー非表示
        if (Input.GetKey(KeyCode.E) && MenuOn == true)
        {
            Menu.SetActive(false);
            MenuOn = false;
            flag = true;
            return;
        }
        // メニュー表示
        if (Input.GetKey(KeyCode.E) && MenuOn == false)
        {
            Menu.SetActive(true);
            MenuOn = true;
            flag = true;
            return;
        }
    }

}
