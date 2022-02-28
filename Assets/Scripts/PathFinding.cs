using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private PathRequestManager pathRequestManager;
    [SerializeField] private Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        pathRequestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 start, Vector3 target)
    {
        StartCoroutine(FindPath(start, target));
    }
    public IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stack<Vector3> wayPoints = new Stack<Vector3>();
        bool pathfindingSuccess = false;

        Node startNode = grid.GetNodeFromPosition(startPos);
        Node targetNode = grid.GetNodeFromPosition(targetPos);

        PriorityQueue<Node> openSet = new PriorityQueue<Node>(grid.GridCount);
        HashSet<Node> closeSet = new HashSet<Node>();

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

    Stack<Vector3> GetPath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = targetNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        return SimplifyPath(path);
    }

    //경로를 단순하게
    Stack<Vector3> SimplifyPath(List<Node> path)
    {
        //단순화시킨 경로를 담을 stack
        Stack<Vector3> wayPoints = new Stack<Vector3>();
        //비교할 옛날 방향
        Vector2Int oldDirection = Vector2Int.zero; //(0,0)

        //List안에 있는 모든 node 비교
        for (int i = 1; i < path.Count; i++)
        {
            //비교할 새로운 방향 = 전 노드 좌표 - 현재 노드 좌표
            Vector2Int newDirection = new Vector2Int(path[i-1].X - path[i].X, path[i-1].Y - path[i].Y);
            //옛날 방향과 새로운 방향이 다를경우
            if (oldDirection != newDirection)
            {
                //이전 노드의 좌표를 추가
                wayPoints.Push(path[i - 1].Position);
                //예전 방향을 새로운 방향으로 업데이트
                oldDirection = newDirection;
            }
        }

        //경로 반환
        return wayPoints;
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
