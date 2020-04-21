using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class NPCHealthRaycast : NetworkBehaviour
{
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip zombieHit;
    [SerializeField]
    private AudioClip zombieDead;
    int frame1;
    bool isDying;
    Animator anim;
    [SerializeField]
    private GameObject bloodParticles;
    [SerializeField]
    private ParticleSystem ps;

    public const int maxHealth = 100;
	[SyncVar (hook = "OnHealthChanged")] public int currentHealth = maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        if (!isServer)
        {
            return;
        }
        //bloodParticles.SetActive(false);
        m_AudioSource = GetComponent<AudioSource>();
        anim = this.GetComponent<Animator>();
        frame1 = 0;
        isDying = false;
        InvokeRepeating("Update1", 0.2f, 0.2f);
    }

    void Update1()
    {
        if (!isServer)
        {
            return;
        }

        

        if (isDying)
        {
            //Debug.Log("frame1: " + frame1);
            if (frame1 >= 30)
            {
                Destroy(gameObject);
            }
            
            frame1++;
        }
    }
	
	public void TakeDamage(int amount){
		
		currentHealth -= amount;
        //bloodParticles.GetComponentInChildren<ParticleSystem>().Stop();
        
        //bloodParticles.SetActive(true);
        //bloodParticles.GetComponent<ParticleSystem>().Play();
        
        

        //Debug.Log("emitting: " + ps.isEmitting);
        //Debug.Log("playing: " + ps.isPlaying);
        //Debug.Log("paused: " + ps.isPaused);
        //Debug.Log("stopped: " + ps.isStopped);

        

        if (currentHealth <= 0 && !isDying)
        {
            if (!ps.isPlaying)
            {
                ps.Play();
            }
            m_AudioSource.loop = false;
            m_AudioSource.clip = zombieDead;
            m_AudioSource.Play();
            anim.SetBool("isMoving", false);
            anim.SetBool("isDying", true);
            isDying = true;
            //die();
		}
        else if (!isDying)
        {
            if (!ps.isPlaying)
            {
                ps.Play();
            }
            m_AudioSource.loop = false;
            m_AudioSource.clip = zombieHit;
            m_AudioSource.Play();
        }
	}
	
	public void OnHealthChanged(int hlthOld, int hlthNew){
		hlthOld = hlthNew;
	}

    /*public void die()
    {
        Destroy(gameObject);
    }*/
}
