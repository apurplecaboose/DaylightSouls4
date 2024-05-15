using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public float nodeRadius, nodeDiameter;
    public GameObject wayPointEmp;
    public Vector2 gridWorldSize;
    public Transform BossPosition;
    Node[,] _grid;
    int _gridNumOnX, _gridNumOnY;
    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        _gridNumOnX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        _gridNumOnY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);//_gridNumX/Y represent the array index

        GenerateGrid();


    }


    void GenerateGrid()
    {
        _grid = new Node[_gridNumOnX, _gridNumOnY];//these two value represent the number of the grid(like setting up the capacity)
        Vector2 worldLeftCornorPos = new Vector2(transform.position.x - gridWorldSize.x / 2, transform.position.y - gridWorldSize.y / 2);
        for (int x = 0; x < _gridNumOnX; x++)
        {
            for (int y = 0; y < _gridNumOnY; y++)
            {
                Vector2 worldPoint = worldLeftCornorPos + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool isWalkable = !(Physics2D.CircleCast(worldPoint,nodeRadius,Vector2.right,0,obstacleLayer));//casting circle to distinguish the obstacles
                _grid[x,y]=new Node(isWalkable, worldPoint,x,y);
            }
        }
    }

    public Node TranslateWorldPosToGrid(Vector2 worldPosition)
    {
        float percentageX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;//calculate the percentage of boss X coordinate on AXIS
        float percentageY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;//calculate the percentage of boss Y coordinate on AXIS
        percentageX = Mathf.Clamp01(percentageX);
        percentageY = Mathf.Clamp01(percentageY);
        int x = Mathf.RoundToInt((_gridNumOnX - 1) * percentageX);//reason why minus one because _gridNum represents the index
        int y = Mathf.RoundToInt((_gridNumOnY - 1) * percentageY);
        return _grid[x,y];
    }

    public List<Node> GetNeiborNodes(Node node)
    {
        List<Node> neigborNodes = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX>=0&&checkX < _gridNumOnX&&checkY>=0&&checkY < _gridNumOnY)//checkX/Y cannot equal to gridnumX/Y
                {
                    neigborNodes.Add(_grid[checkX, checkY]);
                }
            }
        }
        return neigborNodes;
    }



    public List<Node> path = new List<Node>();

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridWorldSize);
        if (_grid != null)
        {
            foreach (Node node in _grid)
            {
                Node bossNodePos = TranslateWorldPosToGrid(BossPosition.position);//Obatin Boss Position
                Gizmos.color = (node.walkable) ? Color.white : Color.red;//if walkable is true ColorYello will be the result, vice versa  walkable is false, Color red will be the result(This is how ?; marks do)
                if (bossNodePos == node)
                {
                    Gizmos.color = Color.yellow;
                }//See Boss Position translate into node works or not
                if(path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
       
    }
}
