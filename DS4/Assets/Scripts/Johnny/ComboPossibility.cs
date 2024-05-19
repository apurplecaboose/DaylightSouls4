
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ComboPossibility : MonoBehaviour
{
    [SerializeField] List<BossDataSc.ComboType> comboTypes = new List<BossDataSc.ComboType>();
    [SerializeField] List<BossDataSc.ComboType> comboHolder = new List<BossDataSc.ComboType>();
    [SerializeField] BossDataSc.ComboType[] resultComboArray, resultComboArray1, resultComboArray2, resultComboArray3, resultComboArray4, resultComboArray5, resultComboArray6, resultComboArray7;
    [SerializeField] float sampleCapacity, lowerPercentage, newTotalAmount;
    [SerializeField] List<float> percentageList = new List<float>();
    [SerializeField] public int[] chosenTime;
    [SerializeField] float[] newAmountArray;
    [SerializeField] int[] comboOptionIndex;

    SelectPatternPanel refToSelectPattern;
    float newPercentage, originalAmount;

    public BossDataSc.ComboType[][] resultComboArrayAllInOne;
    public BossDataSc bossData;



    public float A, B, C;

    private void Awake()
    {

        chosenTime = new int[] {0,0,0,0,0,0,0,0,0,0 };
        newAmountArray = new float[10];
        resultComboArrayAllInOne = new BossDataSc.ComboType[8][];
        comboOptionIndex = new int[3];
        resultComboArray = new BossDataSc.ComboType[3];
        resultComboArray1 = new BossDataSc.ComboType[3];
        resultComboArray2 = new BossDataSc.ComboType[3];
        resultComboArray3 = new BossDataSc.ComboType[3];
        resultComboArray4 = new BossDataSc.ComboType[3];
        resultComboArray5 = new BossDataSc.ComboType[3];
        resultComboArray6 = new BossDataSc.ComboType[3];
        resultComboArray7 = new BossDataSc.ComboType[3];

        for (int i = 0; i < bossData.comboTypes.Count; i++)
        {
            comboTypes.Add(bossData.comboTypes[i]);
        }
        for (int i = 0; i < bossData.possibility.Count; i++)
        {
            percentageList.Add(bossData.possibility[i]);
        }

        for (int i = 0; i < bossData.comboTypes.Count; i++)
        {
            AddingCombo(comboTypes[i], percentageList[i]);
        }

        SetComboGroup(resultComboArray);
        SetComboGroup(resultComboArray1);
        SetComboGroup(resultComboArray2);
        SetComboGroup(resultComboArray3);
        SetComboGroup(resultComboArray4);
        SetComboGroup(resultComboArray5);
        SetComboGroup(resultComboArray6);
        SetComboGroup(resultComboArray7);


        LoadingCombos();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetFinalCombo();
            comboHolder.Clear();
            for(int i =0; i < comboTypes.Count; i++)
            {
                newAmountArray[i] = LowerPossibility(i, chosenTime[i], comboTypes[i]);
            }

            print("this is new total amount:" + newTotalAmount);
            sampleCapacity = newTotalAmount;
            for (int i = 0; i < comboTypes.Count; i++)
            {
                percentageList[i] = Mathf.Round(newAmountArray[i] / sampleCapacity * 1000) / 1000;
            }
            newTotalAmount = 0;

            SetComboGroup(resultComboArray);
            SetComboGroup(resultComboArray1);
            SetComboGroup(resultComboArray2);
            SetComboGroup(resultComboArray3);
            SetComboGroup(resultComboArray4);
            SetComboGroup(resultComboArray5);
            SetComboGroup(resultComboArray6);
            SetComboGroup(resultComboArray7);


            LoadingCombos();
        }

    }

    // Update is called once per frame

    public void ShowAllValue()
    {
        for (int i = 0; i < comboHolder.Count; i++)
        {
            print(comboHolder[i]);
        }
    }



    public void SetComboGroup(BossDataSc.ComboType[] resultCombo)
    {
        for (int i = 0; i < comboOptionIndex.Length; i++)
        {
            comboOptionIndex[i] = Random.Range(0, comboHolder.Count);
        }
        for (int i = 0; i < resultCombo.Length; i++)
        {
            resultCombo[i] = comboHolder[comboOptionIndex[i]];
        }

    }


    public void LoadingCombos()
    {
        resultComboArrayAllInOne[0] = resultComboArray;
        resultComboArrayAllInOne[1] = resultComboArray1;
        resultComboArrayAllInOne[2] = resultComboArray2;
        resultComboArrayAllInOne[3] = resultComboArray3;
        resultComboArrayAllInOne[4] = resultComboArray4;
        resultComboArrayAllInOne[5] = resultComboArray5;
        resultComboArrayAllInOne[6] = resultComboArray6;
        resultComboArrayAllInOne[7] = resultComboArray7;
    }



    public void AddingCombo(BossDataSc.ComboType combo, float percentage)
    {
        float amount = Mathf.Round(sampleCapacity * percentage);
        for (int i = 0; i < amount; i++)
        {
            comboHolder.Add(combo);
        }
    }

    public void GetFinalCombo()
    {
        refToSelectPattern = UIManager.Instance.GetPanel<SelectPatternPanel>();
        int finalLength = refToSelectPattern.finalPattern.Count;
        for(int i =0; i < finalLength; i++)
        {
            for(int j = 0; j < comboTypes.Count; j++)
            {
                if(refToSelectPattern.finalPattern[i]== comboTypes[j])
                {
                    chosenTime[j]++;
                }
                
            }
        }
       
    }

    public float LowerPossibility(int percentageIndex, int chosenTimes, BossDataSc.ComboType combo)
    {
        originalAmount = sampleCapacity * percentageList[percentageIndex];
        for (int i = 0; i < chosenTimes; i++)
        {
            percentageList[percentageIndex] = percentageList[percentageIndex] * (1 - lowerPercentage / 100);
            print("This is percentage:" + percentageList[percentageIndex]);
        }//calculate percentage 
        float newAmount = Mathf.Round(sampleCapacity * percentageList[percentageIndex]);//calculate the amount
        for (int i = 0; i < newAmount; i++)
        {
            comboHolder.Add(combo);
        }
        newTotalAmount += newAmount;
        return newAmount;
    }
}
