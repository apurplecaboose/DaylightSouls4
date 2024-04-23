using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ComboPossibility : MonoBehaviour
{
    [SerializeField] List<ComboType> comboTypes = new List<ComboType>();
    [SerializeField] ComboType comboA, comboB, comboC;
    [SerializeField] float percentageA, percentageB, percentageC,sampleCapacity;
    [SerializeField] int comboIndex;
    [SerializeField]
    public enum ComboType
    {
        A,B,C,D,E
    }
    private void Awake()
    {
        comboA = ComboType.A;
        comboB = ComboType.B;
        comboC = ComboType.C;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddComboAToPool()
    {
        SettingAmount(comboA, percentageA);
    }
    public void AddComboBToPool()
    {
        SettingAmount(comboB, percentageB);
    }
    public void AddComboCToPool()
    {
        SettingAmount(comboC, percentageC);
    }

    public void ShowAllValue()
    {
        for (int i = 0; i < comboTypes.Count; i++)
        {
            print(comboTypes[i]);
        }
    }

    public void ChooseCombo()
    {
        comboIndex = Random.Range(0, comboTypes.Count);
        print(comboTypes[comboIndex]);
        comboTypes.Remove(comboTypes[comboIndex]);//remove the chosen number from the pool

    }

    public void SettingAmount(ComboType combo, float percentage)
    {
        {
            for (int i = 0; i < 1000 * percentage; i++)
            {
                comboTypes.Add(combo);
            }
        }
      

    }
}
