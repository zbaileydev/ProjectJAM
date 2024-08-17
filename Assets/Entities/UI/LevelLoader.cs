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

    public void LoadScene(string scene){
        background.SetActive(true);
        if(scene != ""){
            StartCoroutine(LoadAsynchronously(scene));
        }
	}

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
        loading.allowSceneActivation = false;

        // Play fade animation
        while (!loading.isDone)
        {
            if (loading.progress >= 0.9f)
            {
                loading.allowSceneActivation = true;
            }
            yield return null;
        }
        background.SetActive(false);
    }

}
