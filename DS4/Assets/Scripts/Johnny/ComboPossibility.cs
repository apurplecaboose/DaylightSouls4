
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ComboPossibility : MonoBehaviour
{
    [SerializeField] List<BossDataSc.ComboType> comboTypes = new List<BossDataSc.ComboType>();
    [SerializeField] List<BossDataSc.ComboType> comboHolder = new List<BossDataSc.ComboType>();
    [SerializeField] BossDataSc.ComboType[] resultComboArray, resultComboArray1, resultComboArray2, resultComboArray3, resultComboArray4, resultComboArray5, resultComboArray6, resultComboArray7;
    public BossDataSc.ComboType[][] resultComboArrayAllInOne;
    [SerializeField] float sampleCapacity;
    [SerializeField] List<float> percentageList = new List<float>();
    [SerializeField] int[] comboOptionIndex;
    public BossDataSc bossData;
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

        ChooseCombo(resultComboArray);
        ChooseCombo(resultComboArray1);
        ChooseCombo(resultComboArray2);
        ChooseCombo(resultComboArray3);
        ChooseCombo(resultComboArray4);
        ChooseCombo(resultComboArray5);
        ChooseCombo(resultComboArray6);
        ChooseCombo(resultComboArray7);


        LoadingCombos();


    }

    // Update is called once per frame

    public void ShowAllValue()
    {
        for (int i = 0; i < comboHolder.Count; i++)
        {
            print(comboHolder[i]);
        }
    }

    public void ChooseCombo(BossDataSc.ComboType[] resultCombo)
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

    public void AddingCombo(BossDataSc.ComboType combo, float percentage)
    {
        float amount = 1000 * percentage;
        for (int i = 0; i < amount; i++)
        {
            comboHolder.Add(combo);
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

    public void LowerPossibility()
    {

    }
}
