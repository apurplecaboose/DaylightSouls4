using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMini : Boss_Master // inherit from Boss Master and Monobehaviour
{
    [SerializeField] int _AttackTick;
    [SerializeField] Animation _AttackAnimation;
    
    void Awake()
    {
        Turnspeed = 1000f; //set to whatever initial value you want
    }
    void Update()
    {
        if (Boss_Action == Boss_Action_List.STUNNED || Boss_Action == Boss_Action_List.STUNNED)
        {
            ResetEverything();
        }
    }
    void FixedUpdate()
    {
        //run any neessicary special code here
    }
    
    void EndofLife()
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