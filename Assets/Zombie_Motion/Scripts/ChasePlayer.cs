using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class ChasePlayer : NetworkBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    Animator anim;
    Collider m_Collider;
    float distance;
    int frame;
    int frame2;
    int frame3;
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip attackSound;
    Vector3 randPosition;

    public float enemyViewDistance;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindWithTag("Player");
        //players = GameObject.FindGameObjectsWithTag("Player");


        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();

        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            if (c.gameObject != gameObject && c.gameObject.name == "Hitbox")
            {
                m_Collider = c;
            }
        }

        foreach (AudioSource c in GetComponentsInChildren<AudioSource>())
        {
            if (c.gameObject != gameObject && c.gameObject.name == "AudioAttack")
            {
                m_AudioSource = c;
            }
        }

        //m_Collider = GetComponentInChildren<Collider>();
        m_Collider.enabled = false;
        //m_AudioSource = GetComponentInChildren<AudioSource>();
        //distance = Vector3.Distance(this.transform.position, player.transform.position);
        randPosition = this.transform.position;
        agent.speed = 3f;
        frame = 0;
        frame2 = 0;
        frame3 = 0;

        //StartCoroutine("DoCheck");
        InvokeRepeating("Update1", 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }
    }

    private void Update1()
    {
        if (!isServer)
        {
            return;
        }

        if (anim.GetBool("isDying") == true)
        {
            agent.speed = 0;
            return;
        }

        player = FindClosestPlayer();

        //Debug.Log(frame3);

        if (player)
        {
            distance = Vector3.Distance(this.transform.position, player.transform.position);

            if (distance <= enemyViewDistance)
            {
                FindTarget();
            }
            else
            {
                if (frame3 >= 20)
                {
                    float rng1 = Random.Range(-4f, 4f);
                    float rng2 = Random.Range(-4f, 4f);
                    agent.speed = 2f;
                    randPosition = new Vector3(this.transform.position.x + rng1, this.transform.position.y, this.transform.position.z + rng2);
                    agent.SetDestination(randPosition);
                    anim.SetBool("isMoving", true);
                    frame3 = 0;
                }

                distance = Vector3.Distance(this.transform.position, randPosition);

                if (distance <= 0.5)
                {
                    anim.SetBool("isMoving", false);
                }

                anim.SetBool("isAttacking", false);
                m_Collider.enabled = false;
                frame = 0;
                frame3++;
            }
        }
    }

    private GameObject FindClosestPlayer()
    {
        GameObject[] players;

        players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closestPlayer = null;
        float dist = Mathf.Infinity;
        float currDist = Mathf.Infinity;

        foreach (GameObject g in players)
        {
            currDist = Vector3.Distance(this.transform.position, g.transform.position);

            if (currDist < dist)
            {
                closestPlayer = g;
                dist = currDist;
            }
        }

        return closestPlayer;
    }

    /*IEnumerator DoCheck()
    {
        for (;;)
        {
            // execute block of code here
            yield return new WaitForSeconds(10f);
        }
    }*/

    private void FindTarget()
    {
        agent.SetDestination(player.transform.position);
        //Debug.Log("frame1 = " + frame2 + "frame2 = " + frame2);

        if (frame >= 13)
        {
            frame = 0;
        }
        if (frame2 == 7)
        {
            m_AudioSource.clip = attackSound;
            m_AudioSource.Play();
        }
        else if (frame2 >= 13)
        {
            frame2 = 0;
        }

        if (distance <= 1.8f)
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", true);
            agent.speed = 0.2f;
            frame = -3;
            frame2++;
            if (frame2 > 7 && frame2 <= 10)
            {
                m_Collider.enabled = true;
            }
            else
            {
                m_Collider.enabled = false;
            }
        }
        else if (frame < 0)
        {
            frame++;
            frame2++;
            anim.SetBool("isAttacking", false);
            if (frame2 >= 7 && frame2 <= 10)
            {
                m_Collider.enabled = true;
            }
            else
            {
                m_Collider.enabled = false;
            }
        }
        else
        {
            anim.SetBool("isMoving", true);
            anim.SetBool("isAttacking", false);
            frame++;
            frame2 = 0;
            m_Collider.enabled = false;
            if (frame % 20 <= 10)
            {
                agent.speed = 3f;
            }
            else
            {
                agent.speed = 0.2f;
            }

        }
    }
}
