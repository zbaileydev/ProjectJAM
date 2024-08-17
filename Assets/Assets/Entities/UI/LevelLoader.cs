using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public GameObject background;

    public void StartLevelAnimation()
    {

    }

    public void LoadScene(int scene){
        Debug.Log("Next scene index: " + scene);
        background.SetActive(true);
        StartCoroutine(LoadAsynchronously(scene));
	}

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneIndex);
        loading.allowSceneActivation = false;

        // Play fade animation
        while (!loading.isDone)
        {
            if (loading.progress >= 0.9f)
            {
                background.SetActive(false);
                loading.allowSceneActivation = true;
            }
            yield return null;
        }
        
    }


    public void ClickPlay()
    {
        // Get the next scene index and then load it async.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        LoadScene(nextSceneIndex);
    }

    // TODO: Remove this in the build.
    private void Update() {
        if (Input.GetKeyDown (KeyCode.P))
        {
            Debug.Log("Changing scene");
            ClickPlay();
        }
    }

}
