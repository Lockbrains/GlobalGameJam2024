
using UnityEngine;

public class RightHand_itchinessMeasure : MonoBehaviour
{
    // Apply this script on both left and right hand

    // Drag parent's script to this var
    public PlayerGlobalVar PlayerGlobalVar; //totalItchiness, numOfItchPoint, currentSen, lowSen, midSen, highSen
    public float rightHandCurrentSen = 0;
    public LeftHand_itchinessMeasure left;


    private void OnTriggerEnter(Collider other)
    {
        if (PlayerGlobalVar.itchiness.ContainsKey(other.tag))
        {
            rightHandCurrentSen = PlayerGlobalVar.itchiness[other.tag];

            // In case the next itchy point has a different itchiness
            //PlayerGlobalVar.currentSen = Mathf.Max(PlayerGlobalVar.currentSen, PlayerGlobalVar.itchiness[other.tag]);
            PlayerGlobalVar.currentSen = Mathf.Max(left.leftHandCurrentSen, rightHandCurrentSen);

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
            rightHandCurrentSen = 0f;
            PlayerGlobalVar.currentSen = Mathf.Max(left.leftHandCurrentSen, rightHandCurrentSen);

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
