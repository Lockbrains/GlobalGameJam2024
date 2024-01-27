
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGlobalVar : MonoBehaviour
{
    // Apply this script on the parent of itchinessMeasure

    public Coroutine controller;
    public int numOfItchPoint = 0;
    public float currentSen = 0;
    public float totalItchiness = 0;
    public Dictionary<string, float> itchiness;



    // Dummy variables: simply having an interfaces for designer to manipulate the value
    public float maxItchiness = 100;
    public float lowSen = 5, midSen = 10, highSen = 15;



    private void Start()
    {
        itchiness = new Dictionary<string, float>()
        {
            {"lowSen", lowSen},
            {"midSen", midSen},
            {"highSen", highSen}
        };
    }

    public void StartAddItchinenss()
    {
        //StartCoroutine() must store in a variable, otherwise won't stop with StopCoroutine()
        controller = StartCoroutine(AddItchiness());
    }

    public void StopAddItchinenss()
    {
        StopCoroutine(controller);
    }


    public IEnumerator AddItchiness()
    {
        while (true)
        {
            totalItchiness += currentSen;
            yield return new WaitForSeconds(1f);
        }

    }

}

