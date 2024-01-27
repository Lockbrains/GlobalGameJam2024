using System.Linq;
using UnityEngine;

public class RightHand_itchinessMeasure : MonoBehaviour
{
    // Apply this script on right hand

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
            rightHandCurrentSen = Mathf.Max(lowSen, Mathf.Max(midSen, highSen));

            // type of itchy point(trigger) may change, thus has different itchiness
            PlayerGlobalVar.currentSen = Mathf.Max(left.leftHandCurrentSen, rightHandCurrentSen);

            // if player is not intersecting with any itchy point
            if (PlayerGlobalVar.numOfItchPoint == 0)
            {
                PlayerGlobalVar.StartAddItchinenss();
            }

            // get total triggers by right and left hands
            rightCountSen = CountTrue(triggerWithLow, triggerWithMid, triggerWithHigh);
            PlayerGlobalVar.numOfItchPoint = rightCountSen + left.leftCountSen;

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
            rightHandCurrentSen = Mathf.Max(lowSen, Mathf.Max(midSen, highSen));
            PlayerGlobalVar.currentSen = Mathf.Max(left.leftHandCurrentSen, rightHandCurrentSen);

            // count the number of different type of trigger
            rightCountSen = CountTrue(triggerWithLow, triggerWithMid, triggerWithHigh);
            PlayerGlobalVar.numOfItchPoint = rightCountSen + left.leftCountSen;

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
