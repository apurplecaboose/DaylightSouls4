using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAMPLE_Mini : MonoBehaviour // inherit from Boss Master and Monobehaviour
{
    //References
    GameObject _P, _B;
    P_Master _P_MasterRef;
    Boss_Master _BossMasterRef;
    Player_HealthBar _P_Health;

    [Header("Required Fields MUST FILL!!!")]
    [Tooltip("Reference... Player has 100, 100 Health. Anthing above 100 damage will automatically damage past the sheild into health.")]
    [SerializeField] int _AttackDamage = 10;
    [Tooltip("Small Attack: 8-12 frames, Medium Attack: 18-28 frames, Poise Break Heavy Attack 40-60 frames, Set to 0 for no stun")]
    [SerializeField] int _StunFrames = 10;
    public P_Master.P_StunSize AttackStunType;

    [Header("Optional Fields")]
    [SerializeField] GameObject _DestroyTarget;
    [SerializeField] int _AttackTick;
    void Awake()
    {
        _P = GameObject.FindGameObjectWithTag("Player");
        _B = GameObject.FindGameObjectWithTag("Boss");
        _P_MasterRef = _P.GetComponent<P_Master>();
        _P_Health = _P.GetComponent<Player_HealthBar>();
        _BossMasterRef = _B.GetComponent<Boss_Master>();
        _BossMasterRef.Turnspeed = 1000f; //set to whatever initial value you want
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (_P_MasterRef.Dodging_Invincible) return;
        if (_P_MasterRef.Parry_Invincible) return;
        //damage player
        _P_Health.DamagePlayerHealth(_AttackDamage);
        _P_MasterRef.P_StunInput(_StunFrames, AttackStunType);
    }
    void FixedUpdate()
    {
        //run any neessicary special code here
    }
    //put any public voids here to be triggered by animation events
    public void EndMiyazakiTime()
    {
        //_BossMasterRef.Turnspeed = 10; // example
    }
    public void EndofLife() // end of attack
    {
        //run reset code here if nessicary
        if (_DestroyTarget == null) Destroy(gameObject);
        else Destroy(_DestroyTarget);
    }
    public void ResetEverything()
    {
        //run resetcode here
        if (_DestroyTarget == null) Destroy(gameObject);
        else Destroy(_DestroyTarget);
    }
    void Update()
    {
        if (_BossMasterRef.Boss_Action == Boss_Master.Boss_Action_List.STUNNED || _BossMasterRef.Boss_Action == Boss_Master.Boss_Action_List.Opening)
        {
            ResetEverything();
        }
    }
}