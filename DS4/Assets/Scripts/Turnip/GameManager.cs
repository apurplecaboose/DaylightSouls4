using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        
    }
    void Start()
    {
        
    }
    void Update()
    {
        TESTStartKenPanel();
    }
    void FixedUpdate()
    {
        
    }
    void TESTStartKenPanel()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            UIManager.Instance.ShowPanel<SelectPatternPanel>();
        }
    }
}
