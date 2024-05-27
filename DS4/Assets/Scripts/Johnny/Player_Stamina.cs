using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Stamina : MonoBehaviour
{
    public float StamimaValue = 100;
    float _Max_Stamina;
    public Image StaminaImage;
    private void Awake()
    {
        _Max_Stamina = StamimaValue;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StaminaConsuming(10);
        }
        if (StamimaValue < _Max_Stamina)
        {
            RestoreStamina();
        }
    }
    public void RestoreStamina()
    {
        float restoreSpeed = 20;
        StamimaValue += restoreSpeed * Time.deltaTime;
        float t = Mathf.InverseLerp(0, _Max_Stamina, StamimaValue);
        StaminaImage.fillAmount = t;
    }
    public void StaminaConsuming(float consumeValue)
    {
        float currentValue = StamimaValue;
        currentValue -= consumeValue;
        float t = Mathf.InverseLerp(0, _Max_Stamina, currentValue);
        StaminaImage.fillAmount = t;
        StamimaValue = currentValue;
    }
}
