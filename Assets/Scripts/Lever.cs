using UnityEngine;
using System.Collections;
using OvrTouch.Hands;
using OvrTouch.Controllers;

public class Lever : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private Transform handle;
    [SerializeField]
    private Transform joint;
    [SerializeField]
    private float padding = -20f;

    private Vector3 straightLine;
    private Hand hand;

    private bool isInBottomPosition;

    void Update()
    {
        if (hand == null)
            return;


        if ((hand.Handedness == HandednessId.Left && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) == 0)
        || (hand.Handedness == HandednessId.Right && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) == 0))
            ResetLever();

        Vector3 leverShouldBe = hand.transform.position - joint.position;
        float angleToUp = LeverAngle(leverShouldBe, Vector3.up);

        if (angleToUp > padding)
        {
            if (handle.position.y < joint.position.y)
            {
                isInBottomPosition = true;
                hand = null;
                gameController.StartGame();
            }
            return;
        }

        Vector3 leverIs = handle.position - joint.position;
        float angle = LeverAngle(leverShouldBe, leverIs);
        transform.RotateAround(joint.position, joint.forward, angle);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isInBottomPosition || hand != null)
            return;

        Hand triggeredHand = other.transform.root.GetComponent<Hand>();
        if (triggeredHand == null)
            return;

        if (!(triggeredHand.Handedness == HandednessId.Left && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0)
        && !(triggeredHand.Handedness == HandednessId.Right && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0))
            return;

        hand = triggeredHand;
    }

    private float LeverAngle(Vector3 too, Vector3 from)
    {
        Vector3 cross = Vector3.Cross(too.normalized, from.normalized);
        return Mathf.Asin(cross.z) * Mathf.Rad2Deg;
    }

    public void ResetLever()
    {
        Vector3 leverIs = handle.position - joint.position;
        float angle;

        if (handle.position.y < joint.position.y)
            angle = -LeverAngle(Vector3.up, leverIs) + (180 + padding);
        else
            angle = LeverAngle(Vector3.up, leverIs) + padding;

        transform.RotateAround(joint.position, joint.forward, angle);
        hand = null;
    }
}
