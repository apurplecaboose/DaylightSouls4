using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_LightAttackTriggerEnter : Boss_Master
{
    P_Master _Player;
    [SerializeField] int _LightAttackDamage = 10;
    [SerializeField] int _LightAttackPoiseDamage = 10;
    private void Awake()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_Master>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("BossHitbox")) return;
        AddPoiseDamage(_LightAttackPoiseDamage);
        //damage boss _LightAttackDamage
    }
}
