
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class ComboPossibility : MonoBehaviour
{
    [Header ("MODIFY")]
    [SerializeField] int _BossNumber;
    [SerializeField] List<float> _ComboDropPercentage;
    [SerializeField] float _SampleCapacity, _LowerPercentage, _NewTotalAmount;

    [Header("DO NOT TOUCH")]
    [SerializeField] List<ComboType> _CurrentBossCombos = new List<ComboType>();
    [SerializeField] List<ComboType> _ComboPool = new List<ComboType>();
    [SerializeField] public int[] NumberOfComboSelectionRepeats;
    [SerializeField] float[] _AmountAfterDecrease;
    [SerializeField] int[] _ComboOptionIndex;
    ComboType[] _ResultComboArray, _ResultComboArray1, _ResultComboArray2, _ResultComboArray3, _ResultComboArray4, _ResultComboArray5, _ResultComboArray6, _ResultComboArray7;
    [HideInInspector] public ComboType[][] ResultComboArrayAllInOne;

    [Header("OUTPUT: DO NOT TOUCH")]
    public List<ComboType> FinalOutputComboArray = new List<ComboType>(8); // DONT TOUCH

    //SelectPatternPanel _RefToSelectPattern;
    public enum ComboType
    {
        B1__Slam_Attack,//0
        B1__Punch_Attack,//1
        B1__Death_by_Covid,//2
        B1__GomoGomo_Attack,//3
        B1__StripLife_Attack,//4
        B1__Ganster_Attack,//5
        B1__Italian_Attack,//6
        B1__JesusBless_Attack,//7
        B1__RockNRoll_Attack,//8
        B1__RapGod_Attack,//9

        B2__Slam_Attack,//0
        B2__Punch_Attack,//1
        B2__Death_by_Covid,//2
        B2__GomoGomo_Attack,//3
        B2__StripLife_Attack,//4
        B2__Ganster_Attack,//5
        B2__Italian_Attack,//6
        B2__JesusBless_Attack,//7
        B2__RockNRoll_Attack,//8
        B2__RapGod_Attack,//9

        B999__PLACEHOLDER_FOR_KENS_CODE
    }

    private void Awake()
    {
        //select emuns that pertain to this boss number
        for (int i = 0; i < Enum.GetNames(typeof(ComboType)).Length; i++)
        {
            ComboType comboName = (ComboType)i;
            int bossnumber = ExtractBossNumberFromInput(comboName.ToString());
            if(bossnumber == _BossNumber)
            {
                _CurrentBossCombos.Add(comboName);
            }
        }
        //checks if drop percentage sum is 100 
        if(_ComboDropPercentage.Sum() != 1)
        {
            Debug.LogError("Drop percentages do not add up to 100 percent");
            return;
        }

        _ResultComboArray = new ComboType[3];
        _ResultComboArray1 = new ComboType[3];
        _ResultComboArray2 = new ComboType[3];
        _ResultComboArray3 = new ComboType[3];
        _ResultComboArray4 = new ComboType[3];
        _ResultComboArray5 = new ComboType[3];
        _ResultComboArray6 = new ComboType[3];
        _ResultComboArray7 = new ComboType[3];
        ResultComboArrayAllInOne = new ComboType[8][];
        FinalOutputComboArray = new List<ComboType>();
        _ComboOptionIndex = new int[3];
        NumberOfComboSelectionRepeats = new int[10];
        _AmountAfterDecrease = new float[10];

        LoadInArraysToFinalPatern();

        //Fill the pool with combos according to their percentage
        for (int i = 0; i < _CurrentBossCombos.Count; i++)
        {
            AddingCombo(_CurrentBossCombos[i], _ComboDropPercentage[i]);
        }


        for (int i = 0;i< ResultComboArrayAllInOne.Length; i++)
        {
            SetComboGroup(ResultComboArrayAllInOne[i]);
        }//provide the 3 randomm value as a grounp.
    }

    private void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GetFinalCombo();
        //    _ComboPool.Clear();
        //    for(int i =0; i < _CurrentBossCombos.Count; i++)
        //    {
        //        _AmountAfterDecrease[i] = LowerPossibility(i, NumberOfComboSelectionRepeats[i], _CurrentBossCombos[i]);
        //    }

        //    print("this is new total amount:" + _NewTotalAmount);
        //    _SampleCapacity = _NewTotalAmount;
        //    for (int i = 0; i < _CurrentBossCombos.Count; i++)
        //    {
        //        _ComboDropPercentage[i] = Mathf.Round(_AmountAfterDecrease[i] / _SampleCapacity * 1000) / 1000;
        //    }//recalculate the percentage for each attack
        //    _NewTotalAmount = 0;//prepare for the next decrease personality

        //    for (int i = 0; i < ResultComboArrayAllInOne.Length; i++)
        //    {
        //        SetComboGroup(ResultComboArrayAllInOne[i]);
        //    }//provide the 3 randomm value as a grounp.

        //    LoadInArraysToFinalPatern();
        //}

    }

    int ExtractBossNumberFromInput(string inputString)
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
    public void ShowAllValue()
    {
        for (int i = 0; i < _ComboPool.Count; i++)
        {
            print(_ComboPool[i]);
        }
    }

    public void SetComboGroup(ComboType[] resultCombo)
    {
        //print(resultCombo.Length);
        for (int i = 0; i < _ComboOptionIndex.Length; i++)
        {
            _ComboOptionIndex[i] = UnityEngine.Random.Range(0, _ComboPool.Count);
        }
        for (int i = 0; i < resultCombo.Length; i++)
        {
            resultCombo[i] = _ComboPool[_ComboOptionIndex[i]];
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
            _ComboPool.Add(combo);
        }
    }

    //public void GetFinalCombo()
    //{
    //    _RefToSelectPattern = UIManager.Instance.GetPanel<SelectPatternPanel>();
    //    int finalLength = _RefToSelectPattern.FinalPattern.Count;
    //    ChosenComboFromKen = _RefToSelectPattern.FinalPattern;

    //    for (int i = 0; i < finalLength; i++)
    //    {
    //        for (int j = 0; j < _CurrentBossCombos.Count; j++)
    //        {
    //            if (_RefToSelectPattern.FinalPattern[i] == _CurrentBossCombos[j])
    //            {
    //                NumberOfComboSelectionRepeats[j]++;
    //            }
    //        }
    //    }
    //}
    public void GetFinalCombo(List<ComboType> FinalOutput, string EdwardFixandRewriteEverythingEdition)
    {
        FinalOutputComboArray = FinalOutput;

        for (int i = 0; i < FinalOutput.Count; i++)
        {
            for (int j = 0; j < _CurrentBossCombos.Count; j++)
            {
                if (FinalOutput[i] == _CurrentBossCombos[j])
                {
                    NumberOfComboSelectionRepeats[j]++;
                }
            }
        }
    }
    // Need Adjustment with Ken Script

    public float LowerPossibility(int percentageIndex, int chosenTimes, ComboType combo)
    {
        for (int i = 0; i < chosenTimes; i++)
        {
            _ComboDropPercentage[percentageIndex] = _ComboDropPercentage[percentageIndex] * (1 - _LowerPercentage / 100);
            print("This is percentage:" + _ComboDropPercentage[percentageIndex]);
        }//calculate percentage 
        float newAmount = Mathf.Round(_SampleCapacity * _ComboDropPercentage[percentageIndex]);//calculate the amount
        for (int i = 0; i < newAmount; i++)
        {
            _ComboPool.Add(combo);//add the certain amount of certain attack into the combo holder
        }
        _NewTotalAmount += newAmount;//recalculate the capacity
        return newAmount;
    }
}
