using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackItem
{
    public ComboPossibility.ComboType ComboKey;
    public GameObject GameObjectValue;
}
public class B_ComboLibrary : MonoBehaviour
{
    [SerializeField] AttackItem[] _AttackGameObjectArray;
    [SerializeField] Dictionary<ComboPossibility.ComboType, GameObject> _AttackDictionary;

    GameObject _CurrentAttack;
    Dictionary<ComboPossibility.ComboType,GameObject> InitializeInpectorValuesToDictionary()
    {
        Dictionary<ComboPossibility.ComboType, GameObject> initializedDictionary = new Dictionary<ComboPossibility.ComboType, GameObject>();
        foreach(var item in _AttackGameObjectArray)
        {
            initializedDictionary.Add(item.ComboKey, item.GameObjectValue);
        }
        return initializedDictionary;
    }
    void Awake()
    {
        _AttackGameObjectArray = new AttackItem[Enum.GetNames(typeof(ComboPossibility.ComboType)).Length];
        _AttackDictionary = InitializeInpectorValuesToDictionary();
    }
    void Start()
    {
        
    }
    public void StartUp(ComboPossibility.ComboType combo_name)
    {
        _AttackDictionary.TryGetValue(combo_name, out GameObject value);
        _CurrentAttack = Instantiate(value, this.transform);
        _CurrentAttack.transform.position = this.transform.position; //set intital position
        _CurrentAttack.transform.right = this.transform.right;//set intital rotation
    }
    public void StunBoss_DestroyCurrentAttack()
    {
        if (_CurrentAttack == null) return;
        Destroy(_CurrentAttack, 0.5f); //catch case
        _CurrentAttack = null;
    }
    void Update()
    {
    }
    void FixedUpdate()
    {
        
    }
}
