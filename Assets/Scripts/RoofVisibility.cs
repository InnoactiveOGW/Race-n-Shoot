using UnityEngine;
using System.Collections;

public class RoofVisibility : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

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
        animator.SetTrigger("Open");
    }

    private void ShowRoof()
    {
        Debug.Log("ShowRoof");
        animator.SetTrigger("Close");
    }
}
