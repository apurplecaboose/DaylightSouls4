using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SelectPatternPanel : BasePanel
{
    public Button btnNext;

    public List<ComboPossibility.ComboType> FinalPattern;//J: change the type use to the abandon of scriptable object

    public int patternAmount = 8;

    public int currentPatternIndex;

    public ComboPossibility comboPossibility;

    public Text[] patternTexts;

    public Button btnSelection1;
    public Button btnSelection2;
    public Button btnSelection3;
    private Image imageSelection1;
    private Image imageSelection2;
    private Image imageSelection3;

    public override void Init()
    {
        comboPossibility = GameObject.FindGameObjectWithTag("Boss").GetComponent<ComboPossibility>();

        EventSystem.current.SetSelectedGameObject(btnSelection1.gameObject);

        btnNext.gameObject.SetActive(false);

        imageSelection1 = btnSelection1.gameObject.GetComponentInChildren<Image>();
        imageSelection2 = btnSelection2.gameObject.GetComponentInChildren<Image>();
        imageSelection3 = btnSelection3.gameObject.GetComponentInChildren<Image>();

        InitializeContainer();

        currentPatternIndex = 0;
        UpdateSelection(currentPatternIndex);

        btnNext.onClick.AddListener(() =>
        {
            //Hide myself
            UIManager.Instance.HidePanel<SelectPatternPanel>();

            //todo: Next move
        });

        btnSelection1.onClick.AddListener(() =>
        {
            if (currentPatternIndex == 7)
            {
                btnNext.gameObject.SetActive(true);
            }
            if(currentPatternIndex <= 7)
            {
                string malding = BossEnumStringCleanup(comboPossibility.ResultComboArrayAllInOne[currentPatternIndex][0].ToString());
                patternTexts[currentPatternIndex].text = malding;
                FinalPattern[currentPatternIndex] = comboPossibility.ResultComboArrayAllInOne[currentPatternIndex][0];

                currentPatternIndex++;
                if (currentPatternIndex < 8)
                {
                    UpdateSelection(currentPatternIndex);
                }
            }
        });

        btnSelection2.onClick.AddListener(() =>
        {
            if (currentPatternIndex == 7)
            {
                btnNext.gameObject.SetActive(true);
            }
            if (currentPatternIndex <= 7)
            {
                string malding = BossEnumStringCleanup(comboPossibility.ResultComboArrayAllInOne[currentPatternIndex][1].ToString());
                patternTexts[currentPatternIndex].text = malding;
                FinalPattern[currentPatternIndex] = comboPossibility.ResultComboArrayAllInOne[currentPatternIndex][1];

                currentPatternIndex++;
                if (currentPatternIndex < 8)
                {
                    UpdateSelection(currentPatternIndex);
                }
            }
        });

        btnSelection3.onClick.AddListener(() =>
        {
            if (currentPatternIndex == 7)
            {
                btnNext.gameObject.SetActive(true);
            }
            if (currentPatternIndex <= 7)
            {
                string malding = BossEnumStringCleanup(comboPossibility.ResultComboArrayAllInOne[currentPatternIndex][2].ToString());
                patternTexts[currentPatternIndex].text = malding;
                FinalPattern[currentPatternIndex] = comboPossibility.ResultComboArrayAllInOne[currentPatternIndex][2];

                currentPatternIndex++;
                if (currentPatternIndex < 8)
                {
                    UpdateSelection(currentPatternIndex);
                }
            }
        });
    }
    void InitializeContainer()
    {
        for (int i = 0; i < patternAmount; i++)
        {
            FinalPattern.Add(ComboPossibility.ComboType.B999__PLACEHOLDER_FOR_KENS_CODE);
            print(FinalPattern[i]);
        }
        comboPossibility.ChosenComboFromKen = FinalPattern; //Edward: output to johnny script
    }

    public void UpdateSelection(int index)
    {
        string string1 = BossEnumStringCleanup(comboPossibility.ResultComboArrayAllInOne[index][0].ToString());
        string string2 = BossEnumStringCleanup(comboPossibility.ResultComboArrayAllInOne[index][1].ToString());
        string string3 = BossEnumStringCleanup(comboPossibility.ResultComboArrayAllInOne[index][2].ToString());
        btnSelection1.GetComponentInChildren<Text>().text = string1;
        btnSelection2.GetComponentInChildren<Text>().text = string2;
        btnSelection3.GetComponentInChildren<Text>().text = string3;
    }
    string BossEnumStringCleanup(string inputString)
    {
        string cleanedString = Regex.Replace(inputString, @"^[^_]*__", "");

        cleanedString = cleanedString.Replace("_", " ");

        return cleanedString;
    }
}
