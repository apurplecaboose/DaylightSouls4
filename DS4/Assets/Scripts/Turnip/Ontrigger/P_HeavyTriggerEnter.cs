using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_HeavyTriggerEnter : Boss_Master
{
    P_Master _Player;
    [SerializeField] int _HeavyAttackDamage = 35;
    [SerializeField] int _HeavyAttackPoiseDamage = 45;
    private void Awake()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_Master>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("BossHitbox")) return;
        
        if (_Player.ChargeBonusDamage > 0)
        {
            //remap the bonus damage to a multiplier first vector is the input range second is the multiplier range
            float helddamagemultiplier = RemapValue(_Player.ChargeBonusDamage, new Vector2(0,100), new Vector2(0,1.5f));
            //multiply and round
            int chargedbonusPoiseDmg = Mathf.RoundToInt(_HeavyAttackPoiseDamage * helddamagemultiplier);
            AddPoiseDamage(_HeavyAttackPoiseDamage + chargedbonusPoiseDmg);
            
            int chargedbonusAttackDmg = Mathf.RoundToInt(_HeavyAttackDamage * helddamagemultiplier);
            //add damage
        }
        else
        {
            AddPoiseDamage(_HeavyAttackPoiseDamage);
        }
    }
    float RemapValue(float inputvalue, Vector2 inputrange, Vector2 targetrange)
    {
        float remapnormalize0to1 = Mathf.InverseLerp(inputrange.x, inputrange.y, inputvalue);

        return Mathf.Lerp(targetrange.x, targetrange.y, remapnormalize0to1);
    }
}

