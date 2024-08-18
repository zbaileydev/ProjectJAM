using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("SFX")]
    public AudioSource hoverSound;
    public AudioSource sliderSound;
    public AudioSource clickSound;

    [Header("Menu Canvas")]
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject mainMenu;

    private GameObject[] panels;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Toggling all panels besides the one passed in.
    public void PanelSwitch(GameObject activePanel)
    {
        foreach (var panel in panels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
            
        }
        activePanel.SetActive(true);
    }

    // Determine if we are returning to the main menu 
    // or the pause menu.
    public void BackButton()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            PanelSwitch(mainMenu);
        }
        else
        {
            PanelSwitch(pauseMenu);
        }
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

    // SFX for interacting with UI elements.
    /*
    public void PlayHover(){
		hoverSound.Play();
	}

    public void PlaySFXHover(){
        sliderSound.Play();
    }

    public void PlayClick(){
        clickSound.Play();
    }
    */

}
