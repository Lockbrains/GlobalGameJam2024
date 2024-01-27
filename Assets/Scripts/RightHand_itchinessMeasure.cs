using System.Linq;
using UnityEngine;

public class RightHand_itchinessMeasure : MonoBehaviour
{
    // Apply this script on right hand

    // Drag parent's script to this var
    public PlayerGlobalVar PlayerGlobalVar;
    public float rightHandCurrentSen = 0;
    public LeftHand_itchinessMeasure left;

    private bool triggerWithLow = false;
    private bool triggerWithMid = false;
    private bool triggerWithHigh = false;
    private float lowSen = 0f;
    private float midSen = 0f;
    private float highSen = 0f;
    public int rightCountSen = 0;

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

            rightHandCurrentSen = Mathf.Max(lowSen, Mathf.Max(midSen, highSen));

            // In case the next itchy point has a different itchiness
            PlayerGlobalVar.currentSen = Mathf.Max(left.leftHandCurrentSen, rightHandCurrentSen);


            // if player is not intersecting with any itchy point
            if (PlayerGlobalVar.numOfItchPoint == 0)
            {
                PlayerGlobalVar.StartAddItchinenss();
            }

            rightCountSen = CountTrue(triggerWithLow, triggerWithMid, triggerWithHigh);
            PlayerGlobalVar.numOfItchPoint = rightCountSen + left.leftCountSen;

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

            rightHandCurrentSen = Mathf.Max(lowSen, Mathf.Max(midSen, highSen));
            PlayerGlobalVar.currentSen = Mathf.Max(left.leftHandCurrentSen, rightHandCurrentSen);

            // if player is only intersecting with one itchy point
            if (PlayerGlobalVar.numOfItchPoint == 1)
            {
                PlayerGlobalVar.StopAddItchinenss();
                PlayerGlobalVar.totalItchiness = 0;
            }

            rightCountSen = CountTrue(triggerWithLow, triggerWithMid, triggerWithHigh);
            PlayerGlobalVar.numOfItchPoint = rightCountSen + left.leftCountSen;
        }
    }

    public static int CountTrue(params bool[] args)
    {
        return args.Count(t => t);
    }
}
