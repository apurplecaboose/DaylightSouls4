using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTesting : MonoBehaviour
{
    public void leftTriggerTesting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            print("its triggering");
        }

    }
}
