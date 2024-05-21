using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask ObstacleLayer;
    public float NodeRadius, NodeDiameter;
    public GameObject WayPointEmp;
    public Vector2 GridWorldSize;
    public Transform BossPosition;
    Node[,] _Grid;
    int _GridNumOnX, _GridNumOnY;
    private void Awake()
    {
        NodeDiameter = NodeRadius * 2;
        _GridNumOnX = Mathf.RoundToInt(GridWorldSize.x / NodeDiameter);
        _GridNumOnY = Mathf.RoundToInt(GridWorldSize.y / NodeDiameter);//_gridNumX/Y represent the array index

        GenerateGrid();


    }


    void GenerateGrid()
    {
        _Grid = new Node[_GridNumOnX, _GridNumOnY];//these two value represent the number of the grid(like setting up the capacity)
        Vector2 worldLeftCornorPos = new Vector2(transform.position.x - GridWorldSize.x / 2, transform.position.y - GridWorldSize.y / 2);
        for (int x = 0; x < _GridNumOnX; x++)
        {
            for (int y = 0; y < _GridNumOnY; y++)
            {
                Vector2 worldPoint = worldLeftCornorPos + Vector2.right * (x * NodeDiameter + NodeRadius) + Vector2.up * (y * NodeDiameter + NodeRadius);
                bool isWalkable = !(Physics2D.CircleCast(worldPoint,NodeRadius,Vector2.right,0,ObstacleLayer));//casting circle to distinguish the obstacles
                _Grid[x,y]=new Node(isWalkable, worldPoint,x,y);
            }
        }
    }

    public Node TranslateWorldPosToGrid(Vector2 worldPosition)
    {
        float percentageX = (worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;//calculate the percentage of boss X coordinate on AXIS
        float percentageY = (worldPosition.y + GridWorldSize.y / 2) / GridWorldSize.y;//calculate the percentage of boss Y coordinate on AXIS
        percentageX = Mathf.Clamp01(percentageX);
        percentageY = Mathf.Clamp01(percentageY);
        int x = Mathf.RoundToInt((_GridNumOnX - 1) * percentageX);//reason why minus one because _gridNum represents the index
        int y = Mathf.RoundToInt((_GridNumOnY - 1) * percentageY);
        return _Grid[x,y];
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
                int checkX = node.GridX + x;
                int checkY = node.GridY + y;
                if (checkX>=0&&checkX < _GridNumOnX&&checkY>=0&&checkY < _GridNumOnY)//checkX/Y cannot equal to gridnumX/Y
                {
                    neigborNodes.Add(_Grid[checkX, checkY]);
                }
            }
        }
        return neigborNodes;
    }



    public List<Node> Path = new List<Node>();

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, GridWorldSize);
        if (_Grid != null)
        {
            foreach (Node node in _Grid)
            {
                Node bossNodePos = TranslateWorldPosToGrid(BossPosition.position);//Obatin Boss Position
                Gizmos.color = (node.Walkable) ? Color.white : Color.red;//if walkable is true ColorYello will be the result, vice versa  walkable is false, Color red will be the result(This is how ?; marks do)
                if (bossNodePos == node)
                {
                    Gizmos.color = Color.yellow;
                }//See Boss Position translate into node works or not
                if(Path != null)
                {
                    if (Path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (NodeDiameter - 0.1f));
            }
        }
       
    }
}
