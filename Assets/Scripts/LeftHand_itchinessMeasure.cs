using System.Linq;
using UnityEngine;

public class LeftHand_itchinessMeasure : MonoBehaviour
{
    // Apply this script on left hand

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
            // prepare to count how many triggers, (or could say count the number of different type of trigger)
            // though would be invalid if 2 triggers are the same type
            // but the main purpose is to change the sensitivity value between different types of trigger,
            // thus should not be a big problem
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

            // compare max value of three numbers
            leftHandCurrentSen = Mathf.Max(lowSen, Mathf.Max(midSen, highSen));

            // type of itchy point(trigger) may change, thus has different itchiness
            PlayerGlobalVar.currentSen = Mathf.Max(right.rightHandCurrentSen, leftHandCurrentSen);

            // if player is not intersecting with any itchy point
            if (PlayerGlobalVar.numOfItchPoint == 0)
            {
                PlayerGlobalVar.StartAddItchinenss();
            }

            // get total triggers by right and left hands
            leftCountSen = CountTrue(triggerWithLow, triggerWithMid, triggerWithHigh);
            PlayerGlobalVar.numOfItchPoint = leftCountSen + right.rightCountSen;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlayerGlobalVar.itchiness.ContainsKey(other.tag))
        {
            // prepare to count the number of different type of trigger
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

            // compare max value of three numbers
            leftHandCurrentSen = Mathf.Max(lowSen, Mathf.Max(midSen, highSen));
            PlayerGlobalVar.currentSen = Mathf.Max(right.rightHandCurrentSen, leftHandCurrentSen);

            // count the number of different type of trigger
            leftCountSen = CountTrue(triggerWithLow, triggerWithMid, triggerWithHigh);
            PlayerGlobalVar.numOfItchPoint = leftCountSen + right.rightCountSen;

            // if player is only intersecting with one itchy point
            if (PlayerGlobalVar.numOfItchPoint == 0)
            {
                PlayerGlobalVar.StopAddItchinenss();
                PlayerGlobalVar.totalItchiness = 0;
            }

        }
    }

    // count numbers of "true"
    public static int CountTrue(params bool[] args)
    {
        return args.Count(t => t);
    }
}
