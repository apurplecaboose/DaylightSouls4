using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFollow : MonoBehaviour
{
    [SerializeField]Transform _Follow;
    void Update()
    {
        this.transform.position = new Vector3(_Follow.position.x, _Follow.position.y, this.transform.position.z);
    }
}
