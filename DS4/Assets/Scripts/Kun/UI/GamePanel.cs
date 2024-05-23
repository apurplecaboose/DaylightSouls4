using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Transform backBar;
    public Image frontBar;
    private float orignalMaxHealth = 100;
    [SerializeField]
    private float maxHealth;

    public float MaxHealth { get { return maxHealth; } set { maxHealth = Mathf.Clamp(value, 0, orignalMaxHealth); } }
    [SerializeField]
    private float health;
    public float Health { get { return health; } set { health = Mathf.Clamp(value, 0, MaxHealth); } }

    public Button btnHurt;

    public float reduce;
    public float add;

    public float hurtValue = 30;

    public override void Init()
    {
        maxHealth = orignalMaxHealth;
        health = maxHealth;
        btnHurt.onClick.AddListener(() =>
        {
            Hurt(hurtValue);
        });
    }

    protected override void Update()
    {
        base.Update();
        backBar.localScale = new Vector3(MaxHealth / orignalMaxHealth, backBar.localScale.y, backBar.localScale.z);
        frontBar.fillAmount = Health / MaxHealth;

        if (true)
        {

        }



        
    }
    private void FixedUpdate()
    {
        Recover();
    }
    void Recover()
    {
        Health += 30 * Time.fixedDeltaTime;
    }

    void Hurt(float hurtValue)
    {
        if (Health > 0)
        {
            if (Health>= hurtValue)
            {
                Health -= hurtValue;
            }
            else if(Health < hurtValue)
            {
                float overflow = hurtValue - Health;
                Health = 0;
                MaxHealth -= overflow;
            }
            
        }
        else
        {
            MaxHealth -= hurtValue;
        }
    }
}
