using System;
using System.Collections;
using System.Collections.Generic;
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
        comboPossibility = GameObject.Find("GameData").GetComponent<ComboPossibility>();

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
                patternTexts[currentPatternIndex].text = comboPossibility.ResultComboArrayAllInOne[currentPatternIndex][0].ToString();
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
                patternTexts[currentPatternIndex].text = comboPossibility.ResultComboArrayAllInOne[currentPatternIndex][1].ToString();
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
                patternTexts[currentPatternIndex].text = comboPossibility.ResultComboArrayAllInOne[currentPatternIndex][2].ToString();
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
            FinalPattern.Add(ComboPossibility.ComboType.A);
            print(FinalPattern[i]);
        }
    }

    public void UpdateSelection(int index)
    {
        btnSelection1.GetComponentInChildren<Text>().text = comboPossibility.ResultComboArrayAllInOne[index][0].ToString();
        btnSelection2.GetComponentInChildren<Text>().text = comboPossibility.ResultComboArrayAllInOne[index][1].ToString();
        btnSelection3.GetComponentInChildren<Text>().text = comboPossibility.ResultComboArrayAllInOne[index][2].ToString();
    }
}
