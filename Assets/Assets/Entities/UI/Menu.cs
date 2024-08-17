using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using TMPro;

public class Menu : MonoBehaviour
{

    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject creditsPanel;
    public GameObject optionsPanel;

    [Header("SFX")]
    public AudioSource hoverSound;
    public AudioSource sliderSound;
    public AudioSource clickSound;
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider soundSlider;
    public Slider volumeSlider;

    [Header("Screen")]
    public Toggle fullscreenCheckbox;
    public TMP_Dropdown resolutionDropdown;

    private string gameOverLevel;
    private string mainMenuLevel;

    void Start()
    {
        gameOverLevel = "GameOver";
        mainMenuLevel = "MainMenu";

        resolutionDropdown = resolutionDropdown.GetComponent<TMP_Dropdown>();
        fullscreenCheckbox = fullscreenCheckbox.GetComponent<Toggle>();


        // Ensure all panels are off except the main menu.
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);

        // Many of the options will need to move to a function
        // since we will re-use this for the pause menu
        LoadSettings();
    }

    public void ClickCredits()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void ClickMenu()
    {
        LoadSettings(); // Reset settings when going back.
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);
    }

    public void ClickOptions()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        // If in the editor, stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // If in a build, quit the application
        Application.Quit();
        #endif
    }

    public void GameOver()
    {
        SceneManager.LoadScene(gameOverLevel);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(mainMenuLevel);
    }

    // SFX for interacting with UI elements.
    public void PlayHover(){
		hoverSound.Play();
	}

    public void PlaySFXHover(){
        sliderSound.Play();
    }

    public void PlayClick(){
        clickSound.Play();
    }


    public void SetVolume(string property, float volume)
    {
        audioMixer.SetFloat(property, Mathf.Log10(volume) * 20);
    }

    public void SetResolution(string resolution, bool isFullscreen)
    {
        string[] dimensions = resolution.Split('x');
        // Valid resolution format.
        if (dimensions.Length == 2 && int.TryParse(dimensions[0], out int width) && int.TryParse(dimensions[1], out int height))
        {
            Debug.LogError("Resolution changed to: " + dimensions);
            Screen.SetResolution(width, height, isFullscreen);
        }
        else
        {
            Debug.LogError("Invalid resolution format.");
        }

    } 



    /* Methods to save, load, and apply settings for:
    - 3 sliders for MusicVolume, SoundVolume, and MainVolume.
    - A dropdown for choosing resolution.
    - A checkbox to toggle fullscreen.
    */

    public void SaveSettings() 
    {
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("Sound", soundSlider.value);
        PlayerPrefs.SetFloat("Master", volumeSlider.value);
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenCheckbox.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("Music", 0.75f);
        soundSlider.value = PlayerPrefs.GetFloat("Sound", 0.75f);
        volumeSlider.value = PlayerPrefs.GetFloat("Master", 0.75f);
        fullscreenCheckbox.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", resolutionDropdown.value);    
    }

    public void ApplySettings()
    {
        SetVolume("Master", volumeSlider.value);
        SetVolume("Sound", soundSlider.value);
        SetVolume("Music", musicSlider.value);

        string selectResolution = resolutionDropdown.options[resolutionDropdown.value].text;
        bool isFullscreen = fullscreenCheckbox.isOn;

        SetResolution(selectResolution, isFullscreen);
        SaveSettings();
    }
    
}