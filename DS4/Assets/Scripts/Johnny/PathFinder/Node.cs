using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    /// <summary>
    /// create constructor(Similar to awake)
    /// Grammer for creating constructor is using the class name as function name
    /// It cannot has return type 
    /// </summary>
    /// 
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node neighborParent;
    public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost()
    {
        return gCost + hCost;
    }


}
