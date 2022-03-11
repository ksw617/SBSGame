using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    public float speed = 3f;
    protected Action callback;
    private Coroutine followTarget;

    protected Node currentNode;

    private Stack<Vector2Int> points = new();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        followTarget = null;
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
            if (followTarget != null)
            {
                StopCoroutine(followTarget);
            }

            animator.SetFloat("Speed", 1f);
            points = path;
            followTarget = StartCoroutine(GoNextNode());
        }
    }

    Node nextNode;
    IEnumerator GoNextNode()
    {
        Vector2Int next = points.Pop();
        nextNode = PathRequestManager.Instance.Grid.GetNode(next);

        switch (nextNode.State)
        {
            case NodeState.Empty:
                //이동
                StartCoroutine(Move(nextNode));
                break;
            case NodeState.Pass:
                //0.5초 기다림
                break;
            case NodeState.Block:
                //재탐색
                break;
        }
        transform.LookAt(nextNode.Position);


        

        animator.SetFloat("Speed", 0f);

        callback?.Invoke();
        followTarget = null;
        yield return null;
    }

    IEnumerator Move(Node nextNode)
    {

        //currentNode.occupation = string.Empty;
        //currentNode.State = NodeState.Empty;
        //currentNode = nextNode;
        //currentNode.occupation = gameObject.name;
        //
        //currentNode.State = NodeState.Pass;
        //구현

        while (Vector3.Distance(transform.position, nextNode.Position) >= 0.1f)
        {
            //이동
            transform.position = Vector3.MoveTowards(transform.position, nextNode.Position, Time.fixedDeltaTime * speed);

            yield return new WaitForFixedUpdate();

        }

        StartCoroutine(GoNextNode());

        yield return null;
    }

    private void OnDrawGizmos()
    {
        if (nextNode != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(nextNode.Position, new Vector3(1, 1, 1));
        }
    }


}
