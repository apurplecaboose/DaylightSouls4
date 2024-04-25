using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternToggle : MonoBehaviour
{
    private SelectPatternPanel owner;
    private Toggle tog;
    public int index;
    void Start()
    {
        owner = UIManager.Instance.GetPanel<SelectPatternPanel>();
        tog = GetComponent<Toggle>();
        
        
        //tog.onValueChanged.AddListener((isOn) =>
        //{
        //    if (isOn)
        //    {
        //        owner.currentPatternIndex = index;
        //        owner.UpdateSelection(index);
        //    }
        //});
    }
}
