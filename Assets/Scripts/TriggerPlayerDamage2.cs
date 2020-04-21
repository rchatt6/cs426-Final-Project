using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TriggerPlayerDamage2 : NetworkBehaviour
{
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip hurtSound;
    [SerializeField]
    private int damageValue;

    // Start is called before the first frame update
    void Start()
    {
        //if (!isServer)
            //return;

        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        //GameObject hit = collider.gameObject;
        Health health = collider.GetComponent<Health>();

        if (health != null)
        {
            m_AudioSource.clip = hurtSound;
            m_AudioSource.Play();
            health.TakeDamage(damageValue, collider);
        }
    }
}
