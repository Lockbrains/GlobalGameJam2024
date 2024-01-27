
using UnityEngine;

public class LeftHand_itchinessMeasure_old : MonoBehaviour
{
    // Apply this script on both left and right hand

    // Drag parent's script to this var
    public PlayerGlobalVar PlayerGlobalVar; //totalItchiness, numOfItchPoint, currentSen, lowSen, midSen, highSen
    public float leftHandCurrentSen = 0;
    public RightHand_itchinessMeasure right;


    private void OnTriggerEnter(Collider other)
    {
        if (PlayerGlobalVar.itchiness.ContainsKey(other.tag))
        {
            leftHandCurrentSen = PlayerGlobalVar.itchiness[other.tag];

            // In case the next itchy point has a different itchiness
            //PlayerGlobalVar.currentSen = Mathf.Max(PlayerGlobalVar.currentSen, PlayerGlobalVar.itchiness[other.tag]);
            PlayerGlobalVar.currentSen = Mathf.Max(right.rightHandCurrentSen, leftHandCurrentSen);

            // if player is not intersecting with any itchy point
            if (PlayerGlobalVar.numOfItchPoint == 0)
            {
                PlayerGlobalVar.numOfItchPoint++;
                PlayerGlobalVar.StartAddItchinenss();
            }
            // if player is intersecting with more than 1 itchy point
            else
            {
                PlayerGlobalVar.numOfItchPoint++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlayerGlobalVar.itchiness.ContainsKey(other.tag))
        {
            leftHandCurrentSen = 0f;
            PlayerGlobalVar.currentSen = Mathf.Max(right.rightHandCurrentSen, leftHandCurrentSen);

            // if player is only intersecting with one itchy point
            if (PlayerGlobalVar.numOfItchPoint == 1)
            {
                PlayerGlobalVar.numOfItchPoint--;
                PlayerGlobalVar.StopAddItchinenss();
                PlayerGlobalVar.totalItchiness = 0;

            }
            // if player is intersecting with multiple itchy point
            else
            {
                PlayerGlobalVar.numOfItchPoint--;
            }
        }
    }



}
