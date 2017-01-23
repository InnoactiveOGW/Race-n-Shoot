using UnityEngine;
using System.Collections;

public class RoofVisibility : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AudioReverbZone reverbZone;

    [SerializeField]
    private AudioSource openSound;
    [SerializeField]
    private AudioSource closeSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HideRoof();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ShowRoof();
        }
    }

    private void HideRoof()
    {
        Debug.Log("HideRoof");
        reverbZone.enabled = true;
        animator.SetTrigger("Open");
        openSound.Play();
    }

    private void ShowRoof()
    {
        Debug.Log("ShowRoof");
        reverbZone.enabled = false;
        animator.SetTrigger("Close");
        closeSound.Play();
    }
}
