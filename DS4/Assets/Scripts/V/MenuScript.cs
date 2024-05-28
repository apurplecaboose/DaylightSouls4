using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public int CurrentScene;
    public void StartGame()
    {
        int scenenumb = 9;
        SceneManager.LoadScene(scenenumb);
    }
    public void StartTutorial()
    {
        int scenenumb = 2;
        SceneManager.LoadScene(scenenumb);
    }
    public void NextPage()
    {
        SceneManager.LoadScene(CurrentScene + 1);
    }
    public void PreviousPage()
    {
        SceneManager.LoadScene(CurrentScene - 1);
    }
    public void BackToMenu()
    {
        int scenenumb = 1;
        SceneManager.LoadScene(scenenumb);
    }
}
