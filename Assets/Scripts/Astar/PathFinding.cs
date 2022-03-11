using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private PathRequestManager pathRequestManager;
    [SerializeField] private GridManager grid;

    private void Awake()
    {
        grid = GetComponent<GridManager>();
        pathRequestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 start, Vector3 target)
    {
        StartCoroutine(FindPath(start, target));
    }
    public IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Stack<Vector2Int> wayPoints = new Stack<Vector2Int>();
        Stack<Vector2Int> wayPoints = new();
        bool pathfindingSuccess = false;

        Node startNode = grid.GetNodeFromPosition(startPos);
        Node targetNode = grid.GetNodeFromPosition(targetPos);

        PriorityQueue<Node> openSet = new(grid.GridCount);
        HashSet<Node> closeSet = new();

        openSet.Push(startNode);

        while (openSet.Count > 0)
        {

            Node currentNode = openSet.Pop();

            closeSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                pathfindingSuccess = true;
                break;
            }

            foreach (Node neighborNode in grid.GetNeighbors(currentNode))
            {
                if (closeSet.Contains(neighborNode))
                {
                    continue;
                }

                int newGCost = currentNode.Gcost + GetDistance(currentNode, neighborNode);

                if (openSet.Contains(neighborNode))
                {
                    if (newGCost < neighborNode.Gcost)
                    {
                        neighborNode.Parent = currentNode;
                        neighborNode.Gcost = newGCost;
                    }
                }
                else
                {
                    int newHCost = GetDistance(neighborNode, targetNode);
                    neighborNode.Gcost = newGCost;
                    neighborNode.Hcost = newHCost;

                    neighborNode.Parent = currentNode;

                    openSet.Push(neighborNode);
                }

            }

            yield return null;
        }

        if (pathfindingSuccess)
        {
            wayPoints = GetPath(startNode, targetNode);
        }

        pathRequestManager.FinishPathfinding(wayPoints, pathfindingSuccess);

        yield return null;
    }

    Stack<Vector2Int> GetPath(Node startNode, Node targetNode)
    {
        //Stack<Vector2Int> path = new Stack<Vector2Int>();
        Stack<Vector2Int> path = new();

        Node currentNode = targetNode;
        path.Push(currentNode.Offset);

        while (currentNode != startNode)
        {
            currentNode = currentNode.Parent;
            path.Push(currentNode.Offset);
        }

        return path;
    }
   
    int GetDistance(Node nodeA, Node nodeB)
    {
        int disX = Mathf.Abs(nodeA.X - nodeB.X);
        int disY = Mathf.Abs(nodeA.Y - nodeB.Y);

        if (disX > disY)
        {
            return 14 * disY + 10 * (disX - disY);
        }
        else
        {
            return 14 * disX + 10 * (disY - disX);
        }

    }

}
