
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ComboPossibility : MonoBehaviour
{
    [SerializeField] List<ComboType> _ComboTypes = new List<ComboType>();
    [SerializeField] List<ComboType> _ComboHolder = new List<ComboType>();
    [SerializeField] ComboType[] _ResultComboArray, _ResultComboArray1, _ResultComboArray2, _ResultComboArray3, _ResultComboArray4, _ResultComboArray5, _ResultComboArray6, _ResultComboArray7;
    [SerializeField] float _SampleCapacity, _LowerPercentage, _NewTotalAmount;
    [SerializeField] List<float> _PercentageList= new List<float>(10);
    [SerializeField] public int[] ChosenTime;
    [SerializeField] float[] _AmountAfterDecrease;
    [SerializeField] int[] _ComboOptionIndex;

    SelectPatternPanel _RefToSelectPattern;
    public List<ComboType> ChosenCombo = new List<ComboType>(8);

    public ComboType[][] ResultComboArrayAllInOne;
    public BossDataSc BossData;


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


        //B1__Slam_Attack = 0,
        //B1__Punch_Attack = 1,
        //B1__Death_by_Covid = 2,

        //B2__Runny_nose = 0,
        //B2__Headache = 1,
        //B2__Death_by_Covid = 2

    }
    // convert combo name into its relative boss combo scriptable object





    


    private void Awake()
    {
        _ResultComboArray = new ComboType[3];
        _ResultComboArray1 = new ComboType[3];
        _ResultComboArray2 = new ComboType[3];
        _ResultComboArray3 = new ComboType[3];
        _ResultComboArray4 = new ComboType[3];
        _ResultComboArray5 = new ComboType[3];
        _ResultComboArray6 = new ComboType[3];
        _ResultComboArray7 = new ComboType[3];
        ChosenTime = new int[10];
        ChosenCombo = new List<ComboType>();
        _AmountAfterDecrease = new float[10];
        ResultComboArrayAllInOne = new ComboType[8][];
        _ComboOptionIndex = new int[3];

        LoadInArraysToFinalPatern();

        for (int i = 0; i < _ComboTypes.Count; i++)
        {
            AddingCombo(_ComboTypes[i], _PercentageList[i]);
        }//combine the possibility and the attack to. Fill up the holder according to their persontage


        for (int i = 0;i< ResultComboArrayAllInOne.Length; i++)
        {
            SetComboGroup(ResultComboArrayAllInOne[i]);
        }//provide the 3 randomm value as a grounp.

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetFinalCombo();
            _ComboHolder.Clear();
            for(int i =0; i < _ComboTypes.Count; i++)
            {
                _AmountAfterDecrease[i] = LowerPossibility(i, ChosenTime[i], _ComboTypes[i]);
            }

            print("this is new total amount:" + _NewTotalAmount);
            _SampleCapacity = _NewTotalAmount;
            for (int i = 0; i < _ComboTypes.Count; i++)
            {
                _PercentageList[i] = Mathf.Round(_AmountAfterDecrease[i] / _SampleCapacity * 1000) / 1000;
            }//recalculate the percentage for each attack
            _NewTotalAmount = 0;//prepare for the next decrease personality

            for (int i = 0; i < ResultComboArrayAllInOne.Length; i++)
            {
                SetComboGroup(ResultComboArrayAllInOne[i]);
            }//provide the 3 randomm value as a grounp.

            LoadInArraysToFinalPatern();
        }

    }

    // Update is called once per frame

    public void ShowAllValue()
    {
        for (int i = 0; i < _ComboHolder.Count; i++)
        {
            print(_ComboHolder[i]);
        }
    }



    public void SetComboGroup(ComboType[] resultCombo)
    {
        print(resultCombo.Length);
        for (int i = 0; i < _ComboOptionIndex.Length; i++)
        {
            _ComboOptionIndex[i] = Random.Range(0, _ComboHolder.Count);
        }
        for (int i = 0; i < resultCombo.Length; i++)
        {
            resultCombo[i] = _ComboHolder[_ComboOptionIndex[i]];
        }

    }


    public void LoadInArraysToFinalPatern()
    {
        ResultComboArrayAllInOne[0] = _ResultComboArray;
        ResultComboArrayAllInOne[1] = _ResultComboArray1;
        ResultComboArrayAllInOne[2] = _ResultComboArray2;
        ResultComboArrayAllInOne[3] = _ResultComboArray3;
        ResultComboArrayAllInOne[4] = _ResultComboArray4;
        ResultComboArrayAllInOne[5] = _ResultComboArray5;
        ResultComboArrayAllInOne[6] = _ResultComboArray6;
        ResultComboArrayAllInOne[7] = _ResultComboArray7;
    }



    public void AddingCombo(ComboType combo, float percentage)
    {
        float amount = Mathf.Round(_SampleCapacity * percentage);
        for (int i = 0; i < amount; i++)
        {
            _ComboHolder.Add(combo);
        }
    }

    public void GetFinalCombo()
    {
        _RefToSelectPattern = UIManager.Instance.GetPanel<SelectPatternPanel>();
        int finalLength = _RefToSelectPattern.FinalPattern.Count;
        ChosenCombo = _RefToSelectPattern.FinalPattern;
        for (int i = 0; i < finalLength; i++)
        {
            for (int j = 0; j < _ComboTypes.Count; j++)
            {
                if (_RefToSelectPattern.FinalPattern[i] == _ComboTypes[j])
                {
                    ChosenTime[j]++;
                }

            }
        }

    }
    // Need Adjustment with Ken Script

    public float LowerPossibility(int percentageIndex, int chosenTimes, ComboType combo)
    {
        for (int i = 0; i < chosenTimes; i++)
        {
            _PercentageList[percentageIndex] = _PercentageList[percentageIndex] * (1 - _LowerPercentage / 100);
            print("This is percentage:" + _PercentageList[percentageIndex]);
        }//calculate percentage 
        float newAmount = Mathf.Round(_SampleCapacity * _PercentageList[percentageIndex]);//calculate the amount
        for (int i = 0; i < newAmount; i++)
        {
            _ComboHolder.Add(combo);//add the certain amount of certain attack into the combo holder
        }
        _NewTotalAmount += newAmount;//recalculate the capacity
        return newAmount;
    }
}
