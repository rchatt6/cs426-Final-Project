using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerDamage : MonoBehaviour
{

    public int health = 100;
    int waterDamage = 20;
    //int zombieDamage = 50;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(health);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            health -= waterDamage;
            Debug.Log(health);
            if (health <= 0)
            {
                Debug.Log("you died" + health);
                Destroy(other.gameObject);
            }
        }
        /*
        if(this.tag == "DeathPlane")
        {
            health -= waterDamage;
            if (health <= 0)
            {
                Destroy(other.gameObject);
            }
        }

        if(this.tag == "Zombie")
        {
            health -= zombieDamage;
            if (health <= 0)
            {
                Destroy(other.gameObject);
            }
        }
        */
    }
}
