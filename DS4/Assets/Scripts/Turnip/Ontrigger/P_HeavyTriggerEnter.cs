using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_HeavyTriggerEnter : MonoBehaviour
{
    P_Master _Player;
    GameObject _B;
    Boss_Master _BossMasterRef;
    BossHeathbar _HeathbarRef;
    [SerializeField] int _HeavyAttackDamage;
    [SerializeField] int _HeavyAttackPoiseDamage;
    private void Awake()
    {
        _B = GameObject.FindGameObjectWithTag("Boss");
        _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_Master>();
        _BossMasterRef =_B.GetComponent<Boss_Master>();
        _HeathbarRef = _B.GetComponent<BossHeathbar>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Boss")) return;
        if (_Player.ChargeBonusDamage > 24) // 24 is the hold time if held longer it is a charge attack
        {
            //remap the bonus damage to a multiplier first vector is the input range second is the multiplier range
            float helddamagemultiplier = RemapValue(_Player.ChargeBonusDamage, new Vector2(24,150), new Vector2(0,2f));
            //multiply and round
            int chargedbonusPoiseDmg = Mathf.RoundToInt(_HeavyAttackPoiseDamage * helddamagemultiplier);
            _BossMasterRef.AddPoiseDamage(_HeavyAttackPoiseDamage + chargedbonusPoiseDmg);
            
            int chargedbonusAttackDmg = Mathf.RoundToInt(_HeavyAttackDamage * helddamagemultiplier);
            int damageRandomizere = Random.Range(-3, 10);
            _HeathbarRef.DamageBoss(_HeavyAttackDamage + chargedbonusAttackDmg + damageRandomizere);
        }
        else
        {
            int damageRandomizere = Random.Range(-3, 4);
            _BossMasterRef.AddPoiseDamage(_HeavyAttackPoiseDamage);
            _HeathbarRef.DamageBoss(_HeavyAttackDamage + damageRandomizere);
        }

        Destroy(this); //only one damage per swing
    }
    float RemapValue(float inputvalue, Vector2 inputrange, Vector2 targetrange)
    {
        float remapnormalize0to1 = Mathf.InverseLerp(inputrange.x, inputrange.y, inputvalue);

        return Mathf.Lerp(targetrange.x, targetrange.y, remapnormalize0to1);
    }
}

