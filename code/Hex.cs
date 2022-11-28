using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class Hex : IHeapItem<Hex>
{
    public Vector3 worldPos;
    public int GridX, GridY;
    public int GCost;
    public int HCost;
    public bool walkable = true;
    public Hex parent;
    int heapIndex;


    public Hex(Vector3 pos, int _gridX, int _gridY)
    {
        worldPos = pos;
        GridX = _gridX;
        GridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return GCost + HCost;
        }
    }

    public int CompareTo(Hex nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(nodeToCompare.HCost);
        }
        return -compare;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
}
