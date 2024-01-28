using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public enum GameState
    {
        InGame, Pause, Title, Victory, Failure
    }

    [Header("Game Status")] 
    public GameState state;

    [Header("Pause State")] 
    public GaussianBlurEffect pauseBlur;
    
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = GameState.Title;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.Title:
                UpdateInTitle();
                break;
            case GameState.InGame:
                UpdateInGame();
                break;
            case GameState.Pause:
                UpdateInPause();
                break;
            default:
                break;
        }

        pauseBlur.enabled = state == GameState.Pause; 
    }

    private void UpdateInTitle()
    {
        if (Input.anyKeyDown)
        {
            //state = GameState.InGame;
            GUIManager.instance.RefreshCamera();
            CameraManager.instance.StartGame();
        }
    }

    private void UpdateInGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            state = GameState.Pause;
            GUIManager.instance.RefreshCamera();
        }
    }

    private void UpdateInPause()
    {
        Time.timeScale = 0;
    }

    public void BackToGame()
    {
        state = GameState.InGame;
        Time.timeScale = 1.0f;
        GUIManager.instance.RefreshCamera();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
