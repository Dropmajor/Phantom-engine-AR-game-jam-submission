using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class pathFindingManager : MonoBehaviour
{
    static Action<Vector3[], bool> currentlyHandling;
    public static pathFindingManager instance;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public static void RequestPath(Hex pathStart, Hex pathEnd, Action<Vector3[], bool> callBack)
    {
        currentlyHandling = callBack;
        print(pathStart.worldPos + " " + pathEnd.worldPos);
        Pathfinding.instance.StartFindingPath(pathStart.worldPos, pathEnd.worldPos);
    }

    public void returnPath(Vector3[] path, bool succesful)
    {
        if(succesful)
        {
            currentlyHandling(path, succesful);
        }
    }
}
