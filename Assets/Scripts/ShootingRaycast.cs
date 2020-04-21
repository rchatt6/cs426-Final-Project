using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class ShootingRaycast : NetworkBehaviour
{
	
	private int damage = 25;
	private float range = 200;
	[SerializeField] private Transform camTransform;
	private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CheckifShooting();
    }
	
	void CheckifShooting(){
		if(!isLocalPlayer){
			return;
		}
		
		if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("mag: " + WeaponScript.mag);
            if (WeaponScript.mag > 0)
            {
                Shoot();
            }
			
		}
	}
	
	void Shoot(){
		if(Physics.Raycast(camTransform.TransformPoint(0,0,0.5f), camTransform.forward, out hit, range)){
			//Debug.Log(hit.transform.tag);
			
			if(hit.transform.tag == "Zombie"){
				string uIdentity = hit.transform.name;
				CmdTellServerWhoWasShot(uIdentity, damage);
			}
		}
	}
	
	[Command]
	void CmdTellServerWhoWasShot(string uniqueID, int dmg){
		GameObject go = GameObject.Find(uniqueID);
		go.GetComponent<NPCHealthRaycast>().TakeDamage(dmg);
		//Apply dammage to that player
	}
}
