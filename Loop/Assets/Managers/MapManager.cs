using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public void LoadMenuScene()
    {
        StartCoroutine(LoadAsyncScene("MenuScene"));
    }

    public void LoadPlayScene()
    {
        StartCoroutine(LoadAsyncScene("PlayScene"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadAsyncScene(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
