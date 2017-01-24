using UnityEngine;
using System.Collections;
using OvrTouch.Hands;

public abstract class ThrowableItem : MonoBehaviour
{
    [SerializeField]
    private Grabbable grabbable;

    [SerializeField]
    private AudioSource hitSound;

    void OnCollisionEnter(Collision collision)
    {
        if (grabbable.IsGrabbed)
        {
            Debug.Log("Is grabbed");
            return;
        }

        Debug.Log("Not grabbed");
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Shootable"))
        {
            hitSound.Play();
            Execute(collision.contacts[0].point);
        }
    }

    public abstract void Execute(Vector3 position);
}
