using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")] 
    [SerializeField] private AudioSource as_BGM;
    
    [Header("Prefabs")] 
    [SerializeField] private GameObject snd_laugh_weak;
    [SerializeField] private GameObject snd_laugh_mild;
    [SerializeField] private GameObject snd_laugh_wild;
    
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MakeSound(GameObject sfx)
    {
        GameObject s = Instantiate(sfx);
        Destroy(s, 5f);
    }

    private void MakeSoundAt(GameManager sfx, Transform t)
    {
        GameManager s = Instantiate(sfx, t);
        Destroy(s, 5f);
    }
    
    /// <summary>
    /// Make a laugh sound
    /// </summary>
    /// <param name="i">
    /// i = 0: weak smile;
    /// i = 1: mild laughter;
    /// i = 2: hahahahahahaha
    /// </param>
    public void MakeLaughSnd(int extend)
    {
        switch (extend)
        {
            case 0:
                MakeSound(snd_laugh_weak);
                break;
            case 1:
                MakeSound(snd_laugh_mild);
                break;
            default:
                MakeSound(snd_laugh_wild);
                break;
        }
    }
}
