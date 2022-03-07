using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    public Vector2 gridWorldSize;
    public LayerMask block;

    public Node[,] grid;
    public float nodeRadius;
    [SerializeField] private float nodeDiameter;
    public int nodeXCount, nodeYCount;

    public int GridCount { get => nodeXCount * nodeYCount; }

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2f;

        nodeXCount = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        nodeYCount = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        grid = new Node[nodeXCount, nodeYCount]; 

        Vector3 bottomLeft = transform.position - new Vector3(gridWorldSize.x * 0.5f, 0f, gridWorldSize.y * 0.5f);

        for (int x = 0; x < nodeXCount; x++)
        {
            for (int y = 0; y < nodeYCount; y++)
            {
                Vector3 worldPositon = bottomLeft + new Vector3((x * nodeDiameter + nodeRadius), 0f, (y * nodeDiameter + nodeRadius));

                bool walkable = !(Physics.CheckSphere(worldPositon, nodeRadius, block));

                grid[x, y] = new Node(worldPositon, x, y, walkable);
            }
        }
    }

    public List<Node> GetNeighbors(Node currentNode)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1 ; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = currentNode.X + x;
                int checkY = currentNode.Y + y;

                if ((0 <= checkX && checkX < nodeXCount) && (0 <= checkY && checkY < nodeYCount))
                {
                    Node checkNode = grid[checkX, checkY];
                    if (checkNode.Walkable)
                    {
                        neighbors.Add(checkNode);
                    }
                }
            }
        }

        return neighbors;
    }

    
    public Node GetNodeFromPosition(Vector3 pos)
    {
        float percentX = ((pos.x - nodeRadius) / gridWorldSize.x + 0.5f);
        float percentY = ((pos.z - nodeRadius) / gridWorldSize.y + 0.5f);

        int x = Mathf.RoundToInt((gridWorldSize.x) * percentX);
        int y = Mathf.RoundToInt((gridWorldSize.y) * percentY);

        x = Mathf.Clamp(x, 0, nodeXCount - 1);
        y = Mathf.Clamp(y, 0, nodeYCount - 1);

        return grid[x, y];
    }
 
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 0.5f, gridWorldSize.y));

        if (grid != null)
        {
            foreach(var node in grid)
            {
                Gizmos.color = node.Walkable ?  Color.white : Color.black;

                switch (node.occupation)
                {
                    case "Player":
                        Gizmos.color = Color.blue;
                        break;
                    case "Enemy":
                        Gizmos.color = Color.red;
                        break;
                    default:
                        break;
                }

                Gizmos.DrawCube(node.Position, new Vector3(0.9f, 0.1f, 0.9f));
            }
        }
    }


}
