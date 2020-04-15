using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameObject uiObject;
    private AudioSource m_AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        uiObject.SetActive(false);
        m_AudioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter (Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            uiObject.SetActive(true);
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
            StartCoroutine("WaitForSec");
        }
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(10);
        Destroy(uiObject);
        m_AudioSource.Stop();
        Destroy(gameObject);
    }
}
