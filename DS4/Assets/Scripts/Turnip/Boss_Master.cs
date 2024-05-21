using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Master : MonoBehaviour
{
    B_ComboLibrary _Lib;
    ComboPossibility _Combopossibility;

    [Header("Boss Master")]
    [SerializeField] int _CurrentBossComboIndex = 0;
    public int BossPoiseAmount = 500;
    [SerializeField] int _OpeningTick, _PoiseDamge;
    [SerializeField] int _PoiseTickTimer;
    
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
    }
    void Start()
    {
        
    }
    void Update()
    {
        if(Boss_Action == Boss_Action_List.Chasing)
        {
            //boss speed = based on distance to player;

            //if() distance is less than amount and some amount of timer
            {
                ComboPossibility.ComboType currentCombo = _Combopossibility.ChosenComboArray[_CurrentBossComboIndex];
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
            if(_OpeningTick <= 0)
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
        if (_CurrentBossComboIndex < /*ChosenComboArray.ToArray().Length*/ 1 /*temp*/)
        {
            Boss_Action = Boss_Action_List.Chasing;
            _CurrentBossComboIndex += 1;
        }
        else
        {
            //trigger next boss combo selection
        }
    }
    public void AddPoiseDamage(int poiseDamagetoAdd)
    {
        if (Boss_Action == Boss_Action_List.Opening)
        {
            _PoiseDamge += poiseDamagetoAdd / 2; // half the poise damage dealt if the boss is in opening mode
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
            _PoiseDamge += poiseDamagetoAdd;
            _PoiseTickTimer = 50 * 6; // 50 tick/s * desiredSeconds
        }
    }
    void BossPoiseLogic()
    {
        if(_PoiseDamge >= BossPoiseAmount)
        {
            if(_PoiseDamge == BossPoiseAmount)
            {
                _PoiseDamge = 0;
                _PoiseTickTimer = 0;
                Boss_StunInput(50*3);
            }
            else
            {
                int surplusPoiseDamage = _PoiseDamge - BossPoiseAmount;
                int bonusStun = Mathf.RoundToInt(2 * Mathf.Pow( surplusPoiseDamage, 2)); // some arbitrary function, subject to change TM lel
                _PoiseDamge = 0;
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
        else if (_PoiseDamge > 0) _PoiseDamge -= 1; // if waiting timer is 0 and poise is > 0 then tick down the poise damage 
    }
    public void StartBossOpening(int bossOpeningTimeinTicks)
    {
        _OpeningTick = bossOpeningTimeinTicks;
        Boss_Action = Boss_Action_List.Opening;
    }

}
