using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public AudioMixer audioMixer;
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip confirmSFX;
    [SerializeField]
    private AudioClip backSFX;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuUI.SetActive(false);
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        m_AudioSource.clip = backSFX;
        m_AudioSource.Play();
    }

    void Pause()
    {
        m_AudioSource.clip = confirmSFX;
        m_AudioSource.Play();
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
    }

    public void Quit()
    {
        m_AudioSource.clip = backSFX;
        m_AudioSource.Play();
        Application.Quit();
    }
}
