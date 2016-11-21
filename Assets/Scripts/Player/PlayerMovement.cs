using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float airRes = 0f;
	public float groundRes = 10f;

	public float speed = 30.0f;
	Vector3 movDir = Vector3.zero;
	float xSpeed = 0f;
	float zSpeed = 0f;

	float curDir = 0f;
	Vector3 curNormal = Vector3.up;

	public float gravity = 30f;
	float vertSpeed = 0f;

	CharacterController cController;

	void Awake ()
	{
		cController = GetComponent<CharacterController> ();
	}

	void Update ()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		Vector3 inputDir = new Vector3 (h, 0f, v);

		if (inputDir != Vector3.zero) {
			curDir = Vector3.Angle (new Vector3 (0f, 0f, 1f), inputDir);
			if ((h < 0 && v >= 0) || (v < 0 && h < 0))
				curDir *= -1;
		}

		RaycastHit hit;
		if (Physics.Raycast (transform.position, -curNormal, out hit)) {
			curNormal = Vector3.Lerp (curNormal, hit.normal, 4 * Time.deltaTime);
			Quaternion grndTilt = Quaternion.FromToRotation (Vector3.up, curNormal);
			transform.rotation = grndTilt * Quaternion.Euler (0, curDir, 0);
		}
			
		if (inputDir == Vector3.zero && (movDir.x != 0 || movDir.z != 0)) {
			float deceleration;
			if (cController.isGrounded) {
				deceleration = groundRes * Time.deltaTime;
			} else {
				deceleration = airRes * Time.deltaTime;
			}

			if (xSpeed > 0 && xSpeed - deceleration > 0) {
				xSpeed -= deceleration;
			} else if (xSpeed < 0 && xSpeed + deceleration < 0) {
				xSpeed += deceleration;
			} else {
				xSpeed = 0;
			}

			if (zSpeed > 0 && zSpeed - deceleration > 0) {
				zSpeed -= deceleration;
			} else if (zSpeed < 0 && zSpeed + deceleration < 0) {
				zSpeed += deceleration;
			} else {
				zSpeed = 0;
			}

			movDir = new Vector3 (xSpeed, 0f, zSpeed);
			Debug.Log (movDir);
			
		} else {
			movDir = inputDir.normalized.sqrMagnitude * transform.forward * speed;
			xSpeed = movDir.x;
			zSpeed = movDir.z;
		}


		if (cController.isGrounded)
			vertSpeed = 0;
		vertSpeed -= gravity * Time.deltaTime; // apply gravity
		
		movDir.y = vertSpeed; // keep the current vert speed
		cController.Move (movDir * Time.deltaTime);
	}

}
