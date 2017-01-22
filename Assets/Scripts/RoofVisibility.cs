using UnityEngine;
using System.Collections;

public class RoofVisibility : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AudioSource openSound;
    [SerializeField]
    private AudioSource closeSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HideRoof();
            openSound.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ShowRoof();
            closeSound.Play();
        }
    }

    private void HideRoof()
    {
        Debug.Log("HideRoof");
        animator.SetTrigger("Open");
    }

    private void ShowRoof()
    {
        Debug.Log("ShowRoof");
        animator.SetTrigger("Close");
    }
}
