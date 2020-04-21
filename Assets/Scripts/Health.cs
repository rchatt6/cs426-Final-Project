using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    public const int maxHealth = 100;
    [SyncVar(hook ="UpdateHealth")]public int currentHealth = maxHealth;

    [SerializeField]
    private Stats health;

    private GameObject respawnPoint;

    public static int teamNum;

    private void Awake()
    {
        if (!isLocalPlayer)
            return;

        /*if (NetworkServer.connections.Count % 2 == 0)
        {
            teamNum = 2;
        }
        else
        {
            teamNum = 1;
        }

        Debug.Log("Initialized! You are on team " + teamNum +"!");

        health.Initialize();*/
    }

    void Start()
    {
        if (!isLocalPlayer)
            return;

        if (NetworkServer.connections.Count % 2 == 0)
        {
            teamNum = 2;
        }
        else
        {
            teamNum = 1;
        }

        Debug.Log("Initialized! You are on team " + teamNum + "!");

        health.Initialize();

        currentHealth = maxHealth;
        health.MaxVal = maxHealth;
        health.currentVal = maxHealth;

        //Debug.Log("Start!");
    }

    public void UpdateHealth(int old, int current)
    {
        //old = current;
        current = old;
    }

    public void TakeDamage(int amount, Collider collider)
    {

        if (!isServer)
        {
            return;
        }

        currentHealth -= amount;
        health.currentVal -= amount;

        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            health.currentVal = 0;
            Debug.Log("DEAD!");
            if (teamNum == 1)
            {
                respawnPoint = GameObject.FindGameObjectWithTag("Respawn1");
            }
            else
            {
                respawnPoint = GameObject.FindGameObjectWithTag("Respawn2");
            }
            
            collider.transform.position = respawnPoint.transform.position;
            currentHealth = 100;
            health.currentVal = 100;
        }
    }
}
