using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class P_Master : MonoBehaviour
{
    #region Varibles
    public P_Action_List P_Action;
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
    [Header("Editable")]
    [SerializeField] float _P_MoveSpeed = 15f;
    [SerializeField] float _Ghost_MoveSpeed = 15f;
    [SerializeField] float _SwapRadius = 5;
    [SerializeField] int _DodgeStaminaCost = 15;

    [Header("References")]
    [SerializeField] GameObject _LightAttackPrefab;
    [SerializeField] GameObject _HeavyAttackPrefab;
    [SerializeField] GameObject _HeavyChargePrefab;
    [SerializeField] Stun_visual _StunPrefab;
    [SerializeField] SpriteRenderer _PlayerGhostRangeUI;
    Transform _AudioHolder;
    AudioSource _ParrySfx, _Healing;
    [SerializeField] Animator _PoofAnimator;
    Rigidbody2D _P_rb, _Ghost_rb;
    Transform _BossTransform;
    GameObject _CurrentAttackPrefab;
    Player_HealthBar _Health;
    Player_Stamina _Stamina;
    GameManager _GM;

    [Header("DEBUG DO NOT EDIT")]
    [SerializeField] int _TickCount; //Turnip:un-serialize when debug done
    public bool Dodging_Invincible, Parry_Invincible;
    [HideInInspector] public int ChargeBonusDamage;
    bool _TargetLocked;
    int _ParryIFrames, _HeavyChargeTimer;
    Vector2 _P_moveVec, _Ghost_moveVec;
    float _TargetVol = 0.5f;
    #endregion
    void Awake()
    {
        _GM = Camera.main.GetComponent<GameManager>();  
        _BossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
        _P_rb = this.transform.GetChild(0).GetComponent<Rigidbody2D>();
        _Ghost_rb = this.transform.GetChild(1).GetComponent<Rigidbody2D>();
        _Health = this.gameObject.GetComponent<Player_HealthBar>();
        _Stamina = this.gameObject.GetComponent<Player_Stamina>();
        float localscale = _SwapRadius * 2f;
        _PlayerGhostRangeUI.transform.localScale = new Vector3(localscale, localscale, 1);
        _AudioHolder = GameObject.FindGameObjectWithTag("AudioHolder").transform;
        _ParrySfx = _AudioHolder.GetChild(0).GetComponent<AudioSource>();
        _Healing = _AudioHolder.GetChild(1).GetComponent<AudioSource>();
    }
    void Update()
    {
        if (_GM.PlayState.Equals(GameManager.G_State.Selecting))
        {
            _Healing.volume = 0;
            if(_CurrentAttackPrefab != null) Destroy(_CurrentAttackPrefab);
            P_Action = P_Action_List.SelectingBossAttackState;
            return; // guard clause
        }
        else if(_GM.PlayState.Equals(GameManager.G_State.Playing))
        {
            if (P_Action.Equals(P_Action_List.SelectingBossAttackState))
            {
                P_Action = P_Action_List.NULL_ACTION_STATE;
            }
        }
        TurnPlayer();
        ClampGhostRadius();
        HealingAudio();
    }
    void HealingAudio()
    {
        if (P_Action != P_Action_List.Healing) _TargetVol = -0.5f;
        else _TargetVol = 1.25f;
        float curvol = _Healing.volume;
        float audiosourcevol = Mathf.Lerp(curvol, _TargetVol, 2 * Time.smoothDeltaTime);
        audiosourcevol = Mathf.Clamp(audiosourcevol, 0, 1f);
        _Healing.volume = audiosourcevol;
    }
    void TurnPlayer()
    {
        float turnspeed = 1000;
        if (P_Action == P_Action_List.STUNNED || P_Action == P_Action_List.LightAttack || P_Action == P_Action_List.NeutralHeavy || P_Action == P_Action_List.ChargedHeavy) return; // gaurd clause. If player is stunned dont rotate
        if (_TargetLocked) LockOn();
        else ManualAngleControl();

        void LockOn()
        {
            Vector2 target = _BossTransform.position - _P_rb.transform.position;
            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, target);
            _P_rb.transform.rotation = Quaternion.RotateTowards(_P_rb.transform.rotation, targetRot, turnspeed * Time.deltaTime);

            //_P_rb.transform.up = target; // for instant snaping lock on
        }
        void ManualAngleControl()
        {
            if (_P_moveVec == Vector2.zero) return;
            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, _P_moveVec);
            _P_rb.transform.rotation = Quaternion.RotateTowards(_P_rb.transform.rotation, targetRot, turnspeed * Time.deltaTime);
        }
    }
    void ClampGhostRadius()
    {
        Vector2 p_Pos = _P_rb.transform.position;
        Vector2 g_Pos = _Ghost_rb.transform.position;
        Vector2 playerToGhostVector = g_Pos - p_Pos;

        float pgMagnitude = playerToGhostVector.magnitude;

        Color sr_color = _PlayerGhostRangeUI.color;
        float colorlerpT = Mathf.InverseLerp(_SwapRadius * 0.9995f, _SwapRadius, pgMagnitude);
        sr_color = Color.Lerp(Color.white, Color.red, colorlerpT);
        sr_color.a = Mathf.InverseLerp(_SwapRadius * 0.55f, _SwapRadius + 1.5f, pgMagnitude);
        _PlayerGhostRangeUI.color = sr_color;
        float offset = 0.25f; // offset the edge of the sprite and the actual position
        if (pgMagnitude > _SwapRadius - offset)
        {
            if (_Ghost_moveVec != Vector2.zero) return;
            _Ghost_rb.transform.position = p_Pos + Vector2.ClampMagnitude(playerToGhostVector, _SwapRadius - offset);
        }
    }
    void FixedUpdate()
    {
        if (P_Action.Equals(P_Action_List.SelectingBossAttackState)) return;
        PogChampionParry();
        if (P_Action != P_Action_List.SwapDodge)
        {
            _Ghost_rb.AddForce(_Ghost_moveVec * _Ghost_MoveSpeed * 10f);// move Ghost in any state except stunn and dodging
        }
        switch (P_Action)
        {
            case P_Action_List.STUNNED:
                P_Stun();
                _HeavyChargeTimer = 0;
                ChargeBonusDamage = 0;
                if (_CurrentAttackPrefab == null) break;
                Destroy(_CurrentAttackPrefab);
                break;
            case P_Action_List.NULL_ACTION_STATE:
                _P_rb.AddForce(_P_moveVec * _P_MoveSpeed * 10f); // move player
                _TickCount = 0;
                break;
            case P_Action_List.SwapDodge:
                SwapDodgeAction();
                break;
            case P_Action_List.LightAttack:
                LightAttackAction();
                break;
            case P_Action_List.ChargingUpForHeavy:
                float perecentofOriginalSpeed = 0.325f;
                _P_rb.AddForce(_P_moveVec * _P_MoveSpeed * 10 * perecentofOriginalSpeed); // move player
                ChargingUpHeavyAction();
                break;
            case P_Action_List.NeutralHeavy:
                HeavyAttackAction(null);
                break;
            case P_Action_List.ChargedHeavy:
                HeavyAttackAction(true);
                break;
            case P_Action_List.Healing:
                _P_rb.AddForce(_P_moveVec * _P_MoveSpeed * 10 * 0.25f); // move player
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
                        return true;
                    case P_Action_List.SwapDodge:
                    case P_Action_List.ChargedHeavy:
                    case P_Action_List.NeutralHeavy:
                    case P_Action_List.LightAttack:
                    case P_Action_List.Healing:
                        return false;
                    default:
                        return false;
                }
            }
            if (size == P_StunSize.Large)
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
            LooneyTunesStar(stunFrames);
            _TickCount = stunFrames;
            P_Action = P_Action_List.STUNNED;
            //prob add some screenshake trigger here
        }
        else Debug.Log("HyperArmor");

        void LooneyTunesStar(int frames)
        {
            Stun_visual star = Instantiate(_StunPrefab, Camera.main.transform);
            star.ActiveFrames = frames;
            star.Offset = 0.4f;
            star.Target = _P_rb.transform;
        }
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
        if (input.performed && P_Action != P_Action_List.STUNNED)
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
    #region DodgeN'Heal
    public void SwapDodgeInput(InputAction.CallbackContext inputState)
    {
        if (inputState.performed)
        {
            if (Vector3.Distance(_P_rb.transform.position, _Ghost_rb.transform.position) > _SwapRadius)
            {
                Debug.Log("Out of swap range");
                return; // guard case
            }
            if (_Stamina.StamimaValue < _DodgeStaminaCost / 2) return;
            if (P_Action == P_Action_List.ChargingUpForHeavy)
            {
                if (_TickCount > 5) // used to count cooldown on charge heavy swap
                {
                    _Stamina.StaminaConsuming(_DodgeStaminaCost / 2);
                    _TickCount = 0; // reset 
                    //fire off swap event
                    Vector2 pVelCache = _P_rb.velocity;
                    Vector2 gVelCache = _P_rb.velocity;
                    _P_rb.velocity = gVelCache;
                    _Ghost_rb.velocity = pVelCache;
                    Vector3 playertransformcache = _P_rb.transform.position;
                    Vector3 ghosttransformcache = _Ghost_rb.transform.position;
                    _P_rb.transform.position = ghosttransformcache;
                    _Ghost_rb.transform.position = playertransformcache;
                    return;
                }
            }
            if (_Stamina.StamimaValue < _DodgeStaminaCost) return;
            if (P_Action == P_Action_List.NULL_ACTION_STATE)
            {
                P_Action = P_Action_List.SwapDodge;
                _Stamina.StaminaConsuming(_DodgeStaminaCost);
                return;
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
        int recoveryFrames = 5;
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
    InputAction.CallbackContext? _HealInputAction = null; //set inputaction callback context varible to null.
    public void HealingInput(InputAction.CallbackContext inputState)
    {
        if (_HealInputAction == null)
        {
            _HealInputAction = inputState; //Will be assigned on first button press event
        }
        if (inputState.performed)
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
            if (_TickCount == 10) // same as healing input recoveryFrames
            { // will only run heal when tick is 10 otherwise it is in recovery mode and no heal is done just cooldown
                _Health.RestoreHealth();
            }
            _TickCount -= 1;
        }
    }
    #endregion
    /*---------------------------------------------------------------------------------------------------------*/
    #region AttackStuff
    public void LightAttackInput(InputAction.CallbackContext inputState)
    {
        if (inputState.performed)
        {
            if (P_Action == P_Action_List.NULL_ACTION_STATE || P_Action == P_Action_List.SwapDodge)
            {
                P_Action = P_Action_List.LightAttack;
                _TickCount = 0;
                Dodging_Invincible = false;
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
        int activeFrames = 21;
        int recoveryFrames = 6;
        int totalTicks = startUpFrames + activeFrames + recoveryFrames;

        if (_TickCount >= totalTicks)
        { //Turnip: done ticking reset back to null action state and set tickcount back to 0 for next action
            Destroy(_CurrentAttackPrefab);
            P_Action = P_Action_List.NULL_ACTION_STATE;
            _TickCount = 0;
            return;
        }
        else
        { // Turnip: run logic 
            switch (_TickCount)
            {
                case int t when t == startUpFrames:
                    _CurrentAttackPrefab = Instantiate(_LightAttackPrefab, _P_rb.transform);
                    _CurrentAttackPrefab.transform.right = _P_rb.transform.right;
                    break;
                case int T when T == totalTicks - recoveryFrames:
                    Destroy(_CurrentAttackPrefab);
                    break;
                case int t when t >= totalTicks:
                    ///catch case dont run stuff here u idiot
                    Destroy(_CurrentAttackPrefab);
                    P_Action = P_Action_List.NULL_ACTION_STATE;// catch case
                    _TickCount = 0;
                    break;
            }
            _TickCount += 1;
        }
    }
    /*---------------------------------------------------------------------------------------------------------*/
    public void HeavyAttackInput(InputAction.CallbackContext inputState)
    {
        if (inputState.performed)
        {
            if (P_Action == P_Action_List.NULL_ACTION_STATE || P_Action == P_Action_List.SwapDodge)
            {
                P_Action = P_Action_List.ChargingUpForHeavy;
                _TickCount = 0;
                Dodging_Invincible = false;
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
                int holdthreshold = 24;
                if (_HeavyChargeTimer >= holdthreshold)
                {
                    _TickCount = 0;
                    P_Action = P_Action_List.ChargedHeavy;
                }
                else if (_HeavyChargeTimer < holdthreshold)
                {
                    _TickCount = 0;
                    P_Action = P_Action_List.NeutralHeavy;
                }
            }
        }
    }
    void ChargingUpHeavyAction()//play charge animation, not able to move, disruptable by swap
    {
        int maxChargeTime = 150;
        _HeavyChargeTimer += 1;
        _TickCount += 1;
        ChargeBonusDamage = _HeavyChargeTimer;
        ChargeBonusDamage = Mathf.Clamp(ChargeBonusDamage, 0, maxChargeTime);
        if (_HeavyChargeTimer >= maxChargeTime)
        {
            P_Action = P_Action_List.ChargedHeavy;
            _TickCount = 0;
        }
        if (_HeavyChargeTimer == 5)
        {
            _CurrentAttackPrefab = Instantiate(_HeavyChargePrefab, _P_rb.transform);
            _CurrentAttackPrefab.transform.right = _P_rb.transform.right;
        }
    }
    void HeavyAttackAction(bool? charged)
    {
        Vector3Int? HeavyFrameData(bool? charged)
        {
            if (charged == null) // if neutral
            {
                int startUpFrames = 5;
                int activeFrames = 38;
                int recoveryFrames = 20;
                return new Vector3Int(startUpFrames, activeFrames, recoveryFrames);
            }
            else if (charged.Value)// if charged
            {
                int startUpFrames = 0; //no startupframes as you have hold frames already (SUBJECT TO PLAYTESTING)
                int activeFrames = 38;
                int recoveryFrames = 30; //longer recovery time
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
            Destroy(_CurrentAttackPrefab);
            P_Action = P_Action_List.NULL_ACTION_STATE;
            _TickCount = 0;
            _HeavyChargeTimer = 0;
            ChargeBonusDamage = 0;
            return;
        }
        else
        { // Turnip: run logic 
            switch (_TickCount)
            {
                case int t when t == startUpFrames:
                    if (_CurrentAttackPrefab != null) Destroy(_CurrentAttackPrefab);
                    _CurrentAttackPrefab = Instantiate(_HeavyAttackPrefab, _P_rb.transform);
                    _CurrentAttackPrefab.transform.right = _P_rb.transform.right;
                    break;
                case int t when t == totalTicks - recoveryFrames + 5:
                    Destroy(_CurrentAttackPrefab);
                    break;
                case int t when t >= totalTicks:
                    // catch case DONT PUT STUFF HERE
                    P_Action = P_Action_List.NULL_ACTION_STATE;
                    _TickCount = 0;
                    _HeavyChargeTimer = 0;
                    ChargeBonusDamage = 0;
                    break;
            }
            _TickCount += 1;
        }
    }
    public void PogChampionParry(int iframes)
    {
        _PoofAnimator.transform.position = (_P_rb.transform.position + _BossTransform.position) * 0.5f;
        _PoofAnimator.Play("parrysmoke");

        AudioSource a = _ParrySfx;
        a.pitch = Random.Range(0.85f, 1);
        a.Play();
        _ParryIFrames = iframes;

        _Health.RestoreParry();
    }
    void PogChampionParry()
    {
        if (_ParryIFrames > 0)
        {
            _ParryIFrames -= 1;
            Parry_Invincible = true;
        }
        else Parry_Invincible = false;
    }
    #endregion
}
