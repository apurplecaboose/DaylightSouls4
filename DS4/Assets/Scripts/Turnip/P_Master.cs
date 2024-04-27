using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class P_Master : MonoBehaviour
{
    public enum P_Action_List
    {
        NULL_ACTION_STATE,
        //Turnip: cant move or limited movement in states below:
        SwapDodge,
        LightAttack,
        TapHeavy,
        ChargingUpForHeavy,
        HeldHeavy,
        Healing,
        //Stunned,
    }
    public P_Action_List P_Action;

    Vector2 _moveVector;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        switch (P_Action)
        {
            case P_Action_List.NULL_ACTION_STATE:
                break;
            case P_Action_List.SwapDodge:
                //increment frames add Iframes secion swap player
                break;
            case P_Action_List.LightAttack:
                break;
            case P_Action_List.TapHeavy:
                break;
            case P_Action_List.ChargingUpForHeavy:
                // if held for too long change state to held heavy (also set the time held varible to max as input)
                break;
            case P_Action_List.Healing:
                //hold
                break;
        }
    }
    void P_Move(InputAction.CallbackContext input)
    {
        _moveVector = input.ReadValue<Vector2>(); //Turnip: write input to vector which will add force in the fixed update loop
    }
    void SwapDodgeInput(InputAction.CallbackContext inputState)
    {
        if(inputState.performed)
        {
            if(P_Action == P_Action_List.NULL_ACTION_STATE)
            {
                P_Action = P_Action_List.SwapDodge;
            }
            else
            {
                Debug.Log("Busy... currently executing another action");
                return;
            }
        }
    }
}
