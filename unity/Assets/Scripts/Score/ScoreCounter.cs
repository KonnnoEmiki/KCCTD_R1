using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class ScoreCounter : MonoBehaviour
{
    public GameObject scoreLabel = null;
    public static int scoreflag = 0;
    private int startgame = 0;
    private int Timescore = 0;
    private int gstime = 0;
    private int score = 0;
    private int result;
    public string[] HS;
    private bool save = false;

    private void Start()
    {
        scoreflag = 0;
        gstime = 0;
        score = 0;
        startgame = 0;
        Timescore = 0;
        result = 0;
        TextAsset HiScore = new TextAsset();
        Text score_text = scoreLabel.GetComponent<Text>();
        HiScore = Resources.Load("ScoreLog", typeof(TextAsset)) as TextAsset;
        
        string TextLines = HiScore.text; //テキスト全体をstring型で入れる変数を用意して入れる
        //Splitで一行づつを代入した1次配列を作成
        HS = TextLines.Split('\n');
        score_text.text = "HighScore:" + HS[0]+" "+HS[1]+"PT";
        Int32.TryParse(HS[1], out int y);
        Player.HighScore = y;
    }

    public void savescore(string txt, string txt2)
    {
        {
            save = false;
            StreamWriter sw = new StreamWriter("./Assets/Resources/ScoreLog.txt", false); //true=追記 false=上書き
            sw.WriteLine(txt + "\n" + txt2);
            sw.Flush();
            sw.Close();
        }
    }

    void Update()
    {
        if (result > Player.HighScore)
        {
            savescore(NetworkGUI.PlayerName, (result.ToString()));
        }
        if (NetworkGUI.gs == true)
        {
            if (startgame == 0)
            {
                save = true;
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
                {
                    savescore(NetworkGUI.PlayerName, (result.ToString()));
                    Player.HighScore = result;
                }
                return;
            }
            if (GameManager.syuumaku == true)
            {
                if (result > Player.HighScore)
                {
                    savescore(NetworkGUI.PlayerName, (result.ToString()));
                    Player.HighScore = result;
                }
                return;
            }
            Text score_text = scoreLabel.GetComponent<Text>();
            Timescore = (int)Time.time - gstime;
            result = 10 * Timescore + score;
            score_text.text = "score:" + result;
        }
    }
}
