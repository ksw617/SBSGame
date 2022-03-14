using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    public float speed = 3f;
    protected Action callback;

    protected Node targetNode;
    protected Node currentNode;

    private Stack<Vector2Int> points = new();

    private int waitCount;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        currentNode = PathRequestManager.Instance.Grid.GetNodeFromPosition(transform.position);
        currentNode.occupation = gameObject.name;
    }

    protected void CallFollowTarget(Stack<Vector2Int> path, bool isSuccess)
    {
        if (isSuccess)
        {   
            points = path;
            GoNextNode();
        }
    }
    void GoNextNode()
    {
        if (points.Count > 0)
        {
            Vector2Int next = points.Pop();
            Node nextNode = PathRequestManager.Instance.Grid.GetNode(next);

            waitCount = 0;
            CheckNextStep(nextNode);        
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            currentNode.State = NodeState.Block;
            callback?.Invoke();// Stop
        }

    }

    void CheckNextStep(Node nextNode)
    {
        switch (nextNode.State)
        {
            case NodeState.Empty:
                //이동
                StartCoroutine(Move(nextNode));
                break;
            case NodeState.Pass:
                StartCoroutine(Wait(nextNode));
                break;
            case NodeState.Block:
                ReserchPath();
                break;
        }
    }
    IEnumerator Move(Node nextNode)
    {
        waitCount = 0;

        animator.SetFloat("Speed", 1f);
        transform.LookAt(nextNode.Position);

        currentNode.occupation = string.Empty;
        currentNode.State = NodeState.Empty;
        currentNode = nextNode;
        currentNode.occupation = gameObject.name;
        currentNode.State = NodeState.Pass;
       
        //구현

        while (Vector3.Distance(transform.position, nextNode.Position) >= 0.1f)
        {
            //이동
            transform.position = Vector3.MoveTowards(transform.position, nextNode.Position, Time.fixedDeltaTime * speed);

            yield return new WaitForFixedUpdate();

        }

        GoNextNode();

        yield return null;
    }

    IEnumerator Wait(Node nextNode)
    {
        animator.SetFloat("Speed", 0f);
        waitCount++;
        if (waitCount > 5)
        {
            ReserchPath();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log($"기다림 {gameObject.name}");
            CheckNextStep(nextNode);
        }


        yield return null;

    }

    private void ReserchPath()
    {
        waitCount = 0;
        Debug.Log($"재탐색 {gameObject.name}");
        PathRequestManager.Instance.RequestPath(transform.position, targetNode.Position, CallFollowTarget);
    }

}
