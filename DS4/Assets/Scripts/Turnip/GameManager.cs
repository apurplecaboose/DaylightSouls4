using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool TryndamereMode;
    public enum G_State { Playing, Selecting };
    public G_State PlayState;
    public GameObject ComboSelectionUI_Instance;
    void Awake()
    {

    }
    void Start()
    {

    }
    void Update()
    {
        if(ComboSelectionUI_Instance == null) PlayState = G_State.Playing;
        else PlayState = G_State.Selecting;
    }
    void FixedUpdate()
    {

    }
}