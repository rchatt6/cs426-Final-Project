using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyBoxRotate : MonoBehaviour
{
    public float speedMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Sets the float value of "_Rotation", adjust it by Time.time and a multiplier.
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * speedMultiplier);
    }
}
