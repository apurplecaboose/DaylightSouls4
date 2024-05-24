using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_LightAttackTriggerEnter : MonoBehaviour
{
    GameObject _B;
    [SerializeField] Boss_Master _BossMasterRef;
    [SerializeField] BossHeathbar _HeathbarRef;
    [SerializeField] int _LightAttackDamage = 10;
    [SerializeField] int _LightAttackPoiseDamage = 10;
    private void Awake()
    {
        _B = GameObject.FindGameObjectWithTag("Boss");
        _BossMasterRef = _B.GetComponent<Boss_Master>();
        _HeathbarRef = _B.GetComponent<BossHeathbar>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int damageRandomizere = Random.Range(-2, 2);
        if (!collision.CompareTag("Boss")) return;
        _BossMasterRef.AddPoiseDamage(_LightAttackPoiseDamage);
        _HeathbarRef.DamageBoss(_LightAttackDamage + damageRandomizere);
    }
}
