using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void StartGame()
    {
        int scenenumb = 1;
        SceneManager.LoadScene(scenenumb);
    }
    public void StartTutorial()
    {
        int scenenumb = 1;
        SceneManager.LoadScene(scenenumb);
    }
    public void NextPage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PreviousPage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
    public void BackToMenu()
    {
        int scenenumb = 1;
        SceneManager.LoadScene(scenenumb);
    }
}
