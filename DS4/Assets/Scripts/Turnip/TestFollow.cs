using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestFollow : MonoBehaviour
{
    [SerializeField]Transform _Follow;
    void Awake()
    {
        
    }
    void Start()
    {
        
    }
    void Update()
    {
        this.transform.position = new Vector3(_Follow.position.x, _Follow.position.y, this.transform.position.z);
    }
    void FixedUpdate()
    {
        
    }
}
