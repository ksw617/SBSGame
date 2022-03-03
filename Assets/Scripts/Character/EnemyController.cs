using UnityEngine;
using System.Collections;
public class EnemyController : Character
{
    public Grid grid;
    Node other;
    Node myNode;

    private void Start()
    {
        FindOtherDestination();

        callback = FindOtherDestination;
    }

    void FindOtherDestination()
    {
        Debug.Log("1");

        StartCoroutine("StartFindDestination");

    }

    IEnumerator StartFindDestination()
    {
        myNode = grid.GetNodeFromPosition(transform.position);
        Vector2Int myPosition = new Vector2Int(myNode.X, myNode.Y);
        do
        {
            Vector2Int randPos = GetRandomPosition(myPosition, 3);
            other = grid.grid[randPos.x, randPos.y];
            yield return new WaitForFixedUpdate();
        }
        while (!other.Walkable);

        PathRequestManager.Instance.RequestPath(transform.position, other.Position, CallFollowTarget);
    }

    int RandomSign { get => Random.Range(0, 2) == 0 ? -1 : 1; }

    Vector2Int GetRandomPosition(Vector2Int center, int range)
    {
        Vector2Int checkPoint;
        do
        {
            int randomX, randomY;
            do
            {
                randomX = Random.Range(0, range + 1);
                randomY = Random.Range(0, range - randomX + 1);
            }
            while (randomX == 0 && randomY == 0);

            checkPoint = new Vector2Int(randomX * RandomSign, randomY * RandomSign) + center;

        }
        while ((0 > checkPoint.x || checkPoint.x >= grid.nodeXCount) || (0 > checkPoint.y || checkPoint.y >= grid.nodeYCount));

        return checkPoint;
    }

    private void OnDrawGizmos()
    {
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
