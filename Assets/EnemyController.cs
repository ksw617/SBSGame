using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Grid grid;
    HashSet<Node> randNodes = new HashSet<Node>();

    private void Start()
    {
        Node myNode = grid.GetNodeFromPosition(transform.position);
        StartCoroutine(TestRange(myNode));
    }

    IEnumerator TestRange(Node node)
    {
        Vector2Int myPosition = new Vector2Int(node.X, node.Y);
        while (true)
        {
            Vector2Int randPos =  GetRandomPosition(myPosition, 3);
            Debug.Log($"Add[{randPos.x},{randPos.y}]");
            if (!randNodes.Contains(grid.grid[randPos.x, randPos.y]))
            {
                randNodes.Add(grid.grid[randPos.x, randPos.y]);
            }

            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }

    int randomSign { get => Random.Range(0, 2) == 0 ? -1 : 1; }

    Vector2Int GetRandomPosition(Vector2Int center, int range)
    {
        int randomX = Random.Range(0, range + 1);
        int randomY = Random.Range(0, range - randomX + 1);

        return new Vector2Int(randomX * randomSign, randomY * randomSign) + center;
    }

    private void OnDrawGizmos()
    {
        if (randNodes.Count > 0)
        {
            foreach (var node in randNodes)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(node.Position, new Vector3(1, 1, 1));

            }
        }

    }

}
