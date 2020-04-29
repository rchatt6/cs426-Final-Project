using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class menuStart : MonoBehaviour
{
    GameObject menuTitle;
    GameObject story;
    GameObject controls;
    GameObject credits;

    GameObject playButton;
    GameObject controlsButton;
    GameObject optionsButton;
    GameObject creditsButton;
    GameObject backButton;

    GameObject graphicOptions;
    GameObject graphicOptionsDrop;
    GameObject volumeOptions;
    GameObject volumeSlider;

    public AudioMixer audioMixer;
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip confirmSFX;
    [SerializeField]
    private AudioClip backSFX;

    void Awake()
    {
        menuTitle = GameObject.Find("MenuTitle");
        story = GameObject.Find("StoryInstructions");
        controls = GameObject.Find("Controls");
        credits = GameObject.Find("Credits");

        playButton = GameObject.Find("PlayButton");
        controlsButton = GameObject.Find("ControlsButton");
        optionsButton = GameObject.Find("OptionsButton");
        creditsButton = GameObject.Find("CreditsButton");
        backButton = GameObject.Find("BackButton");

        graphicOptions = GameObject.Find("Option1");
        graphicOptionsDrop = GameObject.Find("GraphicOptionsDrop");
        volumeOptions = GameObject.Find("Option2");
        volumeSlider = GameObject.Find("VolumeSlider");
        backButton.SetActive(false);
        graphicOptions.SetActive(false);
        graphicOptionsDrop.SetActive(false);
        volumeOptions.SetActive(false);
        volumeSlider.SetActive(false);

        m_AudioSource = GetComponent<AudioSource>();
    }

    

    public void changemenuscene(string scenename)
    {
        m_AudioSource.clip = confirmSFX;
        m_AudioSource.Play();
        //Application.LoadLevel(scenename);
        SceneManager.LoadScene(scenename);
    }

    public void menuControls()
    {
        m_AudioSource.clip = confirmSFX;
        m_AudioSource.Play();

        menuTitle.GetComponent<UnityEngine.UI.Text>().text = "Controls";
        story.GetComponent<UnityEngine.UI.Text>().text = "";
        controls.GetComponent<UnityEngine.UI.Text>().text = "Movement - W, A, S, D\n" +
                                                            "Jump - SPACEBAR\n" +
                                                            "Run - SHIFT\n" +
                                                            "Rotate Camera - Mouse\n" +
                                                            "Fire weapon - Left-Mouse\n" +
                                                            "Aim weapon - Hold Right-Mouse\n" +
                                                            "Grab Bridge Piece - Hold Middle-Mouse\n" +
                                                            "Aim/Throw Bridge Piece - Right-Mouse\n" +
                                                            "Reload - R\n" +
                                                            "Pause Menu - ESC";
        credits.GetComponent<UnityEngine.UI.Text>().text = "";

        playButton.SetActive(false);
        controlsButton.SetActive(false);
        optionsButton.SetActive(false);
        creditsButton.SetActive(false);
        backButton.SetActive(true);
    }

    public void menuOptions()
    {
        m_AudioSource.clip = confirmSFX;
        m_AudioSource.Play();

        menuTitle.GetComponent<UnityEngine.UI.Text>().text = "Options";
        story.GetComponent<UnityEngine.UI.Text>().text = "";
        controls.GetComponent<UnityEngine.UI.Text>().text = "";
        credits.GetComponent<UnityEngine.UI.Text>().text = "";
        graphicOptions.SetActive(true);
        graphicOptionsDrop.SetActive(true);
        volumeOptions.SetActive(true);
        volumeSlider.SetActive(true);

        playButton.SetActive(false);
        controlsButton.SetActive(false);
        optionsButton.SetActive(false);
        creditsButton.SetActive(false);
        backButton.SetActive(true);
    }

    public void menuCredits()
    {
        m_AudioSource.clip = confirmSFX;
        m_AudioSource.Play();

        menuTitle.GetComponent<UnityEngine.UI.Text>().text = "Credits";
        story.GetComponent<UnityEngine.UI.Text>().text = "";
        controls.GetComponent<UnityEngine.UI.Text>().text = "";
        credits.GetComponent<UnityEngine.UI.Text>().text = "- UIC CS426 Videogame Design Team -\n" +
                                                            "Rahul Chatterjee\n" +
                                                            "Allen Breyer\n" +
                                                            "Aakash Kotak\n\n" +
                                                            "- Art & Sounds -\n" +
                                                            "Mixamo (Zombie Model & Animations)\n" +
                                                            "SoundIdeasCom (Footstep SFX)\n" +
                                                            "AbloomAudio (Zombie SFX)\n" + 
															"Lots of youtube tutorials!";

        playButton.SetActive(false);
        controlsButton.SetActive(false);
        optionsButton.SetActive(false);
        creditsButton.SetActive(false);
        backButton.SetActive(true);
    }

    public void menuBack()
    {
        m_AudioSource.clip = backSFX;
        m_AudioSource.Play();

        menuTitle.GetComponent<UnityEngine.UI.Text>().text = "Main Menu";
        story.GetComponent<UnityEngine.UI.Text>().text = "Story: In the near future, a virus by the name of COVID-19 has spread " +
                                                            "across the globe and caused the infected to turn into zombie-like creatures. " +
                                                            "This pandemic has led to a war for resources leaving those without any " +
                                                            "supplies vulnerable to the virus. The clock is counting down between two " +
                                                            "superpowers to fight over the remaining resources which will ultimately lead " +
                                                            "to the winner surviving the virus!\n\n" +
                                                            "INSTRUCTIONS: Gather the missing bridge pieces scattered across the Island " +
                                                            "while avoiding the zombies, and build your bridge to get across the water to " +
                                                            "the Island with Resources!";
        controls.GetComponent<UnityEngine.UI.Text>().text = "";
        credits.GetComponent<UnityEngine.UI.Text>().text = "";

        playButton.SetActive(true);
        controlsButton.SetActive(true);
        optionsButton.SetActive(true);
        creditsButton.SetActive(true);
        backButton.SetActive(false);
        graphicOptions.SetActive(false);
        graphicOptionsDrop.SetActive(false);
        volumeOptions.SetActive(false);
        volumeSlider.SetActive(false);
    }

    public void setQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }


    public void setVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void menuExit()
    {
        m_AudioSource.clip = backSFX;
        m_AudioSource.Play();
        Application.Quit();
    }
}
