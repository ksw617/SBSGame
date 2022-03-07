using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyController : Character
{
    public Grid grid;
    public Transform player;
    Node other;
    Node myNode;
    List<Node> walkableNode;


    private void Start()
    {
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

        myNode = grid.GetNodeFromPosition(transform.position);
        Node playerNode = grid.GetNodeFromPosition(player.position);
        walkableNode = GetNodes(15, myNode.X, myNode.Y, playerNode);

        int randIndex = Random.Range(0, walkableNode.Count);
        other = walkableNode[randIndex];

        PathRequestManager.Instance.RequestPath(transform.position, other.Position, CallFollowTarget);
        yield return null;
    }


    private List<Node> GetNodes(int nRange, int nCenterX, int nCenterY, Node playerNode)
    {
        //¿Ü°¢¼± ÁÂÇ¥
        List<Node> nodes = new();
        Vector2Int playerPosition = new Vector2Int(playerNode.X, playerNode.Y);

        for (int index = 1; index < nRange; index++)
        {
            for (int i = 0; i < index; i++)
            {

                Vector2Int point = new Vector2Int(nCenterX - i, nCenterY - index + i);
                if (playerPosition == point)
                {
                    nodes.Clear();
                    nodes.Add(playerNode);
                    return nodes;
                }
                CheckNode(nodes, point);

                point = new Vector2Int(nCenterX - index + i, nCenterY + i);
                if (playerPosition == point)
                {
                    nodes.Clear();
                    nodes.Add(playerNode);
                    return nodes;
                }
                CheckNode(nodes, point);

                point = new Vector2Int(nCenterX + i, nCenterY + index - i);
                if (playerPosition == point)
                {
                    nodes.Clear();
                    nodes.Add(playerNode);
                    return nodes;
                }
                CheckNode(nodes, point);

                point = new Vector2Int(nCenterX + index - i, nCenterY - i);
                if (playerPosition == point)
                {
                    nodes.Clear();
                    nodes.Add(playerNode);
                    return nodes;
                }
                CheckNode(nodes, point);

            }
        }



        return nodes;
    }

    private void CheckNode(List<Node> nodes, Vector2Int point)
    {
        if ((0 <= point.x && point.x < grid.nodeXCount) && (0 <= point.y && point.y < grid.nodeYCount))
        {
            if (grid.grid[point.x, point.y].Walkable)
            {
                nodes.Add(grid.grid[point.x, point.y]);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (walkableNode != null)
        {
            foreach (var node in walkableNode)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(node.Position, new Vector3(1, 1, 1));

            }
        }


        if (other != null)
        {

            Gizmos.color = Color.blue;
            Gizmos.DrawCube(other.Position, new Vector3(1, 1, 1));
        }

        if (myNode != null)
        {

            Gizmos.color = Color.red;
            Gizmos.DrawCube(myNode.Position, new Vector3(1, 1, 1));
        }


    }


}
