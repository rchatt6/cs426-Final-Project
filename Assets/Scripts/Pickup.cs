using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Pickup : NetworkBehaviour
{
    float throwForce = 600;
	Vector3 objectPos;
	float distance;
	
	public bool canhold = true;
	public GameObject item;
	private GameObject tempParent;
	public bool isHolding = false;
	
	void Update ()
	{
        //Debug.Log("isClient: " + isClient);

        if (!isClient)
            return;

        tempParent = GameObject.FindWithTag("Destination");
        //Debug.Log("tempParent: " + tempParent);

        if (tempParent)
        {
            distance = Vector3.Distance(item.transform.position, tempParent.transform.position);
            if (distance >= 4f)
            {
                isHolding = false;
            }
            //Check of isHolding
            if (isHolding == true)
            {
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                item.transform.SetParent(tempParent.transform);


                if (Input.GetMouseButtonDown(1))
                {
                    //throw
                    item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
                    isHolding = false;
                }
            }

            else
            {
                objectPos = item.transform.position;
                item.transform.SetParent(null);
                item.GetComponent<Rigidbody>().useGravity = true;
                item.transform.position = objectPos;
            }
        }
	}
	
	void OnMouseDown()
	{
        if (!isClient)
            return;

        if (distance <= 4f)
		{
			isHolding = true;
			item.GetComponent<Rigidbody>().useGravity = false;
			item.GetComponent<Rigidbody>().detectCollisions = true;
            //item.GetComponent<Rigidbody>().freezeRotation = false;
            //item.GetComponent<Rigidbody>().isKinematic = true;
        }
	}
	
	void OnMouseUp()
	{
        if (!isClient)
            return;

        isHolding = false;
        item.GetComponent<Rigidbody>().useGravity = true;
        //item.GetComponent<Rigidbody>().freezeRotation = true;
    }
}