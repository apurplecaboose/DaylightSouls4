using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPossibility : MonoBehaviour
{
    [SerializeField] List<ComboType> comboTypes = new List<ComboType>();
    [SerializeField] List<ComboType> comboHolder = new List<ComboType>();
    ComboType[] resultComboArray, resultComboArray1, resultComboArray2, resultComboArray3, resultComboArray4, resultComboArray5, resultComboArray6, resultComboArray7;
    [SerializeField] float sampleCapacity, lowerPercentage, newTotalAmount;
    [SerializeField] List<float> percentageList = new List<float>();
    [SerializeField] public int[] chosenTime;
    [SerializeField] float[] newAmountArray;
    [SerializeField] int[] comboOptionIndex;

    float newPercentage, originalAmount;
    public List<ComboType> ChosenComboArray = new List<ComboType>(8);
    public ComboType[][] resultComboArrayAllInOne;



    public float A, B, C;
    public enum ComboType
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        G = 6,
        H = 7,
        I = 8,
        J = 9,
        K = 10,
        L = 11,
        M = 12,
        N = 13,
        O = 14,
        P = 15,
        Q = 16,
        R = 17,
        S = 18,
        T = 19,
        U = 20,
        V = 21,
        W = 22,
        X = 23,
        Y = 24,
        Z = 25,

        B1__Slam_Attack = 0,
        B1__Punch_Attack = 1,
        B1__Death_by_Covid = 2,

        B2__Runny_nose = 0,
        B2__Headache = 1,
        B2__Death_by_Covid = 2

    }
    private void Awake()
    {
        chosenTime = new int[] {1,0,0,0,0,0,0,0,0,0 };
        newAmountArray = new float[10];
        resultComboArrayAllInOne = new ComboType[8][];
        comboOptionIndex = new int[3];
        resultComboArray = new ComboType[3];
        resultComboArray1 = new ComboType[3];
        resultComboArray2 = new ComboType[3];
        resultComboArray3 = new ComboType[3];
        resultComboArray4 = new ComboType[3];
        resultComboArray5 = new ComboType[3];
        resultComboArray6 = new ComboType[3];
        resultComboArray7 = new ComboType[3];

        //for (int i = 0; i < bossData.comboTypes.Count; i++)
        //{
        //    comboTypes.Add(bossData.comboTypes[i]);
        //}
        //for (int i = 0; i < bossData.possibility.Count; i++)
        //{
        //    percentageList.Add(bossData.possibility[i]);
        //}

        //for (int i = 0; i < bossData.comboTypes.Count; i++)
        //{
        //    AddingCombo(comboTypes[i], percentageList[i]);
        //}

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



    public void SetComboGroup(ComboType[] resultCombo)
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



    public void AddingCombo(ComboType combo, float percentage)
    {
        float amount = Mathf.Round(sampleCapacity * percentage);
        for (int i = 0; i < amount; i++)
        {
            comboHolder.Add(combo);
        }
    }


    public float LowerPossibility(int percentageIndex, int chosenTimes, ComboType combo)
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
