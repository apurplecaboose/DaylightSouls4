
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
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

        for(int i = 0;i < bossData.comboTypes.Count; i++)
        {
            AddingCombo(comboTypes[i], percentageList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowAllValue()
    {
        for (int i = 0; i < comboHolder.Count; i++)
        {
            print(comboHolder[i]);
        }
    }

    public void ChooseCombo()
    {
        comboIndex = Random.Range(0, comboHolder.Count);
        print(comboHolder[comboIndex]);
    }

    public void AddingCombo(BossDataSc.ComboType combo, float percentage)
    {
        percentage = percentage * 100;//calculated before for loop
        for (int i = 0; i < percentage; i++)
        {
            comboHolder.Add(combo);
        }
    }

}
