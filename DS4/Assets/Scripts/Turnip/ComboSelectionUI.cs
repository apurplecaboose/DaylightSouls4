using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class LinkPictures
{
    public ComboPossibility.ComboType ComboKey;
    public Sprite ComboImageValue;
}

public class ComboSelectionUI : MonoBehaviour
{
    #region Varibles
    public enum Fade { In, Out };
    [HideInInspector] public Fade FadeStatus;
    float _LerpDtime;

    [Header("Editable but change stuffs in the script too")]
    int _MaxPatternCount = 8;

    [Header("References AUTO")]
    [HideInInspector] public ComboPossibility ComboPossibilityRef;
    Button _Option1Button, _Option2Button, _Option3Button;
    TMP_Text _Option1TMP, _Option2TMP, _Option3TMP;
    Image _Option1_Image, _Option2_Image, _Option3_Image;

    [Header("References")]
    [SerializeField] LinkPictures[] _ComboPictureArray;
    [SerializeField] Dictionary<ComboPossibility.ComboType, Sprite> _Pictionary;
    Dictionary<ComboPossibility.ComboType, Sprite> InitializeInpectorValuesToDictionary()
    {
        Dictionary<ComboPossibility.ComboType, Sprite> initializedDictionary = new Dictionary<ComboPossibility.ComboType, Sprite>();
        foreach (var item in _ComboPictureArray)
        {
            initializedDictionary.Add(item.ComboKey, item.ComboImageValue);
        }
        return initializedDictionary;
    }

    [SerializeField] CanvasGroup _CanvasGroup;
    [SerializeField] Transform _ComboOptionsParent;
    [SerializeField] Transform _EndComboDisplayParent;
    [SerializeField] Button _ExitComboSelectionButton;
    [SerializeField] Image[] _FinalComboImageComponents;
    [SerializeField] TMP_Text _CurrentComboNumberText;

