using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_AttackTrigger : Boss_Master
{
    P_Master _Player;
    [SerializeField] int _AttackDamage = 10;
    private void Awake()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_Master>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        //if(not invincible)
        //damage player
    }
}
