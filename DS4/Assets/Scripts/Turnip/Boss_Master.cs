using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Master : MonoBehaviour
{
    GameManager _GM;
    public GameObject ComboSelectionUI_Prefab;
    B_ComboLibrary _Lib;    
    ComboPossibility _Combopossibility;

    [Header("Boss Master")]
    [SerializeField] int _CurrentBossComboIndex = 0;
    float _BossDefaultSpeed = 3;
    [SerializeField] int _BossPoiseAmount = 750;
    [SerializeField]int _OpeningTick, _PoiseDamage, _PoiseTickTimer;
    [HideInInspector] public float Turnspeed = 1000f;
    [SerializeField] Transform _PlayerTransform;
    AudioSource _PoiseBreakSFX;
    public PathFinding BossPathfinding;
    [SerializeField] Stun_visual _StunPrefab;

    [HideInInspector] public GameObject CurrentAttackMini, LastAttackMini;
    public enum Boss_Action_List
    {
        Chasing,
        Attack,
        Opening,
        STUNNED,
        SelectingBossAttackState // => chasing
    }
    public Boss_Action_List Boss_Action;

    void Awake()
    {
        _GM = Camera.main.GetComponent<GameManager>();
        _Lib = this.GetComponent<B_ComboLibrary>();
        _Combopossibility = this.GetComponent<ComboPossibility>();
        BossPathfinding = this.GetComponent<PathFinding>();
        _PoiseBreakSFX = GameObject.FindGameObjectWithTag("AudioHolder").transform.GetChild(2).GetComponent<AudioSource>();
    }
    private void Start()
    {
        GameObject selection = Instantiate(ComboSelectionUI_Prefab);
        _GM.ComboSelectionUI_Instance = selection;
        Boss_Action = Boss_Action_List.SelectingBossAttackState;
    }
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Backspace)) { Instantiate(ComboSelectionUI_Prefab); }
        RotateBoss();
        if(Boss_Action == Boss_Action_List.Chasing)
        {
            //boss speed = based on distance to player; maybe
            BossPathfinding.Speed = _BossDefaultSpeed;
            Turnspeed = 1000f;
            if(_CurrentBossComboIndex >= 8)
            {
                if (_GM.ComboSelectionUI_Instance != null) return;
                _CurrentBossComboIndex = 0;
                GameObject selection = Instantiate(ComboSelectionUI_Prefab);
                _GM.ComboSelectionUI_Instance = selection;
                Boss_Action = Boss_Action_List.SelectingBossAttackState;
                return;
            }
            if (Vector3.Distance(_PlayerTransform.position, this.transform.position) < 4) //distance is less than amount
            {
                ComboPossibility.ComboType currentCombo = _Combopossibility.FinalOutputComboArray[_CurrentBossComboIndex];
                _Lib.StartUp(currentCombo);
                Boss_Action = Boss_Action_List.Attack;
            }
        }
        //toggle between playing and selecting
        if (Boss_Action.Equals(Boss_Action_List.SelectingBossAttackState))
        {
            BossPathfinding.Speed = 0;
            if (_GM.PlayState.Equals(GameManager.G_State.Playing)) Boss_Action = Boss_Action_List.Chasing;
        }
        else if(_GM.PlayState.Equals(GameManager.G_State.Selecting))
        {
            Boss_Action = Boss_Action_List.SelectingBossAttackState;
        }
    }
    void FixedUpdate()
    {
        BossPoiseLogic();

        if(Boss_Action == Boss_Action_List.Opening || Boss_Action == Boss_Action_List.STUNNED)
        {
            Turnspeed = 1000f;
            BossPathfinding.Speed = 0;
            if (_OpeningTick <= 0)
            {
                _OpeningTick = 0;
                Boss_Action = Boss_Action_List.Chasing;
                CycleNextCombo();
                return;
            }
            _OpeningTick -= 1;
        }
    }
    void CycleNextCombo()
    {
        if (_CurrentBossComboIndex < _Combopossibility.FinalOutputComboArray.ToArray().Length)
        {
            Boss_Action = Boss_Action_List.Chasing; //catch case
            _CurrentBossComboIndex += 1;//increment index
        }
        else // combo past end of array
        {
            //catch case
            if (_GM.ComboSelectionUI_Instance != null) return;
            _CurrentBossComboIndex = 0;
            GameObject selection = Instantiate(ComboSelectionUI_Prefab);
            _GM.ComboSelectionUI_Instance = selection;
            Boss_Action = Boss_Action_List.SelectingBossAttackState;
        }
    }
    void RotateBoss()
    {
        if (Boss_Action == Boss_Action_List.STUNNED) return; // gaurd clause. If stunned dont rotate
        if (Boss_Action == Boss_Action_List.Opening) return;

        Vector2 target = _PlayerTransform.position - this.transform.position;
        Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, target);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRot, Turnspeed * Time.deltaTime);
    }
    public void AddPoiseDamage(int poiseDamagetoAdd)
    {
        if (Boss_Action == Boss_Action_List.Opening)
        {
            _PoiseDamage += poiseDamagetoAdd / 2; // half the poise damage dealt if the boss is in opening mode
            int refreshtime = 50 * 3;// 50 tick/s * desiredSeconds
            if (_PoiseTickTimer < refreshtime) // if you have 3 seconds or more then refresh to 3 otherwise keep at the larger tick timer amount
            {
                _PoiseTickTimer = refreshtime;
                return;
            }
            else return;
        }
        else if (Boss_Action != Boss_Action_List.STUNNED) // if stunned dont let the player do poise damage
        {
            _PoiseDamage += poiseDamagetoAdd;
            _PoiseTickTimer = 50 * 6; // 50 tick/s * desiredSeconds
        }
    }
    void BossPoiseLogic()
    {
        if(_PoiseDamage >= _BossPoiseAmount)
        {
            _PoiseBreakSFX.Play();
            if (_PoiseDamage == _BossPoiseAmount)
            {
                _PoiseDamage = 0;
                _PoiseTickTimer = 0;
                Boss_StunInput(50*3);
            }
            else
            {
                int surplusPoiseDamage = _PoiseDamage - _BossPoiseAmount;
                float curve = Mathf.Clamp(0.15f * Mathf.Pow(surplusPoiseDamage, 1.5f), 0, 100);// some arbitrary function, subject to change TM lel
                int bonusStun = Mathf.RoundToInt(curve); 
                _PoiseDamage = 0;
                _PoiseTickTimer = 0;
                Boss_StunInput(50 * 3 + bonusStun);
            }
        }

        void Boss_StunInput(int stunFrames)
        {
            LooneyTunesStar(stunFrames);
            _OpeningTick = stunFrames;
            Boss_Action = Boss_Action_List.STUNNED;
            _Lib.StunBoss_DestroyCurrentAttack();
            //prob add some screenshake trigger here
        }

        if (_PoiseTickTimer > 0) _PoiseTickTimer -= 1;// wait for "6" seconds before poise starts ticking down
        else if (_PoiseDamage > 0) _PoiseDamage -= 3; // if waiting timer is 0 and poise is > 0 then tick down the poise damage 
    }
    public void StartBossOpening(int bossOpeningTimeinTicks)
    {
        _OpeningTick = bossOpeningTimeinTicks;
        Boss_Action = Boss_Action_List.Opening;
    }
    void LooneyTunesStar(int frames)
    {
        Stun_visual star = Instantiate(_StunPrefab, Camera.main.transform);
        star.ActiveFrames = frames;
        star.Target = this.transform;
        star.Offset = 0.85f;
        star.transform.localScale = new Vector3(1.25f, 1.25f, 1);
    }
    public void DestroyEverythingonBoss()
    {
        Destroy(_Combopossibility);
        Destroy(_Lib);
        Destroy(BossPathfinding);
        Destroy(this.GetComponent<BossHeathbar>());
        Destroy(this);
    }
}
