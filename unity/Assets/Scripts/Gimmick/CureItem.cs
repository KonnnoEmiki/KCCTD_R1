using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class CureItem : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private GameObject cureeffect = null;

    private static bool flag;

    private void Start()
    {
        flag = false;
    }

    // トリガーとの接触時に呼ばれるコールバック
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player") && NetworkGUI.gs == true)
        {
            cureeffect.gameObject.SetActive(true);
            Player.LifeCount = Player.LifeCount + 3;
            ScoreCounter.scoreflag = 6;

           // 
            Delay();
        }
    }

    private void Update()
    {
        if (flag == true)
        {
            cureeffect.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    static async void Delay()
    {
        await Task.Delay(40);
        flag = true;
    }
}