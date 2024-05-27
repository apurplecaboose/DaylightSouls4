using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HealthBar : MonoBehaviour
{
    public float HealthValue = 100, ShieldValue;
    float _ShieldOriginalValue, _HealthOriginalValue,_Max_Health,_Max_Shield;
    public float DamageTaken;
    public Image ShieldImage, HealthImage;
    public Animator Animator;
    private void Awake()
    {
        ShieldValue = HealthValue;
        _ShieldOriginalValue = ShieldValue;
        _HealthOriginalValue = HealthValue;
        _Max_Health = HealthValue;
        _Max_Shield = ShieldValue;
    }
    private void LateUpdate()
    {
        if (_PressedHealThisFrame) Animator.SetBool("IsHealing", true);
        else Animator.SetBool("IsHealing", false);
        _PressedHealThisFrame = false;
    }
    bool _PressedHealThisFrame;
    public void RestoreHealth()
    {
        _PressedHealThisFrame = true;
        float restoreSpeed = 10;
        ShieldValue = Mathf.Clamp(ShieldValue, 0, HealthValue);
        ShieldValue += restoreSpeed * Time.deltaTime;
        float t = Mathf.InverseLerp(0, _ShieldOriginalValue, ShieldValue);
        ShieldImage.fillAmount = t;
    }

    public void RestoreParry()
    {
        _PressedHealThisFrame = true;
        HealthValue += 10f;
        ShieldValue += 15f;
        ShieldValue = Mathf.Clamp(ShieldValue, 0, 100);
        HealthValue = Mathf.Clamp(ShieldValue, 0, 100);
        float t = Mathf.InverseLerp(0, _Max_Shield, ShieldValue);
        float k= Mathf.InverseLerp(0, _Max_Health, HealthValue);
        ShieldImage.fillAmount = t;
        HealthImage.fillAmount = k;
        Animator.SetBool("IsHealing", true);
    }
    public void DamagePlayerHealth(float Damage)
    {
        ShieldDamageTaken(Damage);
        if (ShieldImage.fillAmount == 0 && HealthImage.fillAmount > 0)
        {
            HealthDamageTaken(Damage);
        }
    }
    void ShieldDamageTaken(float damage)
    {
        float currentValue = ShieldValue;
        currentValue -= damage;
        float t = Mathf.InverseLerp(0, _ShieldOriginalValue, currentValue);
        ShieldImage.fillAmount = t;
        ShieldValue = currentValue;
    }
    void HealthDamageTaken(float damage)
    {
        float currentValue = HealthValue;
        currentValue -= damage;
        float t = Mathf.InverseLerp(0, _HealthOriginalValue, currentValue);
        HealthImage.fillAmount = t;
        HealthValue = currentValue;
    }

}
