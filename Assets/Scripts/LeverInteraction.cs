using UnityEngine;
using System.Collections;
using OvrTouch.Hands;
using OvrTouch.Controllers;

public class LeverInteraction : MonoBehaviour
{
    [SerializeField]
    private Transform start;
    [SerializeField]
    private Transform end;
    [SerializeField]
    private Transform handle;
    [SerializeField]
    private Transform joint;

    private Hand hand;

    void Update()
    {
        if (hand == null)
            return;

        Vector3 leverIs = handle.position - joint.position;
        Vector3 leverShouldBe;
        float angle;

        if ((hand.Handedness == HandednessId.Left && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) == 0)
        || (hand.Handedness == HandednessId.Right && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) == 0))
        {
            Debug.Log("released lever");
            leverShouldBe = start.position - joint.position;
            angle = LeverAngle(leverShouldBe, leverIs);

            hand = null;
        }
        else
        {
            Debug.Log("moving lever");
            leverShouldBe = hand.transform.position - joint.position;
            angle = LeverAngle(leverShouldBe, leverIs);
        }

        transform.RotateAround(joint.position, joint.forward, angle);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter 1");
        if (hand != null)
            return;
        Debug.Log("OnTriggerEnter 2");
        Hand triggeredHand = other.transform.root.GetComponent<Hand>();
        if (triggeredHand == null)
            return;
        Debug.Log("OnTriggerEnter 3");
        if (!(triggeredHand.Handedness == HandednessId.Left && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0)
        && !(triggeredHand.Handedness == HandednessId.Right && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0))
            return;

        Debug.Log("grabbed lever");
        hand = triggeredHand;
    }

    private float LeverAngle(Vector3 too, Vector3 from)
    {
        Vector3 cross = Vector3.Cross(too.normalized, from.normalized);
        return Mathf.Asin(cross.z) * Mathf.Rad2Deg;
    }
}
