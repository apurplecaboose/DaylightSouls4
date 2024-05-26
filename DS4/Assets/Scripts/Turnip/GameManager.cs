using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject ComboSelectionUI_Instance;
    void Awake()
    {

    }
    void Start()
    {

    }
    void Update()
    {
        TESTStartEdwardsPanel();
    }
    void FixedUpdate()
    {

    }
    void TESTStartEdwardsPanel()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            //UIManager.Instance.ShowPanel<SelectPatternPanel>();
            Instantiate(ComboSelectionUI_Instance);
        }
    }
}