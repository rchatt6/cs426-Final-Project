using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentArmor { get; set; }
    public float MaxArmor { get; set; }
    public bool hasArmor = true;

    public Slider healthbar;
    public Slider armorbar;

    public Text healthText;
    public Text armorText;

    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = 100f;
        CurrentHealth = MaxHealth;
        MaxArmor = 90f;
        CurrentArmor = MaxArmor;

        healthText.text = CurrentHealth + "/" + MaxHealth;
        armorText.text = CurrentArmor + "/" + MaxArmor;

        healthbar.value = CalculateHealth();
        armorbar.value = CalculateArmor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DeathPlane")
        {
            if (hasArmor && CurrentHealth == 100)
            {
                DealArmorDamage(30);
            }

            if (!hasArmor || CurrentArmor <= 0)
            {
                DealDamage(20);
            }
        }
    }

    void DealArmorDamage(float damageValue)
    {
        CurrentArmor -= damageValue;
        armorbar.value = CalculateArmor();
        armorText.text = CurrentArmor + "/" + MaxArmor;
    }

    void DealDamage(float damageValue)
    {
        CurrentHealth -= damageValue;
        healthbar.value = CalculateHealth();
        healthText.text = CurrentHealth + "/" + MaxHealth;

        if (CurrentHealth <= 0)
            Die();
    }

    float CalculateHealth()
    {
        return CurrentHealth / MaxHealth;
    }

    float CalculateArmor()
    {
        return CurrentArmor / MaxArmor;
    }

    void Die()
    {
        CurrentHealth = 0;
        Debug.Log("You died");
    }
}
