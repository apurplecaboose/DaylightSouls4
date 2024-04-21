using System.Collections;
using System.Collections.Generic;
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

    public void AddComboToPool()
    {
        SettingAmount(comboA, comboB, comboC, percentageA, percentageB, percentageC);
    }

    public void ChooseCombo()
    {
        comboIndex = Random.Range(0, comboTypes.Count);
        print(comboTypes[comboIndex]);
        comboTypes.Remove(comboTypes[comboIndex]);//remove the chosen number from the pool

    }

    public void SettingAmount(ComboType comboA, ComboType comboB, ComboType comboC, float percentageA,float percentageB,float percentageC)
    {
        if (1000 * percentageA + 1000 * percentageB + 1000 * percentageC <=1000)//make the total amounts stays in 1000
        {
            for (int i = 0; i < 1000 * percentageA; i++)
            {
                comboTypes.Add(comboA);
                print(comboTypes[i]);
            }
            for (int i = 0; i < 1000 * percentageB; i++)
            {
                comboTypes.Add(comboB);
                print(comboTypes[i]);
            }
            for (int i = 0; i < 1000 * percentageC; i++)
            {
                comboTypes.Add(comboC);
                print(comboTypes[i]);
            }
        }
      

    }
}
