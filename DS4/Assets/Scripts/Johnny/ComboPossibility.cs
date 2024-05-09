
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
    [SerializeField] int[] comboOptionIndex;

    float newPercentage, originalAmount;

    public BossDataSc.ComboType[][] resultComboArrayAllInOne;
    public BossDataSc bossData;



    public float A, B, C;

    private void Awake()
    {
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
            comboHolder.Clear();

            //ObtainNewComboPos(1);
            float newAmount = LowerPossibility(0, 1, comboTypes[0]);
            float newAmount1 = LowerPossibility(1, 0, comboTypes[1]);
            float newAmount2 = LowerPossibility(2, 0, comboTypes[2]);
            float newAmount3 = LowerPossibility(3, 0, comboTypes[3]);
            float newAmount4 = LowerPossibility(4, 0, comboTypes[4]);
            float newAmount5 = LowerPossibility(5, 0, comboTypes[5]);
            float newAmount6 = LowerPossibility(6, 0, comboTypes[6]);
            float newAmount7 = LowerPossibility(7, 0, comboTypes[7]);
            float newAmount8 = LowerPossibility(8, 0, comboTypes[8]);
            float newAmount9 = LowerPossibility(9, 0, comboTypes[9]);

            print("this is new total amount:" + newTotalAmount);
            sampleCapacity = newTotalAmount;
            percentageList[0] = Mathf.Round(newAmount / sampleCapacity * 1000) / 1000;
            percentageList[1] = Mathf.Round(newAmount1 / sampleCapacity * 1000) / 1000;
            percentageList[2] = Mathf.Round(newAmount2 / sampleCapacity * 1000) / 1000;
            percentageList[3] = Mathf.Round(newAmount3 / sampleCapacity * 1000) / 1000;
            percentageList[4] = Mathf.Round(newAmount4 / sampleCapacity * 1000) / 1000;
            percentageList[5] = Mathf.Round(newAmount5 / sampleCapacity * 1000) / 1000;
            percentageList[6] = Mathf.Round(newAmount6 / sampleCapacity * 1000) / 1000;
            percentageList[7] = Mathf.Round(newAmount7 / sampleCapacity * 1000) / 1000;
            percentageList[8] = Mathf.Round(newAmount8 / sampleCapacity * 1000) / 1000;
            percentageList[9] = Mathf.Round(newAmount9 / sampleCapacity * 1000) / 1000;
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

    //sampleCapacity = sampleCapacity-(originalAmount- newAmount);
    //float newPercentage=newAmount/sampleCapacity;    
    //return newPercentage;


    //float newAmountA, newAmountB, newAmountC;

    //newAmountA = A * (1 - lowerPercentage / 100);
    //A = Mathf.Round(newAmountA / (newAmountA + B + C)*100);
    //newAmountB = Mathf.Round(B / (newAmountA + B + C)*100);
    //newAmountC = Mathf.Round(C / (newAmountA + B + C)*100);
    //print(A + newAmountB + newAmountC);
    //B=newAmountB; C=newAmountC;
}
