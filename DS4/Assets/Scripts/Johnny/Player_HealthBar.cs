using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HealthBar : MonoBehaviour
{
    public float HealthValue = 100, ShieldValue;
    float _ShieldOriginalValue, _HealthOriginalValue;
    public float DamageTaken;
    public Image ShieldImage, HealthImage;
    private void Awake()
    {
        ShieldValue = HealthValue;
        _ShieldOriginalValue = ShieldValue;
        _HealthOriginalValue = HealthValue;
    }
    public void RestoreHealth()
    {
        float restoreSpeed = 10;
        ShieldValue = Mathf.Clamp(ShieldValue, 0, HealthValue);
        ShieldValue += restoreSpeed * Time.deltaTime;
        float t = Mathf.InverseLerp(0, _ShieldOriginalValue, ShieldValue);
        ShieldImage.fillAmount = t;
    }
    public void DamageLogic(float Damage)
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
