using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node:IHeapItem<Node>
{
    public bool Walkable;
    public Vector3 WorldPosition;
    /// <summary>
    /// create constructor(Similar to awake)
    /// Grammer for creating constructor is using the class name as function name
    /// It cannot has return type 
    /// </summary>
    /// 
    public int GridX;
    public int GridY;

    public int GCost;
    public int HCost;
    public Node NeighborParent;
    int _HeapIndex;


    public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
    {
        this.Walkable = walkable;
        this.WorldPosition = worldPosition;
        this.GridX = gridX;
        this.GridY = gridY;
    }

    public int fCost()
    {
        return GCost + HCost;
    }

    public int HeapIndex
    {
        get
        {
            return _HeapIndex;
        }
        set
        {
            _HeapIndex = value;
        }
    }
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost().CompareTo(nodeToCompare.fCost());
        if (compare == 0)
        {
            compare = HCost.CompareTo(nodeToCompare.HCost);
        }
        return -compare;
    }
}
