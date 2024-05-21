using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public GameObject LightAttackPrefab;

    public GameObject HeavyAttackPrefab;
    void Awake()
    {
        
    }
    void Start()
    {
        
    }
    void Update()
    {
        LightAttackTest();
        HeavyAttackTest();
    }
    void FixedUpdate()
    {
        
    }
    void LightAttackTest()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject gb = Instantiate(LightAttackPrefab,transform.position,Quaternion.identity);
            gb.transform.up = this.gameObject.transform.up;
        }
    }
    void HeavyAttackTest()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameObject gb = Instantiate(HeavyAttackPrefab, transform.position, Quaternion.identity);
            gb.transform.up = this.gameObject.transform.up;
        }
    }
}
