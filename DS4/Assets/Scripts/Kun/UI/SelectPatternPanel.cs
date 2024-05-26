//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using TMPro;
//using Random = UnityEngine.Random;

//[Serializable]
//public class LinkPictures
//{
//    public ComboPossibility.ComboType ComboKey;
//    public Sprite ComboImageValue;
//}

//public class SelectPatternPanel : BasePanel
//{
//    public LinkPictures[] _ComboPictureArray;
//    public Dictionary<ComboPossibility.ComboType, Sprite> _Pictionary;

//    Dictionary<ComboPossibility.ComboType, Sprite> InitializeInpectorValuesToDictionary()
//    {
//        Dictionary<ComboPossibility.ComboType, Sprite> initializedDictionary = new Dictionary<ComboPossibility.ComboType, Sprite>();
//        foreach (var item in _ComboPictureArray)
//        {
//            initializedDictionary.Add(item.ComboKey, item.ComboImageValue);
//        }
//        return initializedDictionary;
//    }

//    [Header("Editable")]
//    public int MaxPatternCount = 8;

//    [Header("References AUTO")]
//    [HideInInspector] public ComboPossibility ComboPossibilityRef;

//    [Header("References")]
//    public GameObject _ComboOptionsParent;
//    public GameObject _PatternsBar;

//    public Button ExitComboSelectionButton;
//    public Button Option1Button, Option2Button, Option3Button;
//    public TMP_Text _Option1TMP, _Option2TMP, _Option3TMP;
//    public Image _Option1_Image, _Option2_Image, _Option3_Image;
//    public Image[] _FinalComboImageComponents;

//    [Header("Debug Purposes")]
//    [HideInInspector] public List<ComboPossibility.ComboType> FinalPattern;//OUTPUT
//    [HideInInspector] public int CurrentPatternIndex;
//    public override void Init() // start function in Base Panel 
//    {
//        //references
//        ComboPossibilityRef = GameObject.FindGameObjectWithTag("Boss").GetComponent<ComboPossibility>();
//        //_Option1_Image = Option1Button. gameObject.GetComponent<Image>();
//        //_Option2_Image = Option2Button.gameObject.GetComponent<Image>();
//        //_Option3_Image = Option3Button.gameObject.GetComponent<Image>();

//        //_Option1TMP = Option1Button.gameObject.GetComponentInChildren<TMP_Text>();
//        //_Option2TMP = Option1Button.gameObject.GetComponentInChildren<TMP_Text>();
//        //_Option3TMP = Option1Button.gameObject.GetComponentInChildren<TMP_Text>();

//        _Pictionary = InitializeInpectorValuesToDictionary();


//        //run
//        //EventSystem.current.SetSelectedGameObject(Option1Button.gameObject); //selects first "selected" UI element 

//        ExitComboSelectionButton.gameObject.SetActive(false);
//        _PatternsBar.gameObject.SetActive(false);

//        InitializeContainer();

//        CurrentPatternIndex = 0;
//        UpdateSelection(CurrentPatternIndex);

//        //button on click events
//        ExitComboSelectionButton.onClick.AddListener(() => { UIManager.Instance.HidePanel<SelectPatternPanel>(); });
//        Option1Button.onClick.AddListener(() => { ComboSelected(0); });
//        Option2Button.onClick.AddListener(() => { ComboSelected(1); });
//        Option3Button.onClick.AddListener(() => { ComboSelected(2); });
//    }
//    void ComboSelected(int buttonindexNumber)
//    {
//        if (CurrentPatternIndex == MaxPatternCount - 1) Invoke("LastSelection",0.1f);
//        if (CurrentPatternIndex <= MaxPatternCount - 1)
//        {
//            string malding = BossEnumStringCleanup(ComboPossibilityRef.ResultComboArrayAllInOne[CurrentPatternIndex][buttonindexNumber].ToString());
//            //SelectedComboTexts[CurrentPatternIndex].text = malding;
//            FinalPattern[CurrentPatternIndex] = ComboPossibilityRef.ResultComboArrayAllInOne[CurrentPatternIndex][buttonindexNumber];

//            CurrentPatternIndex++;
//            if (CurrentPatternIndex < MaxPatternCount)
//            {
//                UpdateSelection(CurrentPatternIndex);
//            }
//        }
//    }
//    void LastSelection()
//    {
//        //run some code to move the UI elements
//        ExitComboSelectionButton.gameObject.SetActive(true);
//        _PatternsBar.gameObject.SetActive(true);
//        _ComboOptionsParent.gameObject.SetActive(false);

//        for (int i = 0; i < MaxPatternCount; i++)
//        {
//            ComboPossibility.ComboType combo = FinalPattern[i];
//            _Pictionary.TryGetValue(combo, out var sprite);
//            _FinalComboImageComponents[i].sprite = sprite;
//        }
//        EventSystem.current.SetSelectedGameObject(ExitComboSelectionButton.gameObject);
//    }
//    void InitializeContainer()
//    {
//        for (int i = 0; i < MaxPatternCount; i++)
//        {
//            FinalPattern.Add(ComboPossibility.ComboType.B999__PLACEHOLDER_FOR_KENS_CODE);
//            print(FinalPattern[i]);
//        }
//        ComboPossibilityRef.ChosenComboFromKen = FinalPattern; //Edward: output to johnny script
//    }

//    public void UpdateSelection(int index)
//    {
//        ComboPossibility.ComboType combooption1 = ComboPossibilityRef.ResultComboArrayAllInOne[index][0];
//        ComboPossibility.ComboType combooption2 = ComboPossibilityRef.ResultComboArrayAllInOne[index][1];
//        ComboPossibility.ComboType combooption3 = ComboPossibilityRef.ResultComboArrayAllInOne[index][2];
//        string string1 = BossEnumStringCleanup(combooption1.ToString());
//        string string2 = BossEnumStringCleanup(combooption2.ToString());
//        string string3 = BossEnumStringCleanup(combooption3.ToString());
//        _Option1TMP.text = string1;
//        _Option2TMP.text = string2;
//        _Option3TMP.text = string3;

//        //update images here
//        _Pictionary.TryGetValue(combooption1 , out var pic1);
//        _Pictionary.TryGetValue(combooption2, out var pic2);
//        _Pictionary.TryGetValue(combooption3, out var pic3);
//        _Option1_Image.sprite = pic1;
//        _Option2_Image.sprite = pic2;
//        _Option3_Image.sprite = pic3;
//    }
//    string BossEnumStringCleanup(string inputString)
//    {
//        string cleanedString = Regex.Replace(inputString, @"^[^_]*__", "");

//        cleanedString = cleanedString.Replace("_", " ");

//        return cleanedString;
//    }

//}
