using UnityEngine;
using System.Collections;

public abstract class ThrowableItem : MonoBehaviour
{
    [SerializeField]
    private AudioSource hitSound;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Shootable"))
        {
            hitSound.Play();
            Execute(collision.contacts[0].point);
        }
    }

    public abstract void Execute(Vector3 position);
}
