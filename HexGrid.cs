using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public static HexGrid hexGrid;
    public Hex[,] grid;
    public Vector2 GridSize;
    [Range(0.1f, 1)]
    public float hexRadius;
    float hexHeight = 0;
    public float hexWidth;
    int GridSizeX, GridSizeY;
    float NodeDiameter;
    bool DisplayGizmos = true;
    public GameObject hexagonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        hexRadius = hexagonPrefab.GetComponent<MeshRenderer>().bounds.size.z / 2;
        hexHeight = hexagonPrefab.GetComponent<MeshRenderer>().bounds.size.x / 2;
        hexWidth = hexagonPrefab.GetComponent<MeshRenderer>().bounds.size.z / 2;
        NodeDiameter = hexRadius * 2;

        if (hexGrid != null)
        {
            Destroy(this);
        }
        else
        {
            hexGrid = this;
        }
    }

    public int MaxSize
    {
        get
        {
            return GridSizeX * GridSizeY;
        }
    }

    public void createGrid(Vector3 pos, int sizeX, int sizeY)
    {
        GridSize.x = sizeX;
        GridSize.y = sizeY;
        GridSizeX = Mathf.RoundToInt(GridSize.x / NodeDiameter);
        GridSizeY = Mathf.RoundToInt(GridSize.y / NodeDiameter);
        grid = new Hex[GridSizeX, GridSizeY];
        Vector3 bottomLeft = pos - Vector3.right * GridSize.x / 2 - Vector3.forward * GridSize.y / 2;
        bool offset = true;
        for (int i = 0; i < GridSizeX; i++)
        {
            offset = !offset;
            Vector3 posOffset;
            if (offset)
                posOffset = new Vector3(hexHeight / 2 * i, 0, hexWidth);
            else
                posOffset = new Vector3(hexHeight / 2 * i, 0,0);
            for (int j = 0; j < GridSizeY; j++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (i * hexHeight) + Vector3.forward * (j * NodeDiameter + hexRadius) + posOffset;
                bool Walkable/* = !Physics.CheckSphere(worldPoint, hexRadius, unwalkableArea)*/ = true;
                grid[i, j] = new Hex(worldPoint, i, j);
                if (Walkable)
                {
                    GameObject hexObject = Instantiate(hexagonPrefab);
                    hexObject.transform.position = worldPoint;
                    hexObject.transform.parent = this.transform;
                }
            }
        }
    }

    public Hex NodeAtPosition(Vector3 WorldPos)
    {
        Hex closestHex = grid[0, 0];
        float dist = Mathf.Infinity;
        for (int i = 0; i < GridSizeX; i++)
        {
            for (int j = 0; j < GridSizeY; j++)
            {
                float newDist = Vector3.Distance(grid[i, j].worldPos, WorldPos);
                if (newDist < dist)
                {
                    closestHex = grid[i, j];
                    dist = newDist;
                }
            }
        }
        return closestHex;
    }

    public Hex ReturnNode(int X, int Y)
    {
        if (grid[X, Y] != null)
        {
            return grid[X, Y];
        }
        else
        {
            return null;
        }
    }

    public List<Hex> getNeighbours(Hex node)
    {
        List<Hex> neighbours = new List<Hex>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                else
                {
                    int checkX = node.GridX + i;
                    int CheckY = node.GridY + j;

                    if (checkX >= 0 && CheckY >= 0 && checkX < GridSizeX && CheckY < GridSizeY)
                    {
                        neighbours.Add(grid[checkX, CheckY]);
                    }
                }
            }
        }

        return neighbours;
    }

    private void OnDrawGizmos()
    {
        if (DisplayGizmos == true)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(GridSize.x, 1, GridSize.y));

            if (grid != null)
            {
                foreach (Hex node in grid)
                {
                    Gizmos.color = (node.walkable) ? Color.white : Color.red;
                    Gizmos.DrawCube(node.worldPos, Vector3.one * (NodeDiameter - .1f));
                }
            }
        }
    }
}
