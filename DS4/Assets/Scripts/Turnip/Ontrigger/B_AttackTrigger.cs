using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_AttackTrigger : MonoBehaviour
{
    GameObject _P;
    P_Master _P_MasterRef;
    Player_HealthBar _P_Health;
    [SerializeField] int _AttackDamage = 10;
    [Tooltip("Small Attack: x-y frames, Medium Attack: x-y frames, Poise Break Heavy Attack x-y frames")]
    [SerializeField] int _StunFrames = 10;
    public P_Master.P_StunSize AttackStunType;
    bool _StartDamageTimer;
    int _TickTimer, _WaitforDamageinTicks = 3;
    void Awake()
    {
        _P = GameObject.FindGameObjectWithTag("Player");
        _P_MasterRef = _P.GetComponent<P_Master>();
        _P_Health = _P.GetComponent<Player_HealthBar>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if(_P_MasterRef.Dodging_Invincible) return;
        if (_P_MasterRef.Parry_Invincible) return;
        //damage player
        _StartDamageTimer = true;
    }
    private void FixedUpdate()
    {
        if (!_StartDamageTimer) return;
        _TickTimer += 1;

        if (_TickTimer >= 10)
        {
            _TickTimer -= 1;
            _P_Health.DamagePlayerHealth(_AttackDamage);
            _P_MasterRef.P_StunInput(_StunFrames, AttackStunType);
        }
    }
}
