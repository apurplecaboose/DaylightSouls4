using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class P_Master : MonoBehaviour
{
    public enum P_Action_List
    {
        NULL_ACTION_STATE,
        //Turnip: cant move or limited movement in states below:
        SwapDodge,
        LightAttack,
        NeutralHeavy,
        ChargingUpForHeavy,
        ChargedHeavy,
        Healing,
        //Stunned_S,//__ frame stun
        //Stunned_M,// __ frame stun
        //Stunned_L,//__ frame stun
        SelectingBossAttackState
    }
    [SerializeField] bool _targetLocked;
    [SerializeField] int _tickCount; //Turnip:un-serialize when debug done
    int _heavyChargeTimer, _chargeBonusDamage;

    public P_Action_List P_Action;
    [SerializeField] float _P_MoveSpeed = 15f, _Ghost_MoveSpeed = 15f;
    Vector2 _P_moveVec, _Ghost_moveVec;
    Vector2 _P_MoveVecCache;
    [SerializeField] Rigidbody2D _P_rb, _Ghost_rb;//Turnip:un-serialize when debug done

    public bool Invincible_P;
    void Start()
    {
    }
    void Update()
    {
        if(_targetLocked) LockOn();
        else ManualAngleControl();
    }
    void LockOn()
    {
        
    }
    void ManualAngleControl()
    {

    }
    
    void FixedUpdate()
    {
        switch (P_Action)
        {
            case P_Action_List.NULL_ACTION_STATE:
                _P_rb.AddForce(_P_moveVec * _P_MoveSpeed * 10f); // move player
                _Ghost_rb.AddForce(_Ghost_moveVec * _Ghost_MoveSpeed * 10f);// move Ghost
                _tickCount = 0;
                break;
            case P_Action_List.SwapDodge:
                SwapDodgeAction();
                break;
            case P_Action_List.LightAttack:
                LightAttackAction();
                break;
            case P_Action_List.ChargingUpForHeavy:
                ChargingUpHeavyAction();
                break;
            case P_Action_List.NeutralHeavy:
                HeavyAttackAction(null);
                break;
            case P_Action_List.ChargedHeavy:
                HeavyAttackAction(true);
                break;

            case P_Action_List.Healing:
                //hold down key to heal
                HealingAction();
                break;
        }
    } 
    public void TargetLock(InputAction.CallbackContext input)
    {
        if(input.performed) _targetLocked = !_targetLocked;
    }
    /// <summary>
    /// Move Player and Ghost
    /// </summary>
    /// <param name="input"></param>
    public void P_Move(InputAction.CallbackContext input)
    {
        _P_moveVec = input.ReadValue<Vector2>(); //Turnip: writes to input to vector which will add force in the fixed update loop
    }
    public void Ghost_Move(InputAction.CallbackContext input)
    {
        _Ghost_moveVec = input.ReadValue<Vector2>(); //Turnip: same as P_move
    }
    /// <summary>
    /// Swap Position Dodge
    /// </summary>
    /// <param name="inputState"></param>
    public void SwapDodgeInput(InputAction.CallbackContext inputState)
    {
        if(inputState.performed)
        {
            Debug.Log("dodge press");
            if (P_Action == P_Action_List.NULL_ACTION_STATE)
            {
                P_Action = P_Action_List.SwapDodge;
                Debug.Log("changed state to dodge");
            }
            else
            {
                Debug.Log("Busy... currently executing another action");
                return;
            }
        }
    }
    void SwapDodgeAction()
    {
        //swap player UNINTERUPTABLE
        int startUpFrames = 1;
        int active_i_Frames = 13;
        int recoveryFrames = 8;
        int totalTicks = startUpFrames + active_i_Frames + recoveryFrames;

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
                case int t when t < startUpFrames:
                    Invincible_P = false;
                    break;
                case int t when t == startUpFrames:
                    Invincible_P = true;
                    //fire off swap event

                    Vector2 pVelCache = _P_rb.velocity;
                    Vector2 gVelCache = _P_rb.velocity;
                    _P_rb.velocity = gVelCache;
                    _Ghost_rb.velocity = pVelCache;

                    //_P_rb.velocity = Vector2.zero;
                    //_Ghost_rb.velocity = Vector2.zero;

                    Vector3 playertransformcache = _P_rb.transform.position;
                    Vector3 ghosttransformcache = _Ghost_rb.transform.position;
                    _P_rb.transform.position = ghosttransformcache;
                    _Ghost_rb.transform.position = playertransformcache;
                    break;
                case int t when t >= startUpFrames && t < startUpFrames + active_i_Frames:
                    Invincible_P = true;
                    break;
                case int t when t >= startUpFrames + active_i_Frames && t < totalTicks:
                    Invincible_P = false;
                    break;
                case int t when t >= totalTicks:
                    P_Action = P_Action_List.NULL_ACTION_STATE;// catch case
                    _tickCount = 0;
                    Invincible_P = false;
                    break;
            }
            _tickCount += 1;
        }
    }
    /// <summary>
    /// Light Attack
    /// </summary>
    /// <param name="inputState"></param>
    public void LightAttackInput(InputAction.CallbackContext inputState)
    {
        if (inputState.performed)
        {
            if (P_Action == P_Action_List.NULL_ACTION_STATE)
            {
                P_Action = P_Action_List.LightAttack;
            }
            else
            {
                Debug.Log("Busy... currently executing another action");
                return;
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
                case int t when t < startUpFrames:
                    break;
                case int t when t >= startUpFrames && t < startUpFrames + activeFrames:
                    break;
                case int t when t >= startUpFrames + activeFrames && t < totalTicks:
                    break;
                case int t when t >= totalTicks:
                    P_Action = P_Action_List.NULL_ACTION_STATE;// catch case
                    _tickCount = 0;
                    break;
            }
            _tickCount += 1;
        }
    }
    /// <summary>
    /// Healing
    /// </summary>
    /// <param name="inputState"></param>
    public void HealingInput(InputAction.CallbackContext inputState)
    {
        if(inputState.performed)
        {
            if (P_Action == P_Action_List.NULL_ACTION_STATE)
            {
                P_Action = P_Action_List.Healing;
                int recoveryFrames = 3;
                _tickCount = recoveryFrames;
            }
            else
            {
                Debug.Log("Busy... currently executing another action");
                return;
            }
        }
    }
    void HealingAction()
    {
        //UNINTERUPTABLE
        if (_tickCount <= 0)
        { //Turnip: done ticking reset back to null action state and set tickcount back to 0 for next action
            P_Action = P_Action_List.NULL_ACTION_STATE;
            _tickCount = 0;
            return;
        }
        else
        {
            if(_tickCount == 3) // same as healing input recoveryFrames
            {
                //run healing stuff here
            }
            _tickCount -= 1; 
        }
    }
    /// <summary>
    /// Heavy Attack (Charged and Neutral) Charging State is inturuptable but attacking is not
    /// </summary>
    /// <param name="inputState"></param>
    public void HeavyAttackInput(InputAction.CallbackContext inputState)
    {
        if (inputState.performed)
        {
            if (P_Action == P_Action_List.NULL_ACTION_STATE)
            {
                P_Action = P_Action_List.ChargingUpForHeavy;
            }
            else
            {
                Debug.Log("Busy... Currently in other action");
            }
        }
        if (P_Action == P_Action_List.ChargingUpForHeavy)
        {
            if (inputState.canceled)
            {
                int holdthreshold = 20;
                if (_heavyChargeTimer >= holdthreshold)
                {
                    P_Action = P_Action_List.ChargedHeavy;
                }
                else if (_heavyChargeTimer < holdthreshold)
                {
                    P_Action = P_Action_List.NeutralHeavy;
                }
            }
        }
    }
    void ChargingUpHeavyAction()//play charge animation, not able to move, disruptable by swap
    {
        int maxChargeTime = 100;
        int holdthreshold = 20;
        _heavyChargeTimer += 1;
        _chargeBonusDamage = _heavyChargeTimer - holdthreshold;
        _chargeBonusDamage = Mathf.Clamp(_chargeBonusDamage, 0, maxChargeTime);
        if (_heavyChargeTimer >= maxChargeTime)
        {
            P_Action = P_Action_List.ChargedHeavy;
        }
    }
    void HeavyAttackAction(bool? charged)
    {
        Vector3Int? HeavyFrameData(bool? charged)
        {
            if (charged == null) // if neutral
            {
                int startUpFrames = 6;
                int activeFrames = 5;
                int recoveryFrames = 10;
                return new Vector3Int(startUpFrames, activeFrames, recoveryFrames);
            }
            else if(charged.Value)// if charged
            {
                int startUpFrames = 0; //no startupframes as you have hold frames already (SUBJECT TO PLAYTESTING)
                int activeFrames = 5;
                int recoveryFrames = 20; //longer recovery time
                return new Vector3Int(startUpFrames, activeFrames, recoveryFrames);
            }
            else return null;
        }
        int startUpFrames = HeavyFrameData(charged).Value.x;
        int activeFrames = HeavyFrameData(charged).Value.y;
        int recoveryFrames = HeavyFrameData(charged).Value.z;
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
                // Instantiate Heavy Attack child pass in parameters
                case int t when t < startUpFrames://startup phase
                    break;
                case int t when t >= startUpFrames && t < startUpFrames + activeFrames://play swing animation&check if damage dealt 
                    break;
                case int t when t >= startUpFrames + activeFrames && t < totalTicks://play recover animation
                    break;
                case int t when t >= totalTicks:
                    P_Action = P_Action_List.NULL_ACTION_STATE;// catch case
                    _tickCount = 0;
                    _heavyChargeTimer = 0;
                    _chargeBonusDamage = 0;
                    break;
            }
            _tickCount += 1;
        }
    }


}
