using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMini : MonoBehaviour // inherit from Boss Master and Monobehaviour
{
    [SerializeField] Boss_Master _BossMasterRef;
    [SerializeField] int _AttackTick;
    [SerializeField] Animation _AttackAnimation;
    
    void Awake()
    {
        _BossMasterRef.Turnspeed = 1000f; //set to whatever initial value you want
    }
    void Update()
    {
        if (_BossMasterRef.Boss_Action == Boss_Master.Boss_Action_List.STUNNED || _BossMasterRef.Boss_Action == Boss_Master.Boss_Action_List.Opening)
        {
            ResetEverything();
        }
    }
    void FixedUpdate()
    {
        //run any neessicary special code here
    }

    void EndofLife() // end of attack
    {
        Destroy(gameObject);
        //run reset code here if nessicary
    }
    void ResetEverything()
    {
        //run resetcode here
        Destroy(gameObject);
    }

}