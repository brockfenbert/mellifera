﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // public variables set for each level
    // pollenAvailable can probably be rewritten to count instances of a Pollen object, but that doesn't exist yet
    [SerializeField] int pollenAvailable = 0;
    [SerializeField] int pollenTarget = 0;
    [SerializeField] private int startingHealth = 100;
    public bool canFinishLevel = false;
    public int pollenCollected = 0;
    public int currentHealth;
    public string nextLevel;
    
    // References to other objects
    [SerializeField] Slider pollenSlider;
    [SerializeField] Slider healthSlider;
    public GameObject nextLevelUI;
    public GameObject nextLevelGraphics;
    public GameObject reloadLevelUI;

    public static bool gamePaused = false;

    // DDR BOIS
    public GameObject ddrCanvas;
    public GameObject uiCanvas;


    private PollenTargetSlider pollenTargetSlider;

    GameObject ddrTarget;


    // Start is called before the first frame update
    void Start()
    {
        pollenTargetSlider = FindObjectOfType<PollenTargetSlider>();
        SetupPollenSlider();
        SetupHealthSlider();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPollenLevel();
    }

    public int GetPollenTarget()
    {
        return pollenTarget;
    }

    public int GetPollenAvailable()
    {
        return pollenAvailable;
    }

    void CheckPollenLevel()
    {
        if (pollenCollected >= pollenTarget)
        {
            canFinishLevel = true;
            pollenTargetSlider.SetShouldLerpColor(true);
            nextLevelGraphics.SetActive(true);
        }
        else
        {
            canFinishLevel = false;
            pollenTargetSlider.SetShouldLerpColor(false);
            nextLevelGraphics.SetActive(false);
        }
        pollenSlider.value = pollenCollected;
    }

    void SetupPollenSlider()
    {
        pollenSlider.maxValue = pollenAvailable;
        pollenSlider.minValue = 0;
        pollenSlider.value = 0;
    }
    
    public void CollectPollen(int pollenAmount)
    {
        pollenCollected += pollenAmount;
    }

    void SetupHealthSlider()
    {
        currentHealth = startingHealth;
        healthSlider.maxValue = startingHealth;
        healthSlider.minValue = 0;
        healthSlider.value = currentHealth;
    }

    public void IncrementHealth(int amount)
    {
        currentHealth += amount;
        healthSlider.value = (currentHealth / (1.0f * startingHealth)) * 100 ;
        if (currentHealth <= 0)
        {
            gamePaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            reloadLevelUI.SetActive(true);
        }
    }

    public void ReloadLevel()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        reloadLevelUI.SetActive(false);
        gamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        if (nextLevel != null)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(nextLevel);
        }
    }

    public void ResetNextLevelUI()
    {
        Time.timeScale = 1;
        nextLevelUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartDDR(GameObject target)
    {
        ddrTarget = target;
        gamePaused = true;
        uiCanvas.SetActive(false);
        ddrCanvas.SetActive(true);
        FindObjectOfType<DDRManager>().startDDR();
    }

    public void EndDDR(int score, int maxScore)
    {
        gamePaused = false;
        ddrCanvas.SetActive(false);
        uiCanvas.SetActive(true);
        FindObjectOfType<StingBehavior>().FinishSting(score, maxScore, ddrTarget);

    }
}
