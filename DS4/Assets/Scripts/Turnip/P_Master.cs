using System.Collections;
using System.Collections.Generic;
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
        STUNNED,
        SelectingBossAttackState
    }
    [SerializeField] bool _TargetLocked;
    [SerializeField] int _TickCount; //Turnip:un-serialize when debug done
    int _ParryIFrames;
    int _HeavyChargeTimer; 
    public int ChargeBonusDamage;

    public P_Action_List P_Action;
    [SerializeField] float _P_MoveSpeed = 15f, _Ghost_MoveSpeed = 15f;
    Vector2 _P_moveVec, _Ghost_moveVec;
    [SerializeField] Rigidbody2D _P_rb, _Ghost_rb;//Turnip:un-serialize when debug done
    public Transform BossTransform;
    public bool Dodging_Invincible, Parry_Invincible;

    void Update()
    {
        TurnPlayer();
        TestStun();
    }
    void TestStun()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            P_StunInput(200, P_StunSize.Large);
        }
    }
    void TurnPlayer()
    {
        float turnspeed = 1000;
        if (P_Action == P_Action_List.STUNNED) return; // gaurd clause. If player is stunned dont rotate
        if (_TargetLocked) LockOn();
        else ManualAngleControl();


        void LockOn()
        {
            Vector2 target = BossTransform.position - _P_rb.transform.position;
            //Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, target);
            //_P_rb.transform.rotation = Quaternion.RotateTowards(_P_rb.transform.rotation, targetRot, turnspeed * Time.deltaTime);

            _P_rb.transform.up = target; // for instant snaping lock on
        }
        void ManualAngleControl()
        {
            if (_P_moveVec == Vector2.zero) return;
            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, _P_moveVec);
            _P_rb.transform.rotation = Quaternion.RotateTowards(_P_rb.transform.rotation, targetRot, turnspeed * Time.deltaTime);
        }
    }
    void FixedUpdate()
    {
        PogChampionParry();
        switch (P_Action)
        {
            case P_Action_List.STUNNED:
                P_Stun();
                break;
            case P_Action_List.NULL_ACTION_STATE:
                _P_rb.AddForce(_P_moveVec * _P_MoveSpeed * 10f); // move player
                _Ghost_rb.AddForce(_Ghost_moveVec * _Ghost_MoveSpeed * 10f);// move Ghost
                _TickCount = 0;
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
                if (_HealInputAction.Value.action.IsPressed()) _TickCount = 10; //get key down equivalant
                break;
        }
    }
    public enum P_StunSize { Small, Large }
    public void P_StunInput(int stunFrames, P_StunSize size)
    {
        bool Stunnable()
        {
            if (size == P_StunSize.Small)
            {
                switch (P_Action)
                {
                    case P_Action_List.NULL_ACTION_STATE:
                    case P_Action_List.ChargingUpForHeavy:
                    case P_Action_List.Healing:
                        return true;
                    case P_Action_List.SwapDodge:
                    case P_Action_List.ChargedHeavy: 
                    case P_Action_List.NeutralHeavy:
                    case P_Action_List.LightAttack:
                        return false;
                    default:
                        return false;
                }
            }
            if(size == P_StunSize.Large)
            {
                switch (P_Action)
                {
                    case P_Action_List.NULL_ACTION_STATE:
                    case P_Action_List.LightAttack:
                    case P_Action_List.ChargingUpForHeavy:
                    case P_Action_List.Healing:
                        return true;
                    case P_Action_List.SwapDodge or P_Action_List.ChargedHeavy or P_Action_List.NeutralHeavy:
                        return false;
                    default:
                        return false;
                }
            }
            else return false;
        }
        if (Stunnable())
        {
            _TickCount = stunFrames;
            P_Action = P_Action_List.STUNNED;
            //prob add some screenshake trigger here
        }
        else Debug.Log("HyperArmor");
    }
    void P_Stun()
    {
        if (_TickCount <= 0)
        { //Turnip: done ticking reset back to null action state and set tickcount back to 0 for next action
            P_Action = P_Action_List.NULL_ACTION_STATE;
            _TickCount = 0;
            return;
        }
        else _TickCount -= 1;
    }
    /*---------------------------------------------------------------------------------------------------------*/
    public void TargetLock(InputAction.CallbackContext input)
    {
        if(input.performed && P_Action != P_Action_List.STUNNED)
        {
            _TargetLocked = !_TargetLocked; 
        }
    }
    public void P_Move(InputAction.CallbackContext input)
    {
        _P_moveVec = input.ReadValue<Vector2>(); //Turnip: writes to input to vector which will add force in the fixed update loop
    }
    public void Ghost_Move(InputAction.CallbackContext input)
    {
        _Ghost_moveVec = input.ReadValue<Vector2>(); //Turnip: same as P_move
    }
    /*---------------------------------------------------------------------------------------------------------*/
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
    public void PogChampionParry(int iframes)
    {
        _ParryIFrames = iframes;
    }
    void PogChampionParry()
    {
        if(_ParryIFrames > 0)
        {
            _ParryIFrames -= 1;
            Parry_Invincible = true;
        }           
        else Parry_Invincible = false;
    }
    void SwapDodgeAction()
    {
        //swap player UNINTERUPTABLE
        int startUpFrames = 1;
        int active_i_Frames = 13;
        int recoveryFrames = 8;
        int totalTicks = startUpFrames + active_i_Frames + recoveryFrames;

        if (_TickCount >= totalTicks)
        { //Turnip: done ticking reset back to null action state and set tickcount back to 0 for next action
            P_Action = P_Action_List.NULL_ACTION_STATE;
            _TickCount = 0;
            return;
        }
        else
        { // Turnip: run logic 
            switch (_TickCount)
            {
                case int t when t < startUpFrames:
                    Dodging_Invincible = false;
                    break;
                case int t when t == startUpFrames:
                    Dodging_Invincible = true;
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
                    Dodging_Invincible = true;
                    break;
                case int t when t >= startUpFrames + active_i_Frames && t < totalTicks:
                    Dodging_Invincible = false;
                    break;
                case int t when t >= totalTicks:
                    P_Action = P_Action_List.NULL_ACTION_STATE;// catch case
                    _TickCount = 0;
                    Dodging_Invincible = false;
                    break;
            }
            _TickCount += 1;
        }
    }
    /*---------------------------------------------------------------------------------------------------------*/
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

        if (_TickCount >= totalTicks)
        { //Turnip: done ticking reset back to null action state and set tickcount back to 0 for next action
            P_Action = P_Action_List.NULL_ACTION_STATE;
            _TickCount = 0;
            return;
        }
        else
        { // Turnip: run logic 
            switch (_TickCount)
            {
                case int t when t < startUpFrames:
                    break;
                case int t when t >= startUpFrames && t < startUpFrames + activeFrames:

                    //instantiate 
                    break;
                case int t when t >= startUpFrames + activeFrames && t < totalTicks:
                    break;
                case int t when t >= totalTicks:
                    P_Action = P_Action_List.NULL_ACTION_STATE;// catch case
                    _TickCount = 0;
                    break;
            }
            _TickCount += 1;
        }
    }
    /*---------------------------------------------------------------------------------------------------------*/
    InputAction.CallbackContext? _HealInputAction = null; //set inputaction callback context varible to null.
    public void HealingInput(InputAction.CallbackContext inputState)
    {
        if(_HealInputAction == null)
        {
            _HealInputAction = inputState; //Will be assigned on first button press event
        }
        if(inputState.performed)
        {
            if (P_Action == P_Action_List.NULL_ACTION_STATE)
            {
                P_Action = P_Action_List.Healing;
                int recoveryFrames = 10;
                _TickCount = recoveryFrames;
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
        if (_TickCount <= 0)
        { //Turnip: done ticking reset back to null action state and set tickcount back to 0 for next action
            P_Action = P_Action_List.NULL_ACTION_STATE;
            _TickCount = 0;
            return;
        }
        else
        {
            if(_TickCount == 10) // same as healing input recoveryFrames
            { // will only run heal when tick is 10 otherwise it is in recovery mode and no heal is done just cooldown
                
                
                //run healing stuff here
            }
            _TickCount -= 1; 
        }
    }
    /*---------------------------------------------------------------------------------------------------------*/
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
                if (_HeavyChargeTimer >= holdthreshold)
                {
                    P_Action = P_Action_List.ChargedHeavy;
                }
                else if (_HeavyChargeTimer < holdthreshold)
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
        _HeavyChargeTimer += 1;
        ChargeBonusDamage = _HeavyChargeTimer - holdthreshold;
        ChargeBonusDamage = Mathf.Clamp(ChargeBonusDamage, 0, maxChargeTime);
        if (_HeavyChargeTimer >= maxChargeTime)
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

        if (_TickCount >= totalTicks)
        { //Turnip: done ticking reset back to null action state and set tickcount back to 0 for next action
            P_Action = P_Action_List.NULL_ACTION_STATE;
            _TickCount = 0;
            return;
        }
        else
        { // Turnip: run logic 
            switch (_TickCount)
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
                    _TickCount = 0;
                    _HeavyChargeTimer = 0;
                    ChargeBonusDamage = 0;
                    break;
            }
            _TickCount += 1;
        }
    }
}
