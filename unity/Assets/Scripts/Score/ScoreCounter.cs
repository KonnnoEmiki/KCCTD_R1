using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public GameObject scoreLabel = null;
    public static int scoreflag = 0;
    private int startgame = 0;
    private int Timescore = 0;
    private int gstime = 0;
    private int score = 0;
    private int result;

    private void Start()
    {
        scoreflag = 0;
        gstime = 0;
        score = 0;
        startgame = 0;
        Timescore = 0;
        result = 0;
        Text score_text = scoreLabel.GetComponent<Text>();
        score_text.text = "HighScore:" + Player.HighScore;
    }

    void Update()
    {
        if (NetworkGUI.gs == true)
        {
            if (startgame == 0)
            {
                startgame = 1;
                gstime = (int)Time.time;
            }
            if (scoreflag == 1)
            {
                scoreflag = 0;
                score = score + 10;
            }
            if (scoreflag == 2)
            {
                scoreflag = 0;
                score = score + 5000;
            }
            if (scoreflag == 3)
            {
                scoreflag = 0;
                score = score + 300;
            }
            if (scoreflag == 4)
            {
                scoreflag = 0;
                score = score + 1000;
            }
            if (scoreflag == 5)
            {
                scoreflag = 0;
                score = score + 300;
            }
            if (scoreflag == 6)
            {
                scoreflag = 0;
                score = score + 100;
            }
            if (Player.sibouflag == true)
            {
                if (result > Player.HighScore)
                    Player.HighScore = result;
                return;
            }
            if (GameManager.syuumaku == true)
            {
                if (result > Player.HighScore)
                    Player.HighScore = result;
                return;
            }
            Text score_text = scoreLabel.GetComponent<Text>();
            Timescore = (int)Time.time - gstime;
            result = 10 * Timescore + score;
            score_text.text = "score:" + result;
        }
    }
}
