using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttack : MonoBehaviour
{
    public GameObject DamageArea;
    public GameObject Pivot;
    void Awake()
    {
        InactivateDamageArea();
    }
    void Start()
    {

        //this.transform.up = this.gameObject.transform.up;
    }
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        
    }

    public void ActivateDamageArea()
    {
        DamageArea.SetActive(true);
    }
    public void InactivateDamageArea()
    {
        DamageArea.SetActive(false);
    }
    public void DestroyMyself()
    {
        Destroy(Pivot);
    }

}
