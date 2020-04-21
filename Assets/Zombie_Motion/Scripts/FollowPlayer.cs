using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    Animator anim;
    Collider m_Collider;
    Rigidbody rb;
    Transform t;

    float distance;
    public float playerViewDistance;
    public float playerFollowDistance;

    float distance1;
    float distance2;

    int frame;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();

        if (player)
        {
            distance = Vector3.Distance(this.transform.position, player.transform.position);
            distance1 = Vector3.Distance(this.transform.position, player.transform.position);
            distance2 = Vector3.Distance(this.transform.position, player.transform.position);
        }

        agent.speed = 3.8f;
        frame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            distance = Vector3.Distance(this.transform.position, player.transform.position);

            if ((distance <= playerViewDistance) && (distance >= playerFollowDistance))
            {
                FindTarget();
            }
            else
            {
                frame = 0;
                agent.enabled = true;
                agent.SetDestination(this.transform.position);
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isJumping", false);
            }
        }
    }

    private void FindTarget()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(player.transform.position);
        }

        //Debug.Log("distance: " + distance);
        //Debug.Log("distance1: " + distance1);
        //Debug.Log("distance2: " + distance2);

        if (frame == 1)
        {
            distance1 = Vector3.Distance(this.transform.position, player.transform.position);
        }
        else if (frame == 140)
        {
            distance2 = Vector3.Distance(this.transform.position, player.transform.position);

            if (Mathf.Abs(distance1 - distance2) <= 0.8f)
            {
                //Debug.Log("JUMP!!!");
                agent.speed = 3.8f;
                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isJumping", true);
                agent.enabled = false;
                rb.AddForce(t.up * 500f);
                //rb.AddForce(t.forward * 400f);
                rb.AddRelativeForce(Vector3.forward * 1200f);
            }


        }
        else if (frame >= 200)
        {
            //Debug.Log("Done!");
            frame = 0;
            anim.SetBool("isJumping", false);
            anim.SetBool("isWalking", true);
            agent.enabled = true;
        }

        frame++;

        if (distance <= playerFollowDistance)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
        }
        else if (distance > playerFollowDistance && distance < playerViewDistance / 2)
        {
            agent.speed = 3.8f;
            anim.SetBool("isWalking", true);
            anim.SetBool("isRunning", false);
        }
        else
        {
            agent.speed = 7.6f;
            anim.SetBool("isRunning", true);
            anim.SetBool("isWalking", false);
        }
    }
}
