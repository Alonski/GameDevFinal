using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnightBehavior : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;

    private Animator anim;
    private NavMeshAgent agent;
    public GameObject target;

    public Transform[] groundGuns;
    public GameObject heldGun;

    private GameObject chosenGun;

    private bool hasGun = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0 || anim.GetBool("IsDying"))
        {

            agent.isStopped = true;
            return;
        }

        if (!hasGun)
        {
            var i = 0;
            Transform closestGun = null;
            foreach (var gun in groundGuns)
            {
                if (gun == null)
                {
                    continue;
                }
                var distanceFromGun = getDistanceFromObject(gun);
                var distanceFromClosestGun = getDistanceFromObject(closestGun);
                if (distanceFromGun < distanceFromClosestGun)
                {
                    i++;
                    closestGun = gun;
                }
            }
            chosenGun = closestGun.gameObject;
            GoToPosition(closestGun.position);
            return;
        }

        // Set the agent to go to the currently selected destination.
        GoToPosition(points[destPoint].position);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    void GoToPosition(Vector3 position)
    {
        agent.destination = position;
    }

    // Update is called once per frame
    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            if (chosenGun != null)
            {
                hasGun = true;
                Destroy(chosenGun);

                heldGun.SetActive(true);
            }
            else
            {
                GotoNextPoint();
            }
        }

        //agent.SetDestination(target.transform.position);
        /*
        if(Input.GetKey("x"))
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            if (Input.GetKey("z"))
                anim.SetBool("IsDying", true);
            else
                 anim.SetBool("IsWalking", false);
        }
        */
    }

    private float getDistanceFromObject(Transform objectTransform)
    {
        return objectTransform is null ? float.MaxValue : Vector3.Distance(transform.position, objectTransform.position);
    }
}
