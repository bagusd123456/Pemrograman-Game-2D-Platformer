﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text txScore;
    public Text txHighScore;
    Text txSelamat;
    int highscore;

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("HS", 0);
        if(Data.score > highscore)
        {
            highscore = Data.score;
            PlayerPrefs.SetInt("HS", highscore);
        }
        else if (EnemyController.enemyKilled == 3)
        {
            SceneManager.LoadScene("Congratulations");
        }
        txHighScore.text = "Highscores: " + highscore;
        txScore.text = "Scores: " + Data.score;
    }

    public void Replay()
    {
        Data.score = 0;
        EnemyController.enemyKilled = 0;
        SceneManager.LoadScene("Level 1");
    }

// Update is called once per frame
void Update()
    {
        
    }
}
