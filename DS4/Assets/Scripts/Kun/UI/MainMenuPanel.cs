using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    public Button btnStart;
    public Button btnQuit;
    public override void Init()
    {
        btnStart.onClick.AddListener(() =>
        {
            //隐藏自己面板
            //Hide myself
            UIManager.Instance.HidePanel<MainMenuPanel>();
            //打开游戏面板
            //todo: Open the game panel
            UIManager.Instance.ShowPanel<SelectPatternPanel>();

        });

        btnQuit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
