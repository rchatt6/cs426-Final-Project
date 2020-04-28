using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Mirror;


public class ShootingErrorTesting : MonoBehaviour
{
	
	private int damage = 25;
	private float range = 200;
	[SerializeField] private Transform camTransform;
	private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        /*if (!isServer)
        {
            CmdSendPlayerHit(hit);
			CmdSendPlayerCam(camTransform);
			//CmdSendPlayerCheckifShooting();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!isLocalPlayer)
        {
            return;
        }*/
		/*if (!isServer)
        {
            CmdSendPlayerHit(hit);
			CmdSendPlayerCam(camTransform);
			//CmdSendPlayerCheckifShooting();
			//CheckifShooting();
        }*/
        CheckifShooting();
    }
	
	void CheckifShooting(){
		/*if(!isLocalPlayer){
			return;
		}*/
		/*if (!isServer)
        {
            CmdSendPlayerHit(hit);
			CmdSendPlayerCam(camTransform);
			//CmdSendPlayerCheckifShooting();
			/*if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				Debug.Log("mag: " + WeaponScript.mag);
				if (WeaponScript.mag > 0)
				{
					Shoot();
				}
			
			}*/
        //}*/
		
		if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("mag: " + WeaponScript.mag);
            if (WeaponScript.mag > 0)
            {
                Shoot();
            }
			
		}
	}
	
	/*[Command]
	void CmdSendPlayerHit(RaycastHit hitt){
		RpcUpdatePlayerHit(hitt);
	}
	
	[ClientRpc]
	void RpcUpdatePlayerHit(RaycastHit hitt){
		hit = hitt;
	}
	
	[Command]
	void CmdSendPlayerCam(Transform cam){
		RpcUpdatePlayerCam(cam);
	}
	
	[ClientRpc]
	void RpcUpdatePlayerCam(Transform cam){
		camTransform = cam;
	}
	[Command]
	void CmdSendPlayerCheckifShooting(){
		RpcUpdateCheckifShooting();
	}
	
	[ClientRpc]
	void RpcUpdateCheckifShooting(){
		CheckifShooting();
	}*/
	
	void Shoot(){
		if(Physics.Raycast(camTransform.TransformPoint(0,0,0.5f), camTransform.forward, out hit, range)){
			//Debug.Log(hit.transform.tag);
			
			if(hit.transform.tag == "Zombie"){
				string uIdentity = hit.transform.name;
				CmdTellServerWhoWasShot(uIdentity, damage);
			}
			
			/*if(hit.transform.tag == "Player"){
				string uIdentity = hit.transform.name;
				CmdTellServerWhichPlayerWasShot(uIdentity, damage);
			}*/
		}
	}
	
	//[Command]
	void CmdTellServerWhoWasShot(string uniqueID, int dmg){
		GameObject go = GameObject.Find(uniqueID);
		go.GetComponent<NPCHealthRaycast>().TakeDamage(dmg);
		//Apply dammage to that player
	}
	
//	[Command]
//	void CmdTellServerWhoWasShot(string uniqueID, int dmg){
//		/*GameObject go = GameObject.Find(uniqueID);
//		go.GetComponent<NPCHealthRaycast>().TakeDamage(dmg);*/
//		RpcTellClientWhoWasShot(uniqueID, dmg);
//		//Apply dammage to that player
//	}
	
/*	[ClientRpc]
	void RpcTellClientWhoWasShot(string uniqueID, int dmg){
		GameObject go = GameObject.Find(uniqueID);
		go.GetComponent<NPCHealthRaycast>().TakeDamage(dmg);
		//Apply dammage to that player
	}*/
	
//	[Command]
//	void CmdTellServerWhichPlayerWasShot(string uniqueID, int dmg){
//		/*GameObject go = GameObject.Find(uniqueID);
//		go.GetComponent<NPCHealthRaycast>().TakeDamage(dmg);*/
//		RpcTellClientWhichPlayerWasShot(uniqueID, dmg);
//		//Apply dammage to that player
//	}
	
/*	[ClientRpc]
	void RpcTellClientWhichPlayerWasShot(string uniqueID, int dmg){
		GameObject go = GameObject.Find(uniqueID);
		go.GetComponent<Health>().TakeDamage(dmg, go.GetComponent<Collider>());
		//Apply dammage to that player
	}*/
}
