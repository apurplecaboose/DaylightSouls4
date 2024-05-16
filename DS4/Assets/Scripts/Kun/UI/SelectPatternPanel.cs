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

    public List<BossDataSc.ComboType> finalPattern;

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
                patternTexts[currentPatternIndex].text = comboPossibility.resultComboArrayAllInOne[currentPatternIndex][0].ToString();
                finalPattern[currentPatternIndex] = comboPossibility.resultComboArrayAllInOne[currentPatternIndex][0];

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
                patternTexts[currentPatternIndex].text = comboPossibility.resultComboArrayAllInOne[currentPatternIndex][1].ToString();
                finalPattern[currentPatternIndex] = comboPossibility.resultComboArrayAllInOne[currentPatternIndex][1];

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
                patternTexts[currentPatternIndex].text = comboPossibility.resultComboArrayAllInOne[currentPatternIndex][2].ToString();
                finalPattern[currentPatternIndex] = comboPossibility.resultComboArrayAllInOne[currentPatternIndex][2];

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
            finalPattern.Add(BossDataSc.ComboType.A);
            print(finalPattern[i]);
        }
    }

    public void UpdateSelection(int index)
    {
        btnSelection1.GetComponentInChildren<Text>().text = comboPossibility.resultComboArrayAllInOne[index][0].ToString();
        btnSelection2.GetComponentInChildren<Text>().text = comboPossibility.resultComboArrayAllInOne[index][1].ToString();
        btnSelection3.GetComponentInChildren<Text>().text = comboPossibility.resultComboArrayAllInOne[index][2].ToString();
    }
}