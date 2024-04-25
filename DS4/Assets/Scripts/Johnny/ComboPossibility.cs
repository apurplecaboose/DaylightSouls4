
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPossibility : MonoBehaviour
{
    [SerializeField] List<BossDataSc.ComboType> comboTypes = new List<BossDataSc.ComboType>();
    [SerializeField] List<BossDataSc.ComboType> comboHolder = new List<BossDataSc.ComboType>();
    [SerializeField] float sampleCapacity;
    [SerializeField] List<float> percentageList = new List<float>();
    [SerializeField] int comboIndex;
    public BossDataSc bossData;
    private void Awake()
    {
        for (int i = 0; i < bossData.comboTypes.Count; i++)
        {
            comboTypes.Add(bossData.comboTypes[i]);
            print(comboTypes[i]);
        }
        for (int i = 0; i < bossData.possibility.Count; i++)
        {
            percentageList.Add(bossData.possibility[i]);
            print(percentageList[i]);
        }
        for (int i = 0; i < comboTypes.Count; i++)
        {
            SettingAmount(comboTypes[i], percentageList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    //public void AddComboAToPool()
    //{
    //    SettingAmount(comboA, percentageA);
    //}
    //public void AddComboBToPool()
    //{
    //    SettingAmount(comboB, percentageB);
    //}
    //public void AddComboCToPool()
    //{
    //    SettingAmount(comboC, percentageC);
    //}

    public void ShowAllValue()
    {
        for (int i = 0; i < comboHolder.Count; i++)
        {
            print(comboHolder[i]);
        }
    }

    public void ChooseCombo()
    {
        comboIndex = Random.Range(0, comboTypes.Count);
        print(comboTypes[comboIndex]);
        //comboTypes.Remove(comboTypes[comboIndex]);//remove the chosen number from the pool
        ////number of a's / length = a percentage
        ////for 
        //// if(combotypes[i] == A)
        //// int A number ++;

    }

    public void SettingAmount(BossDataSc.ComboType combo, float percentage)
    {
        for (int i = 0; i < 100 * percentage; i++)
        {
            comboHolder.Add(combo);
            print("SHit");
        }
    }
}
