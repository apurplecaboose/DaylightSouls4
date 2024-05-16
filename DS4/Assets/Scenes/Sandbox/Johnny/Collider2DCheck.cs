using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider2DCheck : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().color = Color.black;
        print("isTriggerWorking");
    }
    private void OnTriggerExit(Collider other)
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }
}
