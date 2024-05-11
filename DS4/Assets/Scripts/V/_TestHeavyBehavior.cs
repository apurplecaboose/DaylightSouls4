using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class _TestHeavyBehavior : MonoBehaviour
{

    public enum P_Action_List
    {
        NULL_ACTION_STATE,
        //Turnip: cant move or limited movement in states below:
        SwapDodge,
        LightAttack,
        NeutralHeavy,//pressed heavy
        ChargingUpForHeavy,
        ChargedHeavy,
        Healing,
        //Stunned_S,//__ frame stun
        //Stunned_M,// __ frame stun
        //Stunned_L,//__ frame stun
        SelectingBossAttackState
    }
    [SerializeField] TextMeshPro ParryStateText;//Turnip:un-serialize when debug done
    [SerializeField] int _tickCount; //Turnip:un-serialize when debug done

    public P_Action_List P_Action;
    [SerializeField] float _P_MoveSpeed = 15f, _Ghost_MoveSpeed = 15f;
    Vector2 _P_moveVec, _Ghost_moveVec;
    [SerializeField] Rigidbody2D _P_rb;//Turnip:un-serialize when debug done

    bool chargeCancelled;

    void FixedUpdate()
    {
        switch (P_Action)
        {
            case P_Action_List.NULL_ACTION_STATE:
                _P_rb.AddForce(_P_moveVec * _P_MoveSpeed * 10f); // move player

                _tickCount = 0;
                break;

            case P_Action_List.NeutralHeavy:
                //UNINTERUPTABLE
                NeutralHeavyAttackAction();
                break;
            case P_Action_List.ChargingUpForHeavy:
                // if held for too long change state to held heavy (also set the time held varible to max as input)
                break;
            case P_Action_List.ChargedHeavy:
                ChargedHeavyAttackAction();
                break;

        }

    }
    public void NeutralHeavyAttackInput(InputAction.CallbackContext inputState)
    {
        if (inputState.performed)
        {
            if (P_Action == P_Action_List.NULL_ACTION_STATE)
            {
                P_Action = P_Action_List.NeutralHeavy;
                Debug.Log("NeutralHeavy");
            }
            else if(P_Action == P_Action_List.ChargingUpForHeavy)
            {
                P_Action = P_Action_List.NeutralHeavy;
                Debug.Log("NeutralHeavy");
            }
            else
            {
                Debug.Log("Busy!");
                return;
            }
        }
    }
    public void HoldHeavyAttackInput(InputAction.CallbackContext inputState)
    {
        if (P_Action == P_Action_List.NULL_ACTION_STATE)
        {
            if (inputState.started)
            {
                P_Action = P_Action_List.ChargingUpForHeavy;
            }

        }
        if (P_Action == P_Action_List.ChargingUpForHeavy)
        {
            if (inputState.canceled)
            {
                P_Action = P_Action_List.NeutralHeavy;
                chargeCancelled = inputState.canceled;
            }
            if (inputState.performed)
            {
                P_Action = P_Action_List.ChargedHeavy;
                Debug.Log("hold");
            }
        }
    }
    void LightAttackAction()
    {
        //swap player UNINTERUPTABLE
        int startUpFrames = 2;
        int activeFrames = 13;
        int recoveryFrames = 10;
        int totalTicks = startUpFrames + activeFrames + recoveryFrames;

        if (_tickCount >= totalTicks)
        { //Turnip: done ticking reset back to null action state and set tickcount back to 0 for next action
            P_Action = P_Action_List.NULL_ACTION_STATE;
            _tickCount = 0;
            return;
        }
        else
        { // Turnip: run logic 
            switch (_tickCount)
            {
                case int t when t < startUpFrames://button pressed
                    break;
                case int t when t >= startUpFrames && t < startUpFrames + activeFrames://play swing animation&check if damage dealt
                    break;
                case int t when t >= startUpFrames + activeFrames && t < totalTicks://play recover animation
                    break;
                case int t when t >= totalTicks:
                    P_Action = P_Action_List.NULL_ACTION_STATE;// catch case
                    _tickCount = 0;
                    break;
            }
            _tickCount += 1;
        }
    }
    void ChargingHeavyAttackAction()//play charge animation, not able to move, disruptable by swap
    {
        if (chargeCancelled)//catch case?
        {
            P_Action = P_Action_List.NeutralHeavy;
        }


    }
    void NeutralHeavyAttackAction()
    {
        //swap player UNINTERUPTABLE
        int startUpFrames = 6;


        int activeFrames = 5;

        int recoveryFrames = 10;
        int totalTicks = startUpFrames + activeFrames + recoveryFrames;

        if (_tickCount >= totalTicks)
        { //Turnip: done ticking reset back to null action state and set tickcount back to 0 for next action
            P_Action = P_Action_List.NULL_ACTION_STATE;
            _tickCount = 0;
            return;
        }
        else
        { // Turnip: run logic 
            switch (_tickCount)
            {
                case int t when t < startUpFrames://startup phase
                    break;
                case int t when t >= startUpFrames && t < startUpFrames + activeFrames://play swing animation&check if damage dealt 
                    break;
                case int t when t >= startUpFrames + activeFrames && t < totalTicks://play recover animation
                    break;
                case int t when t >= totalTicks:
                    P_Action = P_Action_List.NULL_ACTION_STATE;// catch case
                    _tickCount = 0;
                    break;
            }
            _tickCount += 1;
        }
    }
    void ChargedHeavyAttackAction()
    {
        //swap player UNINTERUPTABLE
        int startUpFrames = 5;


        int activeFrames = 5;

        int recoveryFrames = 10;
        int totalTicks = startUpFrames + activeFrames + recoveryFrames;

        if (_tickCount >= totalTicks)
        { //Turnip: done ticking reset back to null action state and set tickcount back to 0 for next action
            P_Action = P_Action_List.NULL_ACTION_STATE;
            _tickCount = 0;
            return;
        }
        else
        { // Turnip: run logic 
            switch (_tickCount)
            {
                case int t when t < startUpFrames://startup phase
                    break;
                case int t when t >= startUpFrames && t < startUpFrames + activeFrames://play swing animation&check if damage dealt 
                    break;
                case int t when t >= startUpFrames + activeFrames && t < totalTicks://play recover animation
                    break;
                case int t when t >= totalTicks:
                    P_Action = P_Action_List.NULL_ACTION_STATE;// catch case
                    _tickCount = 0;
                    break;
            }
            _tickCount += 1;
        }
    }

}
