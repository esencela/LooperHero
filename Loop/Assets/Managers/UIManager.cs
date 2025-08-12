using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI deathText;

    public GameObject playPanel;
    public GameObject gameLossPanel;
    public GameObject gameWinPanel;

    public void UpdateTimer()
    {
        float timeLeft = GameManager.instance.levelTime - Time.timeSinceLevelLoad;
        int minutes = Mathf.Clamp(Mathf.FloorToInt(timeLeft / 60), 0, 1000);
        int seconds = Mathf.Clamp(Mathf.FloorToInt(timeLeft % 60), 0, 1000);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateDeathText()
    {
        int deaths = GameManager.instance.citizenDeaths;

        if (deaths == 1)
        {
            deathText.text = deaths + " citizen died";
            return;
        }

        deathText.text = deaths + " citizens died";
    }

    public void ShowGameLossPanel()
    {
        playPanel.SetActive(false);
        gameLossPanel.SetActive(true);
    }

    public void ShowGameWinPanel()
    {
        playPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }
}
