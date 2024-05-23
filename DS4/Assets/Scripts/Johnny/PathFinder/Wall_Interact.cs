using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Interact : MonoBehaviour
{
    private void Update()
    {
        Wallbreaking();
    }

    int _Attack_Taken_Time=3;
    public bool IsBroken;
    public Grid GridSc;
    void Wallbreaking()
    {
        if (IsBroken && _Attack_Taken_Time>0)
        {
            _Attack_Taken_Time--;
            IsBroken = false;
        }
        else if(_Attack_Taken_Time<=0)
        {
            gameObject.layer = LayerMask.GetMask("Defualt");//change the layer to make the obstable area walkable
            GridSc.GenerateGrid();//Regenrate the grid according to the new map
            Destroy(gameObject);
        }
    }
}
