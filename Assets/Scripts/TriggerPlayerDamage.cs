using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TriggerPlayerDamage : NetworkBehaviour
{

    private AudioSource m_AudioSource;
    [SerializeField] private AudioClip hurtSound;
    private GameObject respawnPoint;
    //Player pl = new Player();

    // Start is called before the first frame update
    void Start()
    {
        if (!isServer)
            return;


        /*Player.maxHealth = 100;
        
        PlayerHealth.CurrentHealth = PlayerHealth.MaxHealth;

        PlayerHealth.MaxArmor = 90;
        PlayerHealth.CurrentArmor = PlayerHealth.MaxArmor;*/

        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerPlayerDamage");

        if (!isServer)
            return;

        

        if (other.tag == "Player")
        {
            //Debug.Log("hurtSound = " + hurtSound);

            m_AudioSource.clip = hurtSound;
            m_AudioSource.Play();

            //Debug.Log("Player.currentHealth: " + pl.currentHealth);

            /*if (pl.currentHealth <= 0)
            {
                //Debug.Log(other.transform.GetChild(0).gameObject.name);
                //Destroy(other.gameObject);
                respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
                other.transform.position = respawnPoint.transform.position;
                //Destroy(other.transform.GetChild(1).gameObject);
            }*/
        }
    }

    /*void DealArmorDamage(float damageValue)
    {
        PlayerHealth.CurrentArmor -= damageValue;
    }

    void DealDamage(float damageValue)
    {
        PlayerHealth.CurrentHealth -= damageValue;
    }*/
}
