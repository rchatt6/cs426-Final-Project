using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static bool hasArmor = true;

    [SerializeField]
    private Stats health;

    [SerializeField]
    private Stats armor;

    [SerializeField]
    private Stats stamina;

    public static float maxStamina = 500f;
    public static float currentStamina;
    public static float maxHealth = 100f;
    public static float currentHealth;
    public static float maxArmor = 90f;
    public static float currentArmor;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public static Player instance;

    private void Awake()
    {
        health.Initialize();
        armor.Initialize();
        instance = this;
    }

    void Start()
    {
        currentStamina = maxStamina;
        stamina.MaxVal = maxStamina;
        stamina.CurrentVal = maxStamina;
        currentHealth = maxHealth;
        health.MaxVal = maxHealth;
        health.CurrentVal = maxHealth;
        currentArmor = maxArmor;
        armor.MaxVal = maxArmor;
        armor.CurrentVal = maxArmor;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.tag == "Zombie")
        {
            if (hasArmor && health.CurrentVal == 100)
            {
                armor.CurrentVal -= 15;
            }
            /*
            if(armor.CurrentVal <= 0)
            {
                hasArmor = false;
            }

            if (!hasArmor && armor.CurrentVal <= 0)
            {
                health.CurrentVal -= 20;
            }
        }*/
        if(other.tag == "BarbWire")
        {
            health.CurrentVal -= 5;
        }
        // deal max damage
        else if (other.tag == "DeathPlane")
        {
            health.CurrentVal -= health.MaxVal;
        }
        // respawns the player with 100 health again
        else if (other.tag == "Respawn")
        {
            if (health.CurrentVal <= 0)
            {
                health.CurrentVal = health.MaxVal;
            }
        }
    }

    public void UseStamina(float amount)
    {
        if (currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            stamina.CurrentVal = currentStamina;

            if (regen != null)
                StopCoroutine(regen);

            regen = StartCoroutine(RegenStamina());
        }
        else
        {
            Debug.Log("Not enough stamina");
        }
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2);

        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            stamina.CurrentVal = currentStamina;
            yield return regenTick;
        }
        regen = null;
    }

}
