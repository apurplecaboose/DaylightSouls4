using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class JohnnyLogo : MonoBehaviour
{
    public bool WebGL;
    public int TargetSceneInt;
    VideoPlayer video;
    void Awake()
    {
        if(WebGL)
        {
            if (TargetSceneInt == 0) Invoke("LoadSecondScene", 2);
            else Invoke("LoadMenuScene", 3);
        }
        else
        {
            video = GetComponent<VideoPlayer>();
            video.Play();
            video.loopPointReached += CheckOver;
        }
    }
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(TargetSceneInt);
    }
    void LoadSecondScene()
    {
        SceneManager.LoadScene(10);
    }
    void LoadMenuScene()
    {
        SceneManager.LoadScene(1);
    }
}
