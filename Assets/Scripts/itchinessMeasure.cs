using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itchinessMeasure : MonoBehaviour
{
    //dummy variables, simply interfaces for designer
    public float maxItchiness = 100;
    public float lowSen_secondsToMaxItchiness = 8f;
    public float midSen_secondsToMaxItchiness = 6f;
    public float highSen_secondsToMaxItchiness = 4f;

    public float totalItchiness = 0;
    private int numOfItchPoint = 0;
    private float currentSen;
    private Coroutine controller;
    private Dictionary<string, float> itchiness;

    private void Start()
    {
        itchiness = new Dictionary<string, float>()
        {
            {"lowSen", maxItchiness/lowSen_secondsToMaxItchiness},
            {"midSen", maxItchiness/midSen_secondsToMaxItchiness},
            {"highSen", maxItchiness/highSen_secondsToMaxItchiness}
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (itchiness.ContainsKey(other.tag))
        {
            // In case the next itchy point has a different itchiness
            currentSen = itchiness[other.tag];

            // if player is not intersecting with any itchy point
            if (numOfItchPoint == 0)
            {
                numOfItchPoint++;
                controller = StartCoroutine(AddItchiness());
            }
            // if player is intersecting with more than 1 itchy point
            else
            {
                numOfItchPoint++;
            }
        }
    }

    private void OnTriggerExit()
    {
        // if player is only intersecting with one itchy point
        if (numOfItchPoint == 1)
        {
            numOfItchPoint--;
            StopCoroutine(controller);
            totalItchiness = 0;

        }
        //if player is intersecting with multiple itchy point
        else
        {
            numOfItchPoint--;
        }
    }

    IEnumerator AddItchiness()
    {
        while (true)
        {
            totalItchiness += currentSen;
            yield return new WaitForSeconds(1f);
        }

    }


}
