using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_ParryTriggerEnter : MonoBehaviour 
{
    P_Master _Player;
    Boss_Master _BossMasterRef;
    [SerializeField] int _BaseParryPoiseDamage;
    private void Awake()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_Master>();
        _BossMasterRef = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss_Master>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("BossParry")) return;
        Destroy(_BossMasterRef.CurrentAttackMini);
        Destroy(_BossMasterRef.LastAttackMini);
        if (_Player.ChargeBonusDamage > 0)
        {
            //remap the bonus damage to a multiplier first vector is the input range second is the multiplier range
            float helddamagemultiplier = RemapValue(_Player.ChargeBonusDamage, new Vector2(0, 125), new Vector2(0, 1.25f));
            //multiply and round
            int chargedbonusPoiseDmg = Mathf.RoundToInt(_BaseParryPoiseDamage * helddamagemultiplier);
            _BossMasterRef.AddPoiseDamage(_BaseParryPoiseDamage + chargedbonusPoiseDmg);
            _Player.PogChampionParry(30); // give the player i frames after parry
            collision.enabled = false;
        }
        else
        {
            _Player.PogChampionParry(30);
            _BossMasterRef.AddPoiseDamage(_BaseParryPoiseDamage);
            collision.enabled = false;
        }
        Destroy(this); //turn off parry collider once parried only 1 parry per swing
    }
    float RemapValue(float inputvalue, Vector2 inputrange, Vector2 targetrange)
    {
        float remapnormalize0to1 = Mathf.InverseLerp(inputrange.x, inputrange.y, inputvalue);

        return Mathf.Lerp(targetrange.x, targetrange.y, remapnormalize0to1);
    }
}