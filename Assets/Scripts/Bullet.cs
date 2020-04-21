using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    void OnCollisionEnter(Collision collision)
	{
		GameObject hit = collision.gameObject;
		NPCHealth health = hit.GetComponent<NPCHealth>();
		
		Debug.Log(collision.transform.name);
		
		//var hit = collision.gameObject;
		//var health = hit.GetComponent<NPCHealth>();
		
		if(health != null){
			health.TakeDamage(10);
		}
		
		Destroy(gameObject);
	}
	
	/*void OnTriggerEnter (Collision other){
		
		other.gameObject.GetComponent<NPCHealth>().TakeDamage(10);
	}*/
}
