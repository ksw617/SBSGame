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

    Vector3 nextPosition;
    IEnumerator FollowTarget(Stack<Vector3> points)
    {
        nextPosition = points.Pop();
        transform.LookAt(nextPosition);
        animator.SetFloat("Speed", 1f);

        float tickTime = 0f;

        while (true)
        {
            //움직임
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.fixedDeltaTime * speed);
         
            //25번 돌때
            if (tickTime > 0.5f)
            {
                //1번 도는거

                //노드 업데이트
                Node nextNode = PathRequestManager.Instance.Grid.GetNodeFromPosition(transform.position);
                if (currentNode != nextNode)
                {
                    currentNode.occupation = string.Empty;
                    currentNode = nextNode;
                    currentNode.occupation = gameObject.name;
                }

                //목적지 도착 확인 & 업데이트
                if (Vector3.Distance(transform.position, nextPosition) <= 0.1f)
                {
                    if (points.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        nextPosition = points.Pop();
                        transform.LookAt(nextPosition);
                    }
                }

                tickTime = 0f;
            }

            tickTime += Time.fixedDeltaTime; // 0.02f

            yield return new WaitForFixedUpdate(); // 0.02초 씩 

        }

        animator.SetFloat("Speed", 0f);

        callback?.Invoke();
        followTarget = null;
        yield return null;
    }

     private void OnDrawGizmos()
     {

    
         if (nextPosition != null)
         {
    
             Gizmos.color = Color.green;
             Gizmos.DrawCube(nextPosition, new Vector3(1, 1, 1));
         }
    

     }

}
