using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryTesterDummy : MonoBehaviour
{//the idea for this is simply trying to prove that parry is working,
 //it will have a collider and constantly check if parry is on,
 //if parry is on it will turn green else it will be red;
    int _tickCount;
    SpriteRenderer _spriteRenderer;
    Color _objectColor;
    public enum _attackState
    {
        attack,
        parried

    }
    public _attackState AttackState;
    // Start is called before the first frame update
    void Start()
    {
        AttackState = _attackState.attack;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _objectColor = _spriteRenderer.color;
    }


  
    void ColorChange()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
