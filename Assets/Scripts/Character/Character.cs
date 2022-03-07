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


    protected void CallFollowTarget(Stack<Vector3> path, bool isSuccess)
    {
        if (isSuccess)
        {
            if (followTarget != null)
            {
                StopCoroutine(followTarget);
            }

            followTarget = StartCoroutine(FollowTarget(path));
        }
    }


    IEnumerator FollowTarget(Stack<Vector3> points)
    {
        Vector3 nextPosition = points.Pop();
        transform.LookAt(nextPosition);
        animator.SetFloat("Speed", 1f);

        while (points.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.fixedDeltaTime * speed);

            Node nextNode = PathRequestManager.Instance.Grid.GetNodeFromPosition(transform.position);
            if (currentNode != nextNode)
            {
                currentNode.occupation = string.Empty;
                currentNode = nextNode;
                currentNode.occupation = gameObject.name;
            }

            if (Vector3.Distance(transform.position, nextPosition) <= 0.1f)
            {
                nextPosition = points.Pop();
                transform.LookAt(nextPosition);
            }

            yield return new WaitForFixedUpdate();

        }

        transform.LookAt(nextPosition);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.fixedDeltaTime * speed);

            Node nextNode = PathRequestManager.Instance.Grid.GetNodeFromPosition(transform.position);
            if (currentNode != nextNode)
            {
                currentNode.occupation = string.Empty;
                currentNode = nextNode;
                currentNode.occupation = gameObject.name;
            }

            if (Vector3.Distance(transform.position, nextPosition) <= 0.1f)
            {
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        animator.SetFloat("Speed", 0f);


        //if (callback != null)
        //{
        //    callback();
        //}

        callback?.Invoke();
        followTarget = null;
        yield return null;
    }
}
