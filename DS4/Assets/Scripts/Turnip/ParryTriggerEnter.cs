using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryTriggerEnter : Boss_Master 
{
    P_Master _Player;
    [SerializeField] int _BaseParryPoiseDamage = 225;
    private void Awake()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_Master>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_Player.ChargeBonusDamage > 0)
        {
            int chargedbonusPoiseDmg = Mathf.RoundToInt(_Player.ChargeBonusDamage * 0.2f);
            _Player.PogChampionParry(15);
            AddPoiseDamage(_BaseParryPoiseDamage + chargedbonusPoiseDmg);
        }
        else
        {
            _Player.PogChampionParry(15);
            AddPoiseDamage(_BaseParryPoiseDamage);
        }
    }
}