using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAttack1 : MonoBehaviour
{
    /// <summary>
    /// Follow the pattern and touch only the start function
    /// </summary>
    Boss_Master _BossMasterRef;
    [Tooltip("IMPORTANT!@!!|#@#!@#!@# MUST FILL OUT")]
    [SerializeField] int _LengthOfLastComboInTicks;
    [Tooltip("Amount of Gap from this combo to the next. IN TICKS 60! (ticks per second)")]
    [SerializeField] private Vector2Int _RandomOpeningRange;
    [Tooltip("Load up you attack prefabs here")]
    [SerializeField] private List<GameObject> _AttackPrefabs;
    int _CurrentAttackIndex = 0;
    void Awake()
    {
        _BossMasterRef = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss_Master>();
    }
    void Start()
    {
        _BossMasterRef.Turnspeed = 5000f;
        float invokeTimeCount = 0;
        /// Sample
        invokeTimeCount += TicksToSeconds(10); // time you want to wait before invoking
        Invoke("FirstAttack", invokeTimeCount);// Attk 1
        invokeTimeCount += TicksToSeconds(60);//length of combo
        invokeTimeCount += TicksToSeconds(UnityEngine.Random.Range(20,30)); // length of gap
        Invoke("CycleNextAttack", invokeTimeCount);// Attk 2
        invokeTimeCount += TicksToSeconds(60);//length of combo
        invokeTimeCount += TicksToSeconds(UnityEngine.Random.Range(20, 30)); // length of gap
        Invoke("CycleNextAttack", invokeTimeCount);// Attk 3
        invokeTimeCount += TicksToSeconds(60);//length of combo
        invokeTimeCount += TicksToSeconds(UnityEngine.Random.Range(20, 30)); // length of gap
        Invoke("CycleNextAttack", invokeTimeCount);// Attk 4
        //repeat etc
    }
    void Update()
    {
        if(_BossMasterRef.Boss_Action == Boss_Master.Boss_Action_List.STUNNED || _BossMasterRef.Boss_Action == Boss_Master.Boss_Action_List.STUNNED)
        {
            Invoke("ResetEverything", 0.1f);
        }
    }
    void FirstAttack()
    {
        GameObject a = Instantiate(_AttackPrefabs[_CurrentAttackIndex], this.transform); // instantiate the first attack
        a.transform.up = this.transform.up;
        _CurrentAttackIndex += 1;
    }
    void CycleNextAttack()
    {
        GameObject a = Instantiate(_AttackPrefabs[_CurrentAttackIndex], this.transform);
        a.transform.up = this.transform.up;
        _CurrentAttackIndex += 1;
        if (_CurrentAttackIndex >= _AttackPrefabs.Count)//EndCombo();
        {
            float comboLength = TicksToSeconds(_LengthOfLastComboInTicks);
            Invoke("EndCombo", comboLength);
        }
    }
    void EndCombo()
    {
        int randomOpening = UnityEngine.Random.Range(_RandomOpeningRange.x, _RandomOpeningRange.y);
        _BossMasterRef.StartBossOpening(randomOpening);
        Destroy(gameObject);
    }
    void ResetEverything()
    {
        //foreach(GameObject attack in _AttackPrefabs) Destroy(attack);
        //_AttackPrefabs.Clear();
        Destroy(gameObject);
    }
    float TicksToSeconds(int ticks)
    {
        float tickrate = 1f / 60f; // Assuming 60 fps
        float seconds = ticks * tickrate;
        return seconds;
    }
}