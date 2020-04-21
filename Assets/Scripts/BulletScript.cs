using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BulletScript : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Zombie")
        {
            Destroy(collision.gameObject);

            gameObject.SetActive(false);
        }
    }


}
