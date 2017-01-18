using UnityEngine;
using System.Collections;

public class RoofVisibility : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Animation animation;

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, player.transform.position - Camera.main.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == this.gameObject)
                HideRoof();
            else
                ShowRoof();
        }
    }

    private void HideRoof()
    {
        Debug.Log("HideRoof");
        animation.Play();
    }

    private void ShowRoof()
    {
        animation.Rewind();
    }
}
