using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Experimental.Rendering;

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

    [SerializeField] int _tickCount, HeavyChargeTimer, chargeBonus, holdpoint = 20, maximumHoldPoint = 100; //Turnip:un-serialize when debug done

    public P_Action_List P_Action;
    [SerializeField] float _P_MoveSpeed = 15f, _Ghost_MoveSpeed = 15f;
    Vector2 _P_moveVec, _Ghost_moveVec;
    
    [SerializeField] Rigidbody2D _P_rb;//Turnip:un-serialize when debug done


    //References for debug only below

    public enum ParryStateDebug
    {
        startup,
        parry,
        recovery
    }
    public ParryStateDebug ParryState;
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
                ChargingUpHeavyAction();
                // if held for too long change state to held heavy (also set the time held varible to max as input)
                break;
            case P_Action_List.ChargedHeavy:
                ChargedHeavyAttackAction(chargeBonus);
                break;

        }

        if (P_Action == P_Action_List.ChargingUpForHeavy)
        {
            HeavyChargeTimer += 1;
        }
        else if (P_Action == P_Action_List.NULL_ACTION_STATE)
        {
            HeavyChargeTimer = 0;
           
        }


    }
    public void HeavyAttackInput(InputAction.CallbackContext inputState)
    {
        



        /// if(preformed)
        /// sometimer += sometime either use delta time or fixed update ints
        ///     if(sometimer >= holdpoint)
        ///        state = chargingupforheavy
        ///        
        /// if(canceled)
        ///     if(state != chargingupforheavy)
        ///         state = NeutralHeavy
        ///     else //you were charging
        ///         state = chargedheavy 
        ///         give the charge time in a varible here
        ///         

        if (inputState.performed)
        {
            P_Action = P_Action_List.ChargingUpForHeavy;

            //if charging is true add charge timmer
        }
        if (P_Action == P_Action_List.ChargingUpForHeavy)
        {
           
            if (inputState.canceled)
            {
                if (HeavyChargeTimer >= holdpoint)
                {
                    P_Action = P_Action_List.ChargedHeavy;
                }
                else if (HeavyChargeTimer < holdpoint )
                {
                    P_Action = P_Action_List.NeutralHeavy;
                }

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
    void ChargingUpHeavyAction()//play charge animation, not able to move, disruptable by swap
    {
       
        HeavyChargeTimer += 1;
        chargeBonus = HeavyChargeTimer - holdpoint;
        if (HeavyChargeTimer >= maximumHoldPoint)
        {
            P_Action = P_Action_List.ChargedHeavy;
            //MaximumTriggeredHeavy = true;
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
                    ParryState = ParryStateDebug.startup;
                    break;
                case int t when t >= startUpFrames && t < startUpFrames + activeFrames://play swing animation&check if damage dealt 
                    break;
                case int t when t >= startUpFrames + activeFrames && t < totalTicks://play recover animation
                    break;
                case int t when t >= totalTicks:
                    P_Action = P_Action_List.NULL_ACTION_STATE;// catch case
                    _tickCount = 0;
                    HeavyChargeTimer = 0;//catch case....?

                    break;
            }
            _tickCount += 1;
        }
    }
    void ChargedHeavyAttackAction(int ChargeBonusTime)
    {
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
                    ParryState = ParryStateDebug.startup;
                    break;
                case int t when t >= startUpFrames && t < startUpFrames + activeFrames://play swing animation&check if damage dealt 
                    break;
                case int t when t >= startUpFrames + activeFrames && t < totalTicks://play recover animation
                    break;
                case int t when t >= totalTicks:
                    P_Action = P_Action_List.NULL_ACTION_STATE;// catch case
                    _tickCount = 0;
                    HeavyChargeTimer = 0;//catch case....?
                    chargeBonus = -1;
                    break;
            }
            _tickCount += 1;
        }
    }


}
