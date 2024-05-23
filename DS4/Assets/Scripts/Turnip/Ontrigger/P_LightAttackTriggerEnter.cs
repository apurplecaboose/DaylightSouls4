using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_LightAttackTriggerEnter : MonoBehaviour
{
    [SerializeField] Boss_Master _BossMasterRef;
    [SerializeField] int _LightAttackDamage = 10;
    [SerializeField] int _LightAttackPoiseDamage = 10;
    private void Awake()
    {
        _BossMasterRef= GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss_Master>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Boss")) return;
        _BossMasterRef.AddPoiseDamage(_LightAttackPoiseDamage);
        //damage boss _LightAttackDamage
    }
}
