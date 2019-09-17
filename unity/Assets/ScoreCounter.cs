using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public GameObject scoreLabel = null;
    public static int score = 0;
    void Update()
    {
        Text score_text = scoreLabel.GetComponent<Text>();
        score_text.text = "score:" + score;
    }
}
