using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class removeItem : MonoBehaviour
{
    public void eatMe()
    {
        if(System.Int32.Parse(this.transform.Find("Text").GetComponent<Text>().text) > 1){
            int tcount = System.Int32.Parse(this.transform.Find("Text").GetComponent<Text>().text) - 1;
            this.transform.Find("Text").GetComponent<Text>().text = "" + tcount;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
