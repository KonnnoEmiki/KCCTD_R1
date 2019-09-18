using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverScoreItem : MonobitEngine.MonoBehaviour
{
    // トリガーとの接触時に呼ばれるコールバック
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player") && NetworkGUI.gs == true)
        {
            if (monobitView.isMine == true)
                ScoreCounter.scoreflag = 5;
            // このコンポーネントを持つGameObjectを破棄する
            Destroy(gameObject);
        }
    }
}
