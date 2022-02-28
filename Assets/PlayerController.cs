using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public float speed = 3f;

    public PathRequestManager pathRequestManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FollowPath(Vector3 targetPosition)
    {
        pathRequestManager.RequestPath(transform.position, targetPosition, CallFollowTarget);
    }

    private void CallFollowTarget(Stack<Vector3> path, bool isSuccess)
    {
        if (isSuccess)
        {
            StopCoroutine("FollowTarget");
            StartCoroutine("FollowTarget", path);
        }
    }


    IEnumerator FollowTarget(Stack<Vector3> points)
    {
        Debug.Log(points.Count);
        Vector3 nextPosition = points.Pop();
        transform.LookAt(nextPosition);
        animator.SetFloat("Speed", 1f);

       while (points.Count > 0)
       {
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.fixedDeltaTime * speed);
       
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

            if (Vector3.Distance(transform.position, nextPosition) <= 0.1f)
            {
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        animator.SetFloat("Speed", 0f);
        yield return null;
    }

   
}
