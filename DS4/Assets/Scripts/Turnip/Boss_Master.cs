using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static P_Master;

public class Boss_Master : MonoBehaviour
{
    public BossDataSc BossDataSc;
    public BossDataSc.ComboType[] AttackArray;
    public int CurrentBossCombo = 0;
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
        
    }
    void Start()
    {
        
    }
    void Update()
    {
        if(Boss_Action == Boss_Action_List.Chasing)
        {
            //if() distance is less than amount and some amount of timer
            {
                //Trigger Library Startup function
                Boss_Action = Boss_Action_List.Attack;
                //boss speed =5;
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

        if(Boss_Action == Boss_Action_List.Opening)
        {
            if(_OpeningTick <= 0)
            {
                _OpeningTick = 0;
                CycleNextCombo();

                void CycleNextCombo()
                {
                    if (CurrentBossCombo < AttackArray.Length)
                    {
                        Boss_Action = Boss_Action_List.Chasing;
                        CurrentBossCombo += 1;
                    }
                    else
                    {
                        //trigger next boss combo selection
                    }
                }
            }
            _OpeningTick -= 1;
        }
        if(Boss_Action == Boss_Action_List.STUNNED)
        {
            Boss_Stun();
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
            //prob add some screenshake trigger here
        }

        if (_PoiseTickTimer > 0) _PoiseTickTimer -= 1;// wait for "6" seconds before poise starts ticking down
        else if (_PoiseDamge > 0) _PoiseDamge -= 1; // if waiting timer is 0 and poise is > 0 then tick down the poise damage 
    }
    void Boss_Stun()
    {
        if (_OpeningTick <= 0)
        { //Turnip: done ticking reset back to null action state and set tickcount back to 0 for next action
            // reset to chase state
            _OpeningTick = 0;
            return;
        }
        else _OpeningTick -= 1;
    }
    public void StartBossOpening(int bossOpeningTimeinTicks)
    {
        _OpeningTick = bossOpeningTimeinTicks;
        Boss_Action = Boss_Action_List.Opening;
    }

}
