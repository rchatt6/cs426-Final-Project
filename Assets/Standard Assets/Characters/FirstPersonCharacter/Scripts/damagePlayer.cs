using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagePlayer : MonoBehaviour
{
    public int health = 100;
    int damage = 100;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(health);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "DeathPlane")
        {
            Destroy(this.gameObject);
            health -= damage;
            Debug.Log("you died" + health);

        }
    }
}
