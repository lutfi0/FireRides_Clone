using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Transform player;
    public Text scoreText;
    public Text highscoreText;
    public int score = 0;
    public int highScore = 0;

    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            }

            return instance;
        }
    }

    private void OnEnable()
    {
        instance = this;
    }

    public void Start()
    {
        highScore = PlayerPrefs.GetInt("highscoreText", 0);
    }

    public void Update()
    {
        scoreText.text = score.ToString();
        highscoreText.text = highScore.ToString();
    }

    void OnDisable()
    {

        if (score > highScore)
        {
            PlayerPrefs.SetInt("highscoreText", score);
            PlayerPrefs.Save();

        }
    }

}