using System.Collections.Generic;
using UnityEngine;
using System;



public class PathRequestManager : MonoBehaviour
{
    private static PathRequestManager instance;
    public static PathRequestManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PathRequestManager>();
            }

            return instance;
        }
    }
    [SerializeField] private PathFinding pathFinding;
    public GridManager Grid { get; private set; }
    
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    bool isProcessing;

    private void Awake()
    {
        pathFinding = GetComponent<PathFinding>();
        Grid = GetComponent<GridManager>();

        isProcessing = false;
    }

    public void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Stack<Vector2Int>, bool> pathCallbak)
    {
        PathRequest pathRequest = new PathRequest(pathStart, pathEnd, pathCallbak);
        pathRequestQueue.Enqueue(pathRequest);

        TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessing && pathRequestQueue.Count > 0)
        {
            isProcessing = true;
            currentPathRequest = pathRequestQueue.Dequeue();

            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishPathfinding(Stack<Vector2Int> path, bool success)
    {
        isProcessing = false;

        currentPathRequest.pathCallback(path, success);

        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Stack<Vector2Int>, bool> pathCallback;

        public PathRequest(Vector3 start, Vector3 end, Action<Stack<Vector2Int>, bool> callback)
        {
            pathStart = start;
            pathEnd = end;
            pathCallback = callback;
        }
    }
  
}
