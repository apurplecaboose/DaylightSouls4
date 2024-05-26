//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BossAttackSample : MonoBehaviour
//{
//    public int parryFrames = 30;
//    public int parryFrameCounter;
//    public int DamageFrames = 30;
//    public int DamageFrameCounter;

//    public bool isParry;
//    public bool isDamage;

//    public GameObject parryObject;
//    public GameObject damageObject;

//    void Start()
//    {
//        parryObject.SetActive(false);
//        damageObject.SetActive(false);
//    }

//    void FixedUpdate()
//    {
//        if (isParry)
//        {
//            parryFrameCounter += 1;
//            if (parryFrameCounter >= parryFrames)
//            {
//                parryObject.SetActive(false);
//                isParry = false;
//                parryFrameCounter = 0;
//            }
//        }
//        else if (isDamage)
//        {
//            DamageFrameCounter += 1;
//            if (DamageFrameCounter >= DamageFrames)
//            {
//                damageObject.SetActive(false);
//                isDamage = false;
//                DamageFrameCounter = 0;
//            }
//        }
//    }

//    public void StartParry()
//    {
//        isParry = true;
//        parryObject.SetActive(true);
//    }
//    public void StartDamage()
//    {
//        isDamage = true;
//        damageObject.SetActive(true);
//    }
//}
