using UnityEngine;
using System.Collections;

public class PlayerGunRotation : MonoBehaviour
{
	void Update ()
	{
		float x = Input.GetAxis ("RightStickX");
		float y = Input.GetAxis ("RightStickY");

		if (x == 0 && y == 0)
			return;
		
		Vector3 dir = Vector3.zero - new Vector3 (x, 0f, y);
		Quaternion newRotation = Quaternion.LookRotation (dir);
		transform.rotation = newRotation;
	}
}
