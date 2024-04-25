using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectPatternPanel : BasePanel
{
    public Button btnNext;

    public Dictionary<int,Color[]> patternPool = new Dictionary<int, Color[]>();

    public Color[] finalPattern;

    public int patternAmount = 8;

    public int currentPatternIndex;
    public PatternToggle[] pts;

    public ToggleGroup tgPatternBar;

    public Toggle togSelection1;
    public Toggle togSelection2;
    public Toggle togSelection3;
    private Image imageSelection1;
    private Image imageSelection2;
    private Image imageSelection3;

    public override void Init()
    {
        btnNext.gameObject.SetActive(false);

        imageSelection1 = togSelection1.gameObject.GetComponentInChildren<Image>();
        imageSelection2 = togSelection2.gameObject.GetComponentInChildren<Image>();
        imageSelection3 = togSelection3.gameObject.GetComponentInChildren<Image>();
        //todo: Add patterns to the patternPool
        //patternPool.Add();
        //To test the functionality Now use a random colour for the representation
        RandonInitializeColorPattern();

        currentPatternIndex = 0;
        UpdateSelection(currentPatternIndex);

        finalPattern = new Color[patternAmount];
        

        
        

        btnNext.onClick.AddListener(() =>
        {
            //Hide myself
            UIManager.Instance.HidePanel<SelectPatternPanel>();

            //todo: Next move
        });

        togSelection1.onValueChanged.AddListener((isOn) =>
        {
            foreach (PatternToggle tog in pts)
            {
                if (tog.gameObject.GetComponent<PatternToggle>().index == currentPatternIndex)
                {
                    tog.gameObject.GetComponentInChildren<Image>().color = imageSelection1.color;
                    finalPattern[currentPatternIndex] = imageSelection1.color;

                    break;
                }
            }
            currentPatternIndex++;
            if (currentPatternIndex >= 8)
            {
                btnNext.gameObject.SetActive(true);
            }
            else
            {
                UpdateSelection(currentPatternIndex);
            }
            //foreach (Toggle tog in tgPatternBar.ActiveToggles())
            //{
            //    tog.gameObject.GetComponentInChildren<Image>().color = imageSelection1.color;
            //    finalPattern[currentPatternIndex] = imageSelection1.color;
            //}
        });
        togSelection2.onValueChanged.AddListener((isOn) =>
        {
            foreach (PatternToggle tog in pts)
            {
                if (tog.gameObject.GetComponent<PatternToggle>().index == currentPatternIndex)
                {
                    tog.gameObject.GetComponentInChildren<Image>().color = imageSelection2.color;
                    finalPattern[currentPatternIndex] = imageSelection2.color;

                    break;
                }
            }
            currentPatternIndex++;
            if (currentPatternIndex>=8)
            {
                btnNext.gameObject.SetActive(true);
            }
            else
            {
                UpdateSelection(currentPatternIndex);
            }



            //foreach (Toggle tog in tgPatternBar.ActiveToggles())
            //{
            //    tog.gameObject.GetComponentInChildren<Image>().color = imageSelection2.color;
            //    finalPattern[currentPatternIndex] = imageSelection2.color;
            //}
        });
        togSelection3.onValueChanged.AddListener((isOn) =>
        {
            foreach (PatternToggle tog in pts)
            {
                if (tog.gameObject.GetComponent<PatternToggle>().index == currentPatternIndex)
                {
                    tog.gameObject.GetComponentInChildren<Image>().color = imageSelection3.color;
                    finalPattern[currentPatternIndex] = imageSelection3.color;

                    break;
                }
            }
            currentPatternIndex++;
            if (currentPatternIndex >= 8)
            {
                btnNext.gameObject.SetActive(true);
            }
            else
            {
                UpdateSelection(currentPatternIndex);
            }
            //foreach (Toggle tog in tgPatternBar.ActiveToggles())
            //{
            //    tog.gameObject.GetComponentInChildren<Image>().color = imageSelection3.color;
            //    finalPattern[currentPatternIndex] = imageSelection3.color;
            //}
        });
    }


    private void RandonInitializeColorPattern()
    {
        patternPool.Clear();
        for (int i = 0; i < patternAmount; i++)
        {
            Color[] randomColor = new Color[3];
            for (int j = 0; j < randomColor.Length; j++)
            {
                int randomNum = Random.Range(0, 5);
                switch (randomNum)
                {
                    case 0:
                        randomColor[j] = Color.white;
                        break;
                    case 1:
                        randomColor[j] = Color.red;
                        break;
                    case 2:
                        randomColor[j] = Color.green;
                        break;
                    case 3:
                        randomColor[j] = Color.blue;
                        break;
                    case 4:
                        randomColor[j] = Color.magenta;
                        break;
                }
            }
            patternPool.Add(i, randomColor);
        }
    }


    public void UpdateSelection(int index)
    {
        Color[] colors = patternPool[index];
        imageSelection1.color = colors[0];
        imageSelection2.color = colors[1];
        imageSelection3.color = colors[2];
    }
}
