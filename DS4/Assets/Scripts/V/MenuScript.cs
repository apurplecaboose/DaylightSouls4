using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public int GameSceneIndex,TutorialSceneIndex,NextPageIndex,MenuIndex ;
    public void StartGame()
    {
        SceneManager.LoadScene(GameSceneIndex);
    }
    public void StatTutorial()
    {
        SceneManager.LoadScene(0);
    }
    public void NextPage()
    {
        SceneManager.LoadScene(0);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
