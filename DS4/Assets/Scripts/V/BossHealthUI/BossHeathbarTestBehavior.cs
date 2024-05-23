using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHeathbarTestBehavior : MonoBehaviour
{// written intended to be attactch to Boss Object
    //fix healthbar lerp, cant be a constant number and set a delay
    public int BossHp, PlayerDamage;
    float _StackCountDwonTimer;
    int _StackedDamage;
    [SerializeField] float _HealthDropDelay, _HealthDropDelayOriginal;
    SpriteRenderer _BossSR;
    [SerializeField]
    TMP_Text _BossHp;
    [SerializeField]
    Slider _BossHP_Slider, _DecayHP_Slider;
    [SerializeField]
    float  _Duration,_ElapsedTime,_T;
    
    public AnimationCurve LerpCurve;


    void Start()
    {
        _HealthDropDelayOriginal = _HealthDropDelay = 1;
        _BossHP_Slider.maxValue = _DecayHP_Slider.maxValue = BossHp;
        _BossSR = this.GetComponent<SpriteRenderer>();
        _Duration = 1;
    }
    void Update()
    {
        DamageStackDisplay();
        HealthDecayLerp();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DamageBoss(10);
        }
    }


    void DamageBoss(int Damage)
    {
        BossHp -= Damage;
        _BossHP_Slider.value = BossHp;
        _StackedDamage += Damage;
        _StackCountDwonTimer += FrameToSec(18f + 5f) + FrameToSec(500f / _StackedDamage);
        
    }
 
    void DamageStackDisplay()
    {
        if (_StackCountDwonTimer > 2f)
        {
            _BossHp.text = _StackedDamage.ToString();
            _StackCountDwonTimer -= 2 * Time.deltaTime;
        }
        else if (_StackCountDwonTimer > 0)
        {
            _BossHp.text = _StackedDamage.ToString();
            _StackCountDwonTimer -= Time.deltaTime;
        }
        else
        {
            _BossHp.text = "";
            _StackedDamage = 0;
            _StackCountDwonTimer = 0;

        }
    }
    void HealthDecayLerp()
    {
        if (_BossHP_Slider.value != _DecayHP_Slider.value)
        {
            _HealthDropDelay -= Time.deltaTime;
        }

        if (_HealthDropDelay <= 0)
        {
             _T = Mathf.Clamp01(_ElapsedTime / _Duration);
            _ElapsedTime += Time.deltaTime;
            _DecayHP_Slider.value = Mathf.Lerp(_DecayHP_Slider.value, _BossHP_Slider.value, LerpCurve.Evaluate(_T));
            Debug.Log(_DecayHP_Slider.value);
        }
        if (_HealthDropDelay <= -0.1&& _BossHP_Slider.value == _DecayHP_Slider.value)
        {
            _HealthDropDelay = _HealthDropDelayOriginal;
            _ElapsedTime = 0;
        }

        if ( _BossHP_Slider.value == _DecayHP_Slider.value)
        {
            _HealthDropDelay = _HealthDropDelayOriginal;
            _ElapsedTime = 0;
        }
    }

    

    float FrameToSec(float frames)
    {
        frames *= 1 / 60f;
        return frames;
    }
}
