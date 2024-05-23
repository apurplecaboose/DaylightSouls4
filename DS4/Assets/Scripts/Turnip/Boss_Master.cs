using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Master : MonoBehaviour
{
    B_ComboLibrary _Lib;    
    ComboPossibility _Combopossibility;

    [Header("Boss Master")]
    int _CurrentBossComboIndex = 0;
    [SerializeField] int _BossPoiseAmount = 500;
    int _OpeningTick, _PoiseDamage, _PoiseTickTimer;
    [HideInInspector] public float Turnspeed = 1000f;
    Transform _PlayerTransform;
    public PathFinding BossPathfinding;
    public enum Boss_Action_List
    {
        Chasing,
        Attack,
        Opening,
        STUNNED,
        SelectingBossAttackState
    }
    public Boss_Action_List Boss_Action;

    void Awake()
    {
        _Lib = this.GetComponent<B_ComboLibrary>();
        _Combopossibility = this.GetComponent<ComboPossibility>();
        BossPathfinding = this.GetComponent<PathFinding>();
        _PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        RotateBoss();
        if(Boss_Action == Boss_Action_List.Chasing)
        {
            //boss speed = based on distance to player;
            BossPathfinding.Speed = 5;
            Turnspeed = 1000f;
            if (Vector3.Distance(_PlayerTransform.position, this.transform.position) < 4) //distance is less than amount
            {
                ComboPossibility.ComboType currentCombo = _Combopossibility.ChosenComboFromKen[_CurrentBossComboIndex];
                _Lib.StartUp(currentCombo);
                Boss_Action = Boss_Action_List.Attack;
            }
        }
        else
        {
            //turn off boss movement?
                //dumb idea move the boss in late update and set boss move speed to 0 at the end of the loop
                //and if you want boss movement every frame in the update set the speed to something nonzero
        }
    }
    void FixedUpdate()
    {
        BossPoiseLogic();

        if(Boss_Action == Boss_Action_List.Opening || Boss_Action == Boss_Action_List.STUNNED)
        {
            Turnspeed = 1000f;
            if (_OpeningTick <= 0)
            {
                _OpeningTick = 0;
                CycleNextCombo();
                return;
            }
            _OpeningTick -= 1;
        }
    }
    void CycleNextCombo()
    {
        if (_CurrentBossComboIndex < _Combopossibility.ChosenComboFromKen.ToArray().Length)
        {
            Boss_Action = Boss_Action_List.Chasing;
            _CurrentBossComboIndex += 1;
        }
        else
        {
            //trigger next boss combo selection
        }
    }
    void RotateBoss()
    {
        if (Boss_Action == Boss_Action_List.STUNNED) return; // gaurd clause. If stunned dont rotate
        if (Boss_Action == Boss_Action_List.Opening) return;

        Vector2 target = _PlayerTransform.position - this.transform.position;
        Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, target);
        _PlayerTransform.transform.rotation = Quaternion.RotateTowards(_PlayerTransform.transform.rotation, targetRot, Turnspeed * Time.deltaTime);
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
            if(_PoiseDamage == _BossPoiseAmount)
            {
                _PoiseDamage = 0;
                _PoiseTickTimer = 0;
                Boss_StunInput(50*3);
            }
            else
            {
                int surplusPoiseDamage = _PoiseDamage - _BossPoiseAmount;
                int bonusStun = Mathf.RoundToInt(2 * Mathf.Pow( surplusPoiseDamage, 2)); // some arbitrary function, subject to change TM lel
                _PoiseDamage = 0;
                _PoiseTickTimer = 0;
                Boss_StunInput(50 * 3 + bonusStun);
            }
        }

        void Boss_StunInput(int stunFrames)
        {
            _OpeningTick = stunFrames;
            Boss_Action = Boss_Action_List.STUNNED;
            _Lib.StunBoss_DestroyCurrentAttack();
            //prob add some screenshake trigger here
        }

        if (_PoiseTickTimer > 0) _PoiseTickTimer -= 1;// wait for "6" seconds before poise starts ticking down
        else if (_PoiseDamage > 0) _PoiseDamage -= 1; // if waiting timer is 0 and poise is > 0 then tick down the poise damage 
    }
    public void StartBossOpening(int bossOpeningTimeinTicks)
    {
        _OpeningTick = bossOpeningTimeinTicks;
        Boss_Action = Boss_Action_List.Opening;
    }

}
