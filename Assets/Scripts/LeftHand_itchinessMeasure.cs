using System.Linq;
using UnityEngine;

public class LeftHand_itchinessMeasure : MonoBehaviour
{
    // Apply this script on left hand

    // Drag parent's script to this var
    public PlayerGlobalVar PlayerGlobalVar;
    public float leftHandCurrentSen = 0;
    public RightHand_itchinessMeasure right;

    private bool triggerWithLow = false;
    private bool triggerWithMid = false;
    private bool triggerWithHigh = false;
    private float lowSen = 0f;
    private float midSen = 0f;
    private float highSen = 0f;
    public int leftCountSen = 0;

    private void OnTriggerStay(Collider other)
    {
        if (PlayerGlobalVar.itchiness.ContainsKey(other.tag))
        {

            if (other.CompareTag("lowSen"))
            {
                triggerWithLow = true;
                lowSen = PlayerGlobalVar.itchiness["lowSen"];
            }
            else if (other.CompareTag("midSen"))
            {
                triggerWithMid = true;
                midSen = PlayerGlobalVar.itchiness["midSen"];
            }
            else if (other.CompareTag("highSen"))
            {
                triggerWithHigh = true;
                highSen = PlayerGlobalVar.itchiness["highSen"];
            }

            leftHandCurrentSen = Mathf.Max(lowSen, Mathf.Max(midSen, highSen));

            // In case the next itchy point has a different itchiness
            PlayerGlobalVar.currentSen = Mathf.Max(right.rightHandCurrentSen, leftHandCurrentSen);


            // if player is not intersecting with any itchy point
            if (PlayerGlobalVar.numOfItchPoint == 0)
            {
                PlayerGlobalVar.StartAddItchinenss();
            }

            leftCountSen = CountTrue(triggerWithLow, triggerWithMid, triggerWithHigh);
            PlayerGlobalVar.numOfItchPoint = leftCountSen + right.rightCountSen;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlayerGlobalVar.itchiness.ContainsKey(other.tag))
        {
            if (other.CompareTag("lowSen"))
            {
                triggerWithLow = false;
                lowSen = 0f;
            }
            else if (other.CompareTag("midSen"))
            {
                triggerWithMid = false;
                midSen = 0f;
            }
            else if (other.CompareTag("highSen"))
            {
                triggerWithHigh = false;
                highSen = 0f;
            }

            leftHandCurrentSen = Mathf.Max(lowSen, Mathf.Max(midSen, highSen));
            PlayerGlobalVar.currentSen = Mathf.Max(right.rightHandCurrentSen, leftHandCurrentSen);

            // if player is only intersecting with one itchy point
            if (PlayerGlobalVar.numOfItchPoint == 1)
            {
                PlayerGlobalVar.StopAddItchinenss();
                PlayerGlobalVar.totalItchiness = 0;
            }

            leftCountSen = CountTrue(triggerWithLow, triggerWithMid, triggerWithHigh);
            PlayerGlobalVar.numOfItchPoint = leftCountSen + right.rightCountSen;
        }
    }

    public static int CountTrue(params bool[] args)
    {
        return args.Count(t => t);
    }
}