using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerDamage : MonoBehaviour
{
    //public float CurrentHealth { get; set; }
    //public float MaxHealth { get; set; }
    //public float CurrentArmor { get; set; }
    //public float MaxArmor { get; set; }
    //public bool hasArmor = true;

    //public float health = 100;
    //public float armor = 90;
    //public float damage = 20;

    private AudioSource m_AudioSource;
    [SerializeField] private AudioClip hurtSound;
    private GameObject respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.MaxHealth = 100;
        PlayerHealth.CurrentHealth = PlayerHealth.MaxHealth;

        PlayerHealth.MaxArmor = 90;
        PlayerHealth.CurrentArmor = PlayerHealth.MaxArmor;

        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_AudioSource.clip = hurtSound;
            m_AudioSource.Play();
            
            if (PlayerHealth.CurrentHealth <= 0)
            {
                //Debug.Log(other.transform.GetChild(0).gameObject.name);
                //Destroy(other.gameObject);
                respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
                other.transform.position = respawnPoint.transform.position;
                //Destroy(other.transform.GetChild(1).gameObject);
            }
        }
    }

    void DealArmorDamage(float damageValue)
    {
        PlayerHealth.CurrentArmor -= damageValue;
    }

    void DealDamage(float damageValue)
    {
        PlayerHealth.CurrentHealth -= damageValue;
    }
}
