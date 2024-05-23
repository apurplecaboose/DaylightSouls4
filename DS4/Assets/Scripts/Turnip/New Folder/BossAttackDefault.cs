using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackDefault : Boss_Master // inherit from Boss Combo Library and Monobehaviour
{
    [Tooltip("Amount of Gap from this combo to the next")]
    [SerializeField] private Vector2Int _RandomOpeningRange;
    [Tooltip("Load up you attack prefabs here")]
    [SerializeField] private List<GameObject> _AttackPrefabs;
    [SerializeField] private int _CurrentAttackIndex = 0;
    void Awake()
    {
        Turnspeed = 1000f; //set to whatever initial value you want
    }
    float TicksToSeconds(int ticks)
    {
        ticks *= 1 / 60;
        return ticks;
    }
    void Start()
    {
        float invokeTimeCount = 0;
        invokeTimeCount += TicksToSeconds(2); // time you want to wait before invoking
        Invoke("FirstAttack", invokeTimeCount);
        invokeTimeCount += TicksToSeconds(50);//length of combo
        invokeTimeCount += TicksToSeconds(12); // length of gap
        Invoke("CycleNextAttack", invokeTimeCount);
        invokeTimeCount += TicksToSeconds(2);//length of combo
        invokeTimeCount += TicksToSeconds(2); // length of gap
        Invoke("CycleNextAttack", invokeTimeCount);
        //repeat etc
    }
    void Update()
    {
        if(Boss_Action == Boss_Action_List.STUNNED || Boss_Action == Boss_Action_List.STUNNED)
        {
            Invoke("ResetEverything", 0.1f);
        }
    }
    void FirstAttack()
    {
        Instantiate(_AttackPrefabs[_CurrentAttackIndex]); // instantiate the first attack
    }
    void CycleNextAttack()
    {
        _CurrentAttackIndex += 1;
        if (_CurrentAttackIndex < _AttackPrefabs.Count)
        {
            Instantiate(_AttackPrefabs[_CurrentAttackIndex], this.transform);
        }
        else EndCombo();
    }

    void EndCombo()
    {
        int randomOpening = UnityEngine.Random.Range(_RandomOpeningRange.x, _RandomOpeningRange.y);
        StartBossOpening(randomOpening);
        Destroy(gameObject);
    }
    void ResetEverything()
    {
        foreach(GameObject attack in _AttackPrefabs) Destroy(attack);
        _AttackPrefabs.Clear();
        Destroy(gameObject);
    }
}