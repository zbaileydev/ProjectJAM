using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;

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
    public Dropdown resolutionDropdown;

    private string gameOverLevel;
    private string mainMenuLevel;
    private Resolution[] availableResolutions;

    void Start()
    {
        gameOverLevel = "GameOver";
        mainMenuLevel = "MainMenu";
        // Get available resolutions before we adjust anything else.
        resolutionDropdown = resolutionDropdown.GetComponent<Dropdown>();
        SelectResolution();


        // Ensure all panels are off except the main menu.
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);

        // Many of the options will need to move to a function
        // since we will re-use this for the pause menu
        LoadSettings();
    }


    void SelectResolution()
    {
        resolutionDropdown.ClearOptions();

        availableResolutions = Screen.resolutions;

        availableResolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + "x" + availableResolutions[i].height;
            options.Add(option);

            if (availableResolutions[i].width == Screen.width && availableResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }


    public void ClickPlay()
    {
        // Load the next scene in the build order
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex++;
        SceneManager.LoadScene(nextSceneIndex);

        // Or we can directly load it
        // SceneManager.LoadScene("Demo");

        // Or call a coroutine if it will take a while to load
        // AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
    }

    public void ClickCredits()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void ClickMenu()
    {
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
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
        PlayerPrefs.SetFloat("MainVolume", volumeSlider.value);
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenCheckbox.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        volumeSlider.value = PlayerPrefs.GetFloat("MainVolume", 0.75f);
        fullscreenCheckbox.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", resolutionDropdown.value);    
    }

    public void ApplySettings()
    {
        SetVolume("MainVolume", volumeSlider.value);
        SetVolume("SoundVolume", soundSlider.value);
        SetVolume("MusicVolume", musicSlider.value);

        string selectResolution = resolutionDropdown.options[resolutionDropdown.value].text;
        bool isFullscreen = fullscreenCheckbox.isOn;

        SetResolution(selectResolution, isFullscreen);
        SaveSettings();
    }

    
}