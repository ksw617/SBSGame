using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class PathRequestManager : MonoBehaviour
{
    [SerializeField] private PathFinding pathFinding;
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    bool isProcessing;

    private void Awake()
    {
        pathFinding = GetComponent<PathFinding>();
        isProcessing = false;
    }

    public void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Stack<Vector3>, bool> pathCallbak)
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

    public void FinishPathfinding(Stack<Vector3> path, bool success)
    {
        isProcessing = false;

        currentPathRequest.pathCallback(path, success);

        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Stack<Vector3>, bool> pathCallback;

        public PathRequest(Vector3 start, Vector3 end, Action<Stack<Vector3>, bool> callback)
        {
            pathStart = start;
            pathEnd = end;
            pathCallback = callback;
        }
    }
  
}
