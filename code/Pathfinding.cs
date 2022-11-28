using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void StartFindingPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 StartPos, Vector3 TargetPos)
    {
        Vector3[] Waypoints = new Vector3[0];
        bool success = false;
        Hex startNode = HexGrid.hexGrid.NodeAtPosition(StartPos);
        Hex EndNode = HexGrid.hexGrid.NodeAtPosition(TargetPos);
        startNode.parent = startNode;
        if (startNode.walkable && EndNode.walkable)
        {
            Heap<Hex> OpenSet = new Heap<Hex>(HexGrid.hexGrid.MaxSize);
            HashSet<Hex> closedSet = new HashSet<Hex>();
            OpenSet.Add(startNode);

            while (OpenSet.Count > 0)
            {
                Hex currentNode = OpenSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == EndNode)
                {
                    success = true;
                    break;
                }

                foreach (Hex neighbour in HexGrid.hexGrid.getNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                    int newMovementCostToNeighbour = currentNode.GCost + getDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.GCost || !OpenSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = getDistance(neighbour, EndNode);
                        neighbour.parent = currentNode;

                        if (!OpenSet.Contains(neighbour))
                            OpenSet.Add(neighbour);
                        else
                            OpenSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (success == true)
        {
            Waypoints = RetracePath(startNode, EndNode);
        }
        pathFindingManager.instance.returnPath(Waypoints, success);
    }

    Vector3[] RetracePath(Hex startNode, Hex EndNode)
    {
        List<Hex> path = new List<Hex>();
        Hex CurrentNode = EndNode;
        while (CurrentNode != startNode)
        {
            path.Add(CurrentNode);
            CurrentNode = CurrentNode.parent;
        }
        Vector3[] Waypoints = simplifyPath(path);
        Array.Reverse(Waypoints);
        return Waypoints;
    }

    Vector3[] simplifyPath(List<Hex> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        if (path.Count == 1)
        {
            waypoints.Add(path[0].worldPos);
        }
        else
        {
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i - 1].worldPos);
                }
                directionOld = directionNew;
            }
        }
        return waypoints.ToArray();
    }

    int getDistance(Hex FirstNode, Hex EndNode)
    {
        int distX = Mathf.Abs(FirstNode.GridX - EndNode.GridX);
        int distY = Mathf.Abs(FirstNode.GridY - EndNode.GridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}