    [Header("Debug Purposes")]
    List<ComboPossibility.ComboType> _FinalPattern = new List<ComboPossibility.ComboType>(); //OUTPUT
    int _CurrentPatternIndex;
    #endregion
    void Awake()
    {
        //set game state

        //get components and references
        FadeStatus = Fade.In;
        ComboPossibilityRef = GameObject.FindGameObjectWithTag("Boss").GetComponent<ComboPossibility>();
        Transform button1transform = _ComboOptionsParent.GetChild(0);
        Transform button2transform = _ComboOptionsParent.GetChild(1);
        Transform button3transform = _ComboOptionsParent.GetChild(2);
        _Option1Button = button1transform.GetComponent<Button>();
        _Option2Button = button2transform.GetComponent<Button>();
        _Option3Button = button3transform.GetComponent<Button>();
        _Option1_Image = button1transform.GetComponent<Image>();
        _Option2_Image = button2transform.GetComponent<Image>();
        _Option3_Image = button3transform.GetComponent<Image>();
        _Option1TMP = button1transform.GetChild(0).GetComponent<TMP_Text>();
        _Option2TMP = button2transform.GetChild(0).GetComponent<TMP_Text>();
        _Option3TMP = button3transform.GetChild(0).GetComponent<TMP_Text>();

        _Pictionary = InitializeInpectorValuesToDictionary();
        EventSystem.current.SetSelectedGameObject(_Option1Button.gameObject); //selects first "selected" UI element 
        float zoomScale = 1.5f;
        button1transform.localScale = button1transform.transform.localScale = new Vector3(zoomScale, zoomScale, 1);
    }
    void Start()
    {
        //references
        _ExitComboSelectionButton.gameObject.SetActive(false);
        _EndComboDisplayParent.gameObject.SetActive(false);

        _CurrentPatternIndex = 0;
        // initialize the array for placement
        for (int i = 0; i < _MaxPatternCount; i++)
        {
            _FinalPattern.Add(ComboPossibility.ComboType.B999__PLACEHOLDER_FOR_KENS_CODE);
        }
        UpdateSelection(0); // initialize the first round of selections
    }
    void Update()
    {
        int displaynumber = _CurrentPatternIndex + 1;
        displaynumber = Mathf.Clamp(displaynumber, 0, 8);
        _CurrentComboNumberText.text = "Combo Number " + displaynumber;
        _LerpDtime += Time.deltaTime;
        if(FadeStatus.Equals(Fade.In))
        {
            float lerptotaltime = 1;
            float a = Mathf.Lerp(0, 1, _LerpDtime / lerptotaltime);
            _CanvasGroup.alpha = a;
        }
        if (FadeStatus.Equals(Fade.Out))
        {
            float lerptotaltime = 0.5f;
            float a = Mathf.Lerp(1, -0.1f, _LerpDtime / lerptotaltime);
            _CanvasGroup.alpha = a;
            if (a <= 0) Destroy(_CanvasGroup.gameObject);
        }
    }
    #region ButtonUnityEvents
    public void SelectOption1()
    {
        ComboSelected(0);
    }
    public void SelectOption2()
    {
        ComboSelected(2);
    }
    public void SelectOption3()
    {
        ComboSelected(2);
    }
    public void ExitComboSelectionScreen()
    {
        _LerpDtime = 0;
        FadeStatus = Fade.Out;
        Destroy(_ExitComboSelectionButton);
        //hide panel resume game state
    }
    #endregion
    void ComboSelected(int buttonindexNumber)
    {
        if (_CurrentPatternIndex == _MaxPatternCount - 1) Invoke("LastSelection", 0.1f);
        if (_CurrentPatternIndex <= _MaxPatternCount - 1)
        {
            string malding = BossEnumStringCleanup(ComboPossibilityRef.ResultComboArrayAllInOne[_CurrentPatternIndex][buttonindexNumber].ToString());
            //SelectedComboTexts[CurrentPatternIndex].text = malding;
            _FinalPattern[_CurrentPatternIndex] = ComboPossibilityRef.ResultComboArrayAllInOne[_CurrentPatternIndex][buttonindexNumber];

            _CurrentPatternIndex++;
            if (_CurrentPatternIndex < _MaxPatternCount)
            {
                UpdateSelection(_CurrentPatternIndex);
            }
        }
    }
    void LastSelection()
    {
        //run some code to move the UI elements
        ComboPossibilityRef.GetFinalCombo(_FinalPattern, "rewriting feels bad man...");
        //change game state

        _ExitComboSelectionButton.gameObject.SetActive(true);
        _EndComboDisplayParent.gameObject.SetActive(true);
        _ComboOptionsParent.transform.parent.gameObject.SetActive(false);
        //_ComboOptionsParent.gameObject.SetActive(false);

        for (int i = 0; i < _MaxPatternCount; i++)
        {
            ComboPossibility.ComboType combo = _FinalPattern[i];
            _Pictionary.TryGetValue(combo, out var sprite);
            _FinalComboImageComponents[i].sprite = sprite;
        }
        EventSystem.current.SetSelectedGameObject(_ExitComboSelectionButton.gameObject);
    }

    public void UpdateSelection(int index)
    {
        ComboPossibility.ComboType combooption1 = ComboPossibilityRef.ResultComboArrayAllInOne[index][0];
        ComboPossibility.ComboType combooption2 = ComboPossibilityRef.ResultComboArrayAllInOne[index][1];
        ComboPossibility.ComboType combooption3 = ComboPossibilityRef.ResultComboArrayAllInOne[index][2];
        string string1 = BossEnumStringCleanup(combooption1.ToString());
        string string2 = BossEnumStringCleanup(combooption2.ToString());
        string string3 = BossEnumStringCleanup(combooption3.ToString());
        _Option1TMP.text = string1;
        _Option2TMP.text = string2;
        _Option3TMP.text = string3;

        //update images here
        _Pictionary.TryGetValue(combooption1, out var pic1);
        _Pictionary.TryGetValue(combooption2, out var pic2);
        _Pictionary.TryGetValue(combooption3, out var pic3);
        _Option1_Image.sprite = pic1;
        _Option2_Image.sprite = pic2;
        _Option3_Image.sprite = pic3;
    }
    string BossEnumStringCleanup(string inputString)
    {
        string cleanedString = Regex.Replace(inputString, @"^[^_]*__", "");

        cleanedString = cleanedString.Replace("_", " ");

        return cleanedString;
    }

}
