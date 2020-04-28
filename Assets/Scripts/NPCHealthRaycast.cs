using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class NPCHealthRaycast : NetworkBehaviour
{
    //[SyncVar]
	private AudioSource m_AudioSource;
	//[SyncVar]
    [SerializeField]
    private AudioClip zombieHit;
	//[SyncVar]
    [SerializeField]
    private AudioClip zombieDead;
    int frame1;
    bool isDying;
	//[SyncVar]
    Animator anim;
	//[SyncVar]
    [SerializeField]
    private GameObject bloodParticles;
	//[SyncVar]
    [SerializeField]
    private ParticleSystem ps;

	//[SyncVar]
    public const int maxHealth = 100;
	[SyncVar (hook = "OnHealthChanged")] public int currentHealth = maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        if (hasAuthority)
        {
            CmdSendNPCHealth(currentHealth);
			//CmdSendNPCPos(transform.position);
        }
        //bloodParticles.SetActive(false);
        m_AudioSource = GetComponent<AudioSource>();
        anim = this.GetComponent<Animator>();
        frame1 = 0;
        isDying = false;
        InvokeRepeating("Update1", 0.2f, 0.2f);
    }
	
	[Command]
	void CmdSendNPCHealth(int currhlth){
		RpcUpdateNPCHealth(currhlth);
	}
	
	[ClientRpc]
	void RpcUpdateNPCHealth(int currhlth){
		currentHealth = currhlth;
	}
	
	[Command]
	void CmdSendNPCPos(Vector3 pos){
		RpcUpdateNPCPos(pos);
	}
	
	[ClientRpc]
	void RpcUpdateNPCPos(Vector3 pos){
		transform.position = pos;
	}

    void Update1()
    {
        if (hasAuthority)
        {
            CmdSendNPCHealth(currentHealth);
			CmdSendNPCPos(transform.position);
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
	
	private void FixedUpdate()
    {
        if(!hasAuthority)
        {
            return;
        }
		CmdSendNPCPos(transform.position);
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
