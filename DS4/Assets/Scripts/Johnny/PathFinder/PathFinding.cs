using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _BossTransform;
    [SerializeField] Transform _PlayerTransform;
    [SerializeField] Grid _Grid;
    [Header("Editable")]
    public int GridDisToStop;
    public float Speed;
    [Header("DEBUG DO NOT CHANGE")]
    public Vector3 Target;
    public int NodeIndex;
    bool _StopMove;

    private void Awake()
    {
        _BossTransform = this.transform;
        //_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        PathFinder(_BossTransform.position, _PlayerTransform.position);
        FollowThePath();
    }
    void PathFinder(Vector2 startPosition, Vector2 targetPosition)
    {
        Node startNode = _Grid.TranslateWorldPosToGrid(startPosition);
        Node targetNode = _Grid.TranslateWorldPosToGrid(targetPosition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost() < currentNode.fCost() || openSet[i].fCost() == currentNode.fCost() && openSet[i].HCost < currentNode.HCost)
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
            foreach (Node neighbor in _Grid.GetNeiborNodes(currentNode))
            {
                if (!neighbor.Walkable || closeSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementDisToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);//basically obtain the new G value to for the other neighbor
                if (newMovementDisToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = newMovementDisToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.NeighborParent = currentNode;
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
            currentNode = currentNode.NeighborParent;
        }
        path.Reverse();
        _Grid.Path = path;
        print(_Grid.Path.Count);
    }
    void FollowThePath()
    {      
        if (_Grid.Path.Count> GridDisToStop&& !_StopMove)
        {
            Target = _Grid.Path[NodeIndex].WorldPosition;
            _BossTransform.position = Vector2.MoveTowards(_BossTransform.position, Target, Speed * Time.deltaTime);
        }
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        //similar to calculate the distance between coordinates
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);//only obstain the distance as magnitude
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY) { return 14 * dstY + 10 * (dstX - dstY); }           
        return 14 * dstX + 10 * (dstY - dstX);
        //calculate the dst

    }
}
