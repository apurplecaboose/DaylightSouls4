using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public Transform boss, player;

    Grid _grid;
    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }
    private void Update()
    {
        PathFinder(boss.position, player.position);
        FollowThePath();
    }
    void PathFinder(Vector2 startPosition, Vector2 targetPosition)
    {
        Node startNode = _grid.TranslateWorldPosToGrid(startPosition);
        Node targetNode = _grid.TranslateWorldPosToGrid(targetPosition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost() < currentNode.fCost() || openSet[i].fCost() == currentNode.fCost() && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }


            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            //print("Openset NUmber: " + openSet.Count);
            //print("CloseSet NUmber:" + closeSet.Count);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;//Reach the destination
            }


            //need more understanding
            foreach (Node neighbor in _grid.GetNeiborNodes(currentNode))
            {
                if (!neighbor.walkable || closeSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementDisToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);//basically obtain the new G value to for the other neighbor
                if (newMovementDisToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementDisToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.neighborParent = currentNode;
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);//Add all the neighbor node to the openset to find out the shortest route
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.neighborParent;
        }
        path.Reverse();
        _grid.path = path;
        print(_grid.path.Count);
    }

    public Vector3 target;
    public int nodeIndex;
    public float speed;
    void FollowThePath()
    {
        speed = 5 ;      
        if (_grid.path.Count>0)
        {
            target = _grid.path[nodeIndex].worldPosition;
            boss.position = Vector2.MoveTowards(boss.position, target, speed * Time.deltaTime);
        }
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        //similar to calculate the distance between coordinates
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);//only obstain the distance as magnitude
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) { return 14 * dstY + 10 * (dstX - dstY); }           
        return 14 * dstX + 10 * (dstY - dstX);
        //calculate the dst

    }
}
