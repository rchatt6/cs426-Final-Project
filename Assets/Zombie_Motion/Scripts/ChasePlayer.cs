using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    Animator anim;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        distance = Vector3.Distance(this.transform.position, player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(this.transform.position, player.transform.position);
        Debug.Log(distance);
        if (distance <= 20)
        {
            FindTarget();
        }
        else
        {
            agent.SetDestination(this.transform.position);
            anim.SetBool("isMoving", false);
        }
    }

    private void FindTarget()
    {
        agent.SetDestination(player.transform.position);
        
        if (agent.remainingDistance <= 2)
        {
            anim.SetBool("isMoving", false);
        }
        else
        {

            anim.SetBool("isMoving", true);
        }
    }
}
