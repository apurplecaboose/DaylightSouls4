using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_SimpleStabAttack : MonoBehaviour
{
    /// <summary>
    /// Follow the pattern and touch only the start function
    /// </summary>
    Boss_Master _BossMasterRef;
    [Tooltip("IMPORTANT!@!!|#@#!@#!@# MUST FILL OUT")]
    [SerializeField] int _LengthOfLastComboInTicks;
    [Tooltip("Amount of Gap from this combo to the next. IN TICKS 60! (ticks per second)")]
    [SerializeField] private Vector2Int _RandomOpeningRange;
    [Tooltip("Load up you attack prefabs here. Ensure is greater than 1. Ensure invoke amount is same as combo amount")]
    [SerializeField] private List<GameObject> _MiniAttackPrefabs;
    int _CurrentAttackIndex = 0;
    void Awake() // Don't Touch
    {
        _BossMasterRef = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss_Master>();
    }
    void Start() //Changable
    {
        _BossMasterRef.Turnspeed = 5000f;
        float invokeTimeCount = 0;
        /// Sample
        invokeTimeCount += TicksToSeconds(UnityEngine.Random.Range(0, 20)); // time you want to wait before invoking
        Invoke("FirstAttack", invokeTimeCount);// Attk 1
        invokeTimeCount += TicksToSeconds(70);//length of combo
        invokeTimeCount += TicksToSeconds(UnityEngine.Random.Range(20, 30)); // length of gap
        Invoke("CycleNextAttack", invokeTimeCount);// Attk 2
        invokeTimeCount += TicksToSeconds(70);//length of combo
        invokeTimeCount += TicksToSeconds(UnityEngine.Random.Range(60, 90)); // length of gap
        Invoke("CycleNextAttack", invokeTimeCount);// Attk 3
        invokeTimeCount += TicksToSeconds(70);//length of combo
        invokeTimeCount += TicksToSeconds(UnityEngine.Random.Range(20, 30)); // length of gap
        Invoke("CycleNextAttack", invokeTimeCount);// Attk 4
        //repeat etc
    }
    void Update() // Don't Touch
    {
        if(_BossMasterRef.Boss_Action == Boss_Master.Boss_Action_List.STUNNED || _BossMasterRef.Boss_Action == Boss_Master.Boss_Action_List.STUNNED)
        {
            Invoke("ResetEverything", 0.1f);
        }
    }
    void FirstAttack() // Don't Touch
    {
        GameObject a = Instantiate(_MiniAttackPrefabs[_CurrentAttackIndex], this.transform); // instantiate the first attack
        a.transform.up = this.transform.up;
        if (_BossMasterRef.CurrentAttackMini == null)
        {
            _BossMasterRef.CurrentAttackMini = a;
        }
        else
        {
            _BossMasterRef.LastAttackMini = _BossMasterRef.CurrentAttackMini;
            _BossMasterRef.CurrentAttackMini = a;
        }
        _CurrentAttackIndex += 1;
    }
    void CycleNextAttack() // Don't Touch
    {
        GameObject a = Instantiate(_MiniAttackPrefabs[_CurrentAttackIndex], this.transform);
        a.transform.up = this.transform.up;
         if (_BossMasterRef.CurrentAttackMini == null)
        {
            _BossMasterRef.CurrentAttackMini = a;
        }
        else
        {
            _BossMasterRef.LastAttackMini = _BossMasterRef.CurrentAttackMini;
            _BossMasterRef.CurrentAttackMini = a;
        }
        _CurrentAttackIndex += 1;
        if (_CurrentAttackIndex >= _MiniAttackPrefabs.Count)//EndCombo();
        {
            float comboLength = TicksToSeconds(_LengthOfLastComboInTicks);
            Invoke("EndCombo", comboLength);
        }
    }
    void EndCombo() // Don't Touch
    {
        int randomOpening = UnityEngine.Random.Range(_RandomOpeningRange.x, _RandomOpeningRange.y);
        _BossMasterRef.StartBossOpening(randomOpening);
        Destroy(gameObject);
    }
    void ResetEverything() // Don't Touch
    {
        //foreach(GameObject attack in _AttackPrefabs) Destroy(attack);
        //_AttackPrefabs.Clear();
        Destroy(gameObject);
    }
    float TicksToSeconds(int ticks) // Don't Touch
    {
        float tickrate = 1f / 60f; // Assuming 60 fps
        float seconds = ticks * tickrate;
        return seconds;
    }
}