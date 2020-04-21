using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player : NetworkBehaviour
{
    public static bool hasArmor = true;

    [SerializeField]
    private Stats health;

    [SerializeField]
    private Stats armor;

    [SerializeField]
    private Stats stamina;

    public const float maxStamina = 500f;
    [SyncVar]
    public float currentStamina = maxStamina;

    public const float maxHealth = 100f;
    [SyncVar]
    public float currentHealth = maxHealth;

    public const float maxArmor = 90f;
    [SyncVar]
    public float currentArmor = maxArmor;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public static Player instance;

    private void Awake()
    {
        if (!isLocalPlayer)
            return;

        health.Initialize();
        armor.Initialize();
        instance = this;
    }

    void Start()
    {
        if (!isLocalPlayer)
            return;

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

    void Update()
    {
        if (!isLocalPlayer)
            return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer)
            return;

        if (other.tag == "Zombie")
        {
            if (armor.CurrentVal >= 1 && armor.CurrentVal <= 90)
            {
                armor.CurrentVal -= 30;
            }
            if (armor.CurrentVal <= 0)
            {
                armor.CurrentVal = 0;
                health.CurrentVal -= 20;
            }
        }
        else if (other.tag == "BarbWire")
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
                armor.CurrentVal = armor.MaxVal;
                health.CurrentVal = health.MaxVal;
            }
        }
        Debug.Log("health.CurrentVal2: " + health.CurrentVal);
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
            currentStamina += maxStamina / 50;
            stamina.CurrentVal = currentStamina;
            yield return regenTick;
        }
        regen = null;
    }

}
