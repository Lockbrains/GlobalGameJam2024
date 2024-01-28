using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCheck : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y <= 10.0f)
        {
            SceneManager.LoadScene(0);
        }
    }
}
