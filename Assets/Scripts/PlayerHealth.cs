using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static float CurrentHealth { get; set; }
    public static float MaxHealth { get; set; }
    public static float CurrentArmor { get; set; }
    public static float MaxArmor { get; set; }
    public static bool hasArmor = true;

    public Slider healthbar;
    public Slider armorbar;

    public Text healthText;
    public Text armorText;

    public Rigidbody rb;

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

        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zombie")
        {
            //rb.MovePosition(transform.position + transform.right * Time.fixedDeltaTime);
            //rb.transform.position = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);

            if (hasArmor && CurrentHealth == 100)
            {
                DealArmorDamage(30);
            }

            if (!hasArmor || CurrentArmor <= 0)
            {
                DealDamage(20);
            }
        }
        else if (other.tag == "DeathPlane")
        {
            DealDamage(MaxHealth);
        }
        else if (other.tag == "Respawn")
        {
            DealDamage(MaxHealth * -1f);
        }
    }

    void DealArmorDamage(float damageValue)
    {
        CurrentArmor -= damageValue;

        if (CurrentArmor < 0)
        {
            CurrentArmor = 0;
        }

        armorbar.value = CalculateArmor();
        armorText.text = CurrentArmor + "/" + MaxArmor;
    }

    void DealDamage(float damageValue)
    {
        CurrentHealth -= damageValue;
        healthbar.value = CalculateHealth();
        healthText.text = CurrentHealth + "/" + MaxHealth;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            healthbar.value = 0;
            healthText.text = CurrentHealth + "/" + MaxHealth;
            Die();
        }
        else if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
            healthbar.value = MaxHealth;
            healthText.text = CurrentHealth + "/" + MaxHealth;
        }
            
    }

    float CalculateHealth()
    {
        return CurrentHealth / MaxHealth;
    }

    float CalculateArmor()
    {
        return CurrentArmor / MaxArmor;
    }

    public void Die()
    {
        //CurrentHealth = 0;
        //Debug.Log("You died");
        //healthbar.value = CalculateHealth();
        //healthText.text = CurrentHealth + "/" + MaxHealth;
    }
}
