using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject[] inventoryIcons;

    void OnTriggerEnter(Collider other)
    {
        foreach(Transform child in inventoryPanel.transform)
        {
            if(child.gameObject.tag == other.gameObject.tag)
            {
                string c = child.Find("Text").GetComponent<Text>().text;
                int tcount = System.Int32.Parse(c) + 1;
                child.Find("Text").GetComponent<Text>().text = "" + tcount;
                return;
            }
        }

        GameObject i;
        if(other.gameObject.tag == "red")
        {
            i = Instantiate(inventoryIcons[0]);
            i.transform.SetParent(inventoryPanel.transform);
        }
        else if(other.gameObject.tag == "green")
        {
            i = Instantiate(inventoryIcons[1]);
            i.transform.SetParent(inventoryPanel.transform);
        }
        else if(other.gameObject.tag == "blue")
        {
            i = Instantiate(inventoryIcons[2]);
            i.transform.SetParent(inventoryPanel.transform);
        }
    }

}
