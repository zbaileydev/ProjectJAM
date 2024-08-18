﻿using System;
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

    [Header("Managers")]
    public LevelLoader levelLoader;
    public UIManager uIManager;

    private string gameOverLevel;
    private string mainMenuLevel;

    void Start()
    {
        gameOverLevel = "GameOver";
        mainMenuLevel = "MainMenu";

        // Ensure all panels are off except the main menu.
        uIManager.PanelSwitch(menuPanel);
    }

    public void ClickPlay()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        levelLoader.LoadScene(nextSceneIndex);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(gameOverLevel);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(mainMenuLevel);
    }

}