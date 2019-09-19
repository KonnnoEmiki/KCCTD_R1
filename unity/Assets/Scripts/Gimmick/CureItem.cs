using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureItem : MonobitEngine.MonoBehaviour
{
    // トリガーとの接触時に呼ばれるコールバック
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player") && NetworkGUI.gs == true)
        {
                Player.LifeCount = Player.LifeCount + 3;
                ScoreCounter.scoreflag = 6;
            // このコンポーネントを持つGameObjectを破棄する
            Destroy(gameObject);
        }
    }
}