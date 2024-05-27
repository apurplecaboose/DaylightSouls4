using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [Header("Required Fields MUST FILL!!!")]
    [Header("set index number according to build settings and script will jump to the desired scene")]
    public int GameSceneIndex,TutorialSceneStartIndex,MenuIndex;
    int _TutorialCurrentPageIndex,_NextPageIndex;

    public void StartGame()
    {
        SceneManager.LoadScene(GameSceneIndex);
    }
    public void StartTutorial()
    {
        SceneManager.LoadScene(TutorialSceneStartIndex);
    }
    public void NextPage()
    {
        SceneManager.LoadScene(_NextPageIndex);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(MenuIndex);
    }
    private void Awake()
    {
        _TutorialCurrentPageIndex = TutorialSceneStartIndex;
    }
    void Update()
    {
        if (_NextPageIndex! > 10)
        {
          _NextPageIndex = _TutorialCurrentPageIndex+1;
        }
        if (_NextPageIndex >= 10)
        {
            BackToMenu();
        }

    }
}
