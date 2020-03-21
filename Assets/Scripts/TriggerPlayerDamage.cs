using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerDamage : MonoBehaviour
{
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentArmor { get; set; }
    public float MaxArmor { get; set; }
    public bool hasArmor = true;

    public float health = 100;
    public float armor = 90;
    public float damage = 20;

    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = 100;
        CurrentHealth = MaxHealth;

        MaxArmor = 90;
        CurrentArmor = MaxArmor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (hasArmor && CurrentHealth == 100)
            {
                DealArmorDamage(30);
            }

            if (!hasArmor || CurrentArmor <= 0)
            {
                DealDamage(20);
            }
            
            if (CurrentHealth <= 0)
            {
                Destroy(other.gameObject);
            }
        }
    }

    void DealArmorDamage(float damageValue)
    {
        CurrentArmor -= damageValue;
    }

    void DealDamage(float damageValue)
    {
        CurrentHealth -= damageValue;
    }
}
