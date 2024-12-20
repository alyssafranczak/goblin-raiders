using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Image gameStateScreen;
    public Text gameStateText;
    public Text pointGoalText;
    public static bool isGameOver = false;

    public AudioClip gameOverSFX;
    public AudioClip gameWonSFX;

    public int pointGoal;
    public string nextLevel;

    int points;

    void Start()
    {
        isGameOver = false;
        points = 0;
        pointGoalText.text = points.ToString() + "/" + pointGoal.ToString();
    }

    void Update()
    {

        if (!isGameOver)
        {
            if (points >= pointGoal)
            {
                LevelBeat();
            }
        }
    }

    public void LevelLost()
    {
        isGameOver = true;
        gameStateText.text = "You Lost!";
        gameStateScreen.gameObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().volume = 0f;
        AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position);

        Invoke("LoadCurrentLevel", 4);

    }

    public void LevelBeat()
    {
        isGameOver = true;
        gameStateText.text = "You Win!";
        gameStateScreen.gameObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().volume = 0f;
        AudioSource.PlayClipAtPoint(gameWonSFX, Camera.main.transform.position);

        if (nextLevel.Equals("None"))
        {
            gameStateText.text = "You Win! Thanks for Playing!";
        }
        else if (!string.IsNullOrEmpty(nextLevel))
        {
            Invoke("LoadNextLevel", 5);
        }
    }

    public void incrementPoints(int value)
    {
        points += value;
        pointGoalText.text = points.ToString() + "/" + pointGoal.ToString();
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
