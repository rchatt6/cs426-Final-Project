using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ItemPickup : NetworkBehaviour
{
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip pickupSound;
    [SerializeField]
    private int gainValue;
    private bool flag;
    private int frame1;

    void Start()
    {
        //if (!isServer)
        //return;

        m_AudioSource = GetComponent<AudioSource>();
        flag = false;
        frame1 = 0;
        InvokeRepeating("Update1", 0.2f, 0.2f);
    }

    void Update1()
    {

        if (flag)
        {
            if (frame1 >= 1)
            {
                flag = false;
                Destroy(gameObject);
            }

            frame1++;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Health health = collider.GetComponent<Health>();

        if (health != null)
        {
            health.GainHealth(gainValue, collider);
            m_AudioSource.clip = pickupSound;
            m_AudioSource.Play();
            flag = true;

            //Destroy(gameObject);
        }
    }

}
