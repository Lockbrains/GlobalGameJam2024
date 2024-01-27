using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;

    [Header("Screens")] 
    [SerializeField] private GameObject screen_Title;
    [SerializeField] private GameObject screen_MainUI;
    [SerializeField] private GameObject screen_PauseUI;
    
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshCamera();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshCamera()
    {
        screen_Title.SetActive(GameManager.instance.state == GameManager.GameState.Title);
        screen_MainUI.SetActive(GameManager.instance.state == GameManager.GameState.InGame);
        screen_PauseUI.SetActive(GameManager.instance.state == GameManager.GameState.Pause);
    }
}
