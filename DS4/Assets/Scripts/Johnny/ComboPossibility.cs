
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Burst.Intrinsics;
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
    [SerializeField] int _BossNumber;
    SelectPatternPanel _RefToSelectPattern;
    public List<ComboType> ChosenComboFromKen = new List<ComboType>(8);

    public ComboType[][] ResultComboArrayAllInOne;

    Dictionary<ComboType, float> _ComboWithPercentageDictionary = new Dictionary<ComboType, float>();


    public enum ComboType
    {
        B1__Slam_Attack = 0,
        B1__Punch_Attack = 1,
        B1__Death_by_Covid = 2,
        B1__GomoGomo_Attack = 3,
        B1__StripLife_Attack = 4,
        B1__Ganster_Attack = 5,
        B1__Italian_Attack = 6,
        B1__JesusBless_Attack = 7,
        B1__RockNRoll_Attack = 8,
        B1__RapGod_Attack = 9,

        B2__Slam_Attack = 0,
        B2__Punch_Attack = 1,
        B2__Death_by_Covid = 2,
        B2__GomoGomo_Attack = 3,
        B2__StripLife_Attack = 4,
        B2__Ganster_Attack = 5,
        B2__Italian_Attack = 6,
        B2__JesusBless_Attack = 7,
        B2__RockNRoll_Attack = 8,
        B2__RapGod_Attack = 9,

        //B2__Runny_nose = 0,
        //B2__Headache = 1,
        //B2__Death_by_Covid = 2
        B999__PLACEHOLDER_FOR_KENS_CODE

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
        ChosenComboFromKen = new List<ComboType>();
        _AmountAfterDecrease = new float[10];
        ResultComboArrayAllInOne = new ComboType[8][];
        _ComboOptionIndex = new int[3];

        LoadInArraysToFinalPatern();

        ///
        _ComboWithPercentageDictionary.Add(ComboType.B1__Slam_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B1__Punch_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B1__Death_by_Covid, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B1__GomoGomo_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B1__StripLife_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B1__Ganster_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B1__Italian_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B1__JesusBless_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B1__RockNRoll_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B1__RapGod_Attack, 0.1f);

        
        _ComboWithPercentageDictionary.Add(ComboType.B2__Slam_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B2__Punch_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B2__Death_by_Covid, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B2__GomoGomo_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B2__StripLife_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B2__Ganster_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B2__Italian_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B2__JesusBless_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B2__RockNRoll_Attack, 0.1f);
        _ComboWithPercentageDictionary.Add(ComboType.B2__RapGod_Attack, 0.1f);

        foreach (ComboType key in _ComboWithPercentageDictionary.Keys)
        {
            float percentage = _ComboWithPercentageDictionary[key];
            _PercentageList.Add(percentage);
        }// load in the initial value for percentage
        ///

        foreach (ComboType combo in Enum.GetValues(typeof(ComboType)))
        {
            int currentbossnumber = ExtractBossNumberFromInput(combo.ToString());
            if (currentbossnumber == _BossNumber)
            {
                //Debug.Log("SUCESS " + BossEnumStringCleanup(combo.ToString()) + " is in Boss " + _BossNumber + " moveset");
                _ComboTypes.Add(combo);
            }
            else { }//Debug.Log("FAIL " + BossEnumStringCleanup(combo.ToString()) + " is not in Boss " + _BossNumber + " moveset");
        }

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
    public int ExtractBossNumberFromInput(string inputString)
    {
        Match match = Regex.Match(inputString, @"B(\d+)");
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }
        else
        {
            Debug.LogError("null");
            return -1;
        }
    }

    public string BossEnumStringCleanup(string inputString)
    {
        string cleanedString = Regex.Replace(inputString, @"^[^_]*__", "");
        cleanedString = cleanedString.Replace("_", " ");
        return cleanedString;
    }



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
            _ComboOptionIndex[i] = UnityEngine.Random.Range(0, _ComboHolder.Count);
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
        ChosenComboFromKen = _RefToSelectPattern.FinalPattern;

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
