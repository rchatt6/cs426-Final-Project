using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    InventoryUI inventory;
    // Start is called before the first frame update
    void Start()
    {
        //inventory = InventoryUI.instance;
        //inventory.onItemChangedCallback += UpdateUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        Debug.Log("UPDATING UI");
    }
}
