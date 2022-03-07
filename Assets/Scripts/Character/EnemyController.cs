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

        for (int index = 1; index < nRange; index++)
        {
            for (int i = 0; i < index; i++)
            {

                Vector2Int point = new Vector2Int(nCenterX - i, nCenterY - index + i);
                CheckNode(nodes, point);

                point = new Vector2Int(nCenterX - index + i, nCenterY + i);
                CheckNode(nodes, point);

                point = new Vector2Int(nCenterX + i, nCenterY + index - i);
                CheckNode(nodes, point);

                point = new Vector2Int(nCenterX + index - i, nCenterY - i);
                CheckNode(nodes, point);

            }
        }



        return nodes;
    }

    private void CheckNode(List<Node> nodes, Vector2Int point)
    {
        GridManager grid = PathRequestManager.Instance.Grid;

        if ((0 <= point.x && point.x < grid.nodeXCount) && (0 <= point.y && point.y < grid.nodeYCount))
        {
            if (grid.grid[point.x, point.y].Walkable)
            {
                nodes.Add(grid.grid[point.x, point.y]);
            }
        }
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
