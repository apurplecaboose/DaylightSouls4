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
    float _stackCountDwonTimer;
    int _stackedDamage;
    [SerializeField] float _healthDropDelay, _healthDropDelayOriginal;
    SpriteRenderer _bossSR;
    [SerializeField]
    TMP_Text _bossHp;
    [SerializeField]
    Slider _bossHP_Slider, _decayHP_Slider;
    [SerializeField]
    float  duration,elapsedTime,t;
    
    public AnimationCurve LerpCurve;


    void Start()
    {
        _healthDropDelayOriginal = _healthDropDelay = 1;
        _bossHP_Slider.maxValue = _decayHP_Slider.maxValue = BossHp;
        _bossSR = this.GetComponent<SpriteRenderer>();
        duration = 1;
    }
    void Update()
    {
        DamageStackDisplay();
        HealthDecayLerp();
    }
    private void OnTriggerEnter2D(Collider2D bossCollider)
    {
        if (bossCollider.CompareTag("LightAttack"))
        {
            Debug.Log("collided");
            BossHp -= PlayerDamage;
            _bossSR.color = Color.red;
            _bossHP_Slider.value = BossHp;

            _stackedDamage += PlayerDamage;
            _stackCountDwonTimer += FrameToSec(18f + 5f) + FrameToSec(500f / _stackedDamage);
        }
        if (bossCollider.CompareTag("HeavyAttack"))
        {
            BossHp -= PlayerDamage * 2;
            _bossSR.color = Color.red;
            _bossHP_Slider.value = BossHp;

            _stackedDamage += PlayerDamage;
            _stackCountDwonTimer += FrameToSec(18f + 5f) + FrameToSec(500f / _stackedDamage);
        }
    }

    private void OnTriggerExit2D(Collider2D bossCollider)
    {
        if (bossCollider.CompareTag("LightAttack"))
        {
            _bossSR.color = Color.white;
        }
        if (bossCollider.CompareTag("HeavyAttack"))
        {
            _bossSR.color = Color.white;
        }
    }
    void DamageStackDisplay()
    {
        if (_stackCountDwonTimer > 2f)
        {
            _bossHp.text = _stackedDamage.ToString();
            _stackCountDwonTimer -= 2 * Time.deltaTime;
        }
        else if (_stackCountDwonTimer > 0)
        {
            _bossHp.text = _stackedDamage.ToString();
            _stackCountDwonTimer -= Time.deltaTime;
        }
        else
        {
            _bossHp.text = "";
            _stackedDamage = 0;
            _stackCountDwonTimer = 0;

        }
    }
    void HealthDecayLerp()
    {
        

        if (_bossHP_Slider.value != _decayHP_Slider.value)
        {
            _healthDropDelay -= Time.deltaTime;
        }

        if (_healthDropDelay <= 0)
        {
             t = Mathf.Clamp01(elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            _decayHP_Slider.value = Mathf.Lerp(_decayHP_Slider.value, _bossHP_Slider.value, LerpCurve.Evaluate(t));
            Debug.Log(_decayHP_Slider.value);
        }
        if ( _bossHP_Slider.value == _decayHP_Slider.value)
        {
            _healthDropDelay = _healthDropDelayOriginal;
            elapsedTime = 0;
        }
    }

    

    float FrameToSec(float frames)
    {
        frames *= 1 / 60f;
        return frames;
    }
}
