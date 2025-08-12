using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance { get { return _instance; } }

    public float levelTime;

    public UIManager uiManager;

    public LockAndHideCursor cursor;

    public int citizenDeaths = 0;

    bool gameFinished = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad > levelTime)
        {
            FinishGame();
            return;
        }

        uiManager.UpdateTimer();
    }

    void FinishGame()
    {
        if (gameFinished)
            return;

        if (citizenDeaths > 0)
        {
            GameLoss();
        }
        else
        {
            GameWin();
        }        
    }

    void GameWin()
    {
        gameFinished = true;

        cursor.ShowCursor();
        uiManager.ShowGameWinPanel();
    }

    void GameLoss()
    {
        gameFinished = true;

        cursor.ShowCursor();
        uiManager.ShowGameLossPanel();
    }

    public void PlayerDeath()
    {
        GameLoss();
    }

    public void CitizenDeath()
    {
        citizenDeaths++;
        uiManager.UpdateDeathText();
    }
}
