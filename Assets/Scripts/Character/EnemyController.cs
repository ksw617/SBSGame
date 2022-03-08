using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyController : Character
{
    protected override void Start()
    {
        base.Start();
        FindOtherDestination();
        callback = FindOtherDestination;
    }

    void FindOtherDestination()
    {
        StartCoroutine("StartFindDestination");
    }

    IEnumerator StartFindDestination()
    {
        //yield return new WaitForSeconds(3f);


        List<Node> walkableNode = GetNodes(15, currentNode.X, currentNode.Y);

        int randIndex = Random.Range(0, walkableNode.Count);
        Node other = walkableNode[randIndex];

        PathRequestManager.Instance.RequestPath(transform.position, other.Position, CallFollowTarget);
        yield return null;
    }


    private List<Node> GetNodes(int nRange, int nCenterX, int nCenterY)
    {
        //¿Ü°¢¼± ÁÂÇ¥
        List<Node> nodes = new();
        Vector2Int center = new Vector2Int(nCenterX, nCenterY);

        for (int index = 1; index < nRange; index++)
        {
            for (int i = 0; i < index; i++)
            {
                if (SerchNodes(nodes, center, index, i))
                {
                    return nodes;
                }
            }
        }

        return nodes;
    }

    private bool SerchNodes(List<Node> nodes, Vector2Int center, int index, int i)
    {
        GridManager grid = PathRequestManager.Instance.Grid;
        Vector2Int[] points = GetNeighborNodes(center, index, i);

        foreach (var point in points)
        {
            Node checkNode = grid.CheckNode(point.x, point.y);
            if (checkNode != null)
            {
                if (checkNode.occupation == "Player")
                {
                    if (nodes.Count > 0)
                    {
                        nodes.Clear();
                    }
                    nodes.Add(checkNode);
                    return true;
                }

                nodes.Add(checkNode);
            }
        }

        return false;
    }

    private Vector2Int[] GetNeighborNodes(Vector2Int center, int index, int i)
    {
        Vector2Int[] points = new Vector2Int[4];
        
        points[0] = new Vector2Int(center.x - i, center.y - index + i);
        points[1] = new Vector2Int(center.x - index + i, center.y + i);
        points[2] = new Vector2Int(center.x + i, center.y + index - i);
        points[3] = new Vector2Int(center.x + index - i, center.y - i);
        
        return points;
    }




    // private void OnDrawGizmos()
    // {
    //     if (walkableNode != null)
    //     {
    //         foreach (var node in walkableNode)
    //         {
    //             Gizmos.color = Color.green;
    //             Gizmos.DrawCube(node.Position, new Vector3(1, 1, 1));
    //
    //         }
    //     }
    //
    //
    //     if (other != null)
    //     {
    //
    //         Gizmos.color = Color.blue;
    //         Gizmos.DrawCube(other.Position, new Vector3(1, 1, 1));
    //     }
    //
    //     if (myNode != null)
    //     {
    //
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawCube(myNode.Position, new Vector3(1, 1, 1));
    //     }
    //
    //
    // }


}
