using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BossDataSc;

public class B_ComboLibrary : MonoBehaviour
{
    [HideInInspector]
    public Boss_Master BossMasterRef; // used to change state from attack to opening
    public B_ComboLibrary ComboLibraryRef;
    public List<GameObject> AllBossCombos;

    GameObject _CurrentAttack;
    public void StartUp(ComboType combo_name)
    {
        _CurrentAttack = Instantiate(AllBossCombos[(int)combo_name]); // cast as int (make sure your enum has an associated int index)
    }
    void Awake()
    {
        BossMasterRef = this.GetComponent<Boss_Master>();
        ComboLibraryRef = this;
    }
    void Start()
    {
        
    }
    void Update()
    {
        if(BossMasterRef.Boss_Action == Boss_Master.Boss_Action_List.STUNNED)
        {
            Destroy(_CurrentAttack);
        }
    }
    void FixedUpdate()
    {
        
    }
}
public class Boss_AttackDataTransfer : B_ComboLibrary
{
    public B_ComboLibrary ComboLibraryRef;
    public Boss_Master Boss_MasterRef;
