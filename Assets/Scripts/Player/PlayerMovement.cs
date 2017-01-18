using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float airRes;
    [SerializeField]
    private float groundRes;
    [SerializeField]
    private float normalSpeed;

    [HideInInspector]
    public float speed;

    [HideInInspector]
    public float speedBoostTimer;
    [SerializeField]
    private GameObject boostFire;

    private Vector3 movDir = Vector3.zero;
    private float xSpeed = 0f;
    private float zSpeed = 0f;

    private float curDir = 0f;
    private Vector3 curNormal = Vector3.up;

    public float gravity = 30f;
    private float vertSpeed = 0f;

    private CharacterController cController;

    void Awake()
    {
        speed = normalSpeed;
        cController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (speedBoostTimer > 0f)
        {
            boostFire.SetActive(true);
            speedBoostTimer = Mathf.Max(0f, speedBoostTimer - Time.deltaTime);
            if (speedBoostTimer == 0f)
            {
                boostFire.SetActive(false);
                speed = normalSpeed;
            }
        }

        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        float h = input.x;
        float v = input.y;

        Vector3 inputDir = new Vector3(h, 0f, v);

        if (inputDir != Vector3.zero)
        {
            curDir = Vector3.Angle(new Vector3(0f, 0f, 1f), inputDir);
            if ((h < 0 && v >= 0) || (v < 0 && h < 0))
                curDir *= -1;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -curNormal, out hit))
        {
            Vector3 cameraDirection = Camera.main.transform.forward;
            Vector3 cameraFlatDirection = new Vector3(cameraDirection.x, 0f, cameraDirection.z);
            float viewAngle = Vector3.Angle(new Vector3(0f, 0f, 1f), cameraFlatDirection);

            curNormal = Vector3.Lerp(curNormal, hit.normal, 4 * Time.deltaTime);
            Quaternion grndTilt = Quaternion.FromToRotation(Vector3.up, curNormal);
            transform.rotation = grndTilt * Quaternion.Euler(0, curDir + viewAngle, 0);
        }

        if (inputDir == Vector3.zero && (movDir.x != 0 || movDir.z != 0))
        {
            float deceleration;
            if (cController.isGrounded)
            {
                deceleration = groundRes * Time.deltaTime;
            }
            else
            {
                deceleration = airRes * Time.deltaTime;
            }

            if (xSpeed > 0 && xSpeed - deceleration > 0)
            {
                xSpeed -= deceleration;
            }
            else if (xSpeed < 0 && xSpeed + deceleration < 0)
            {
                xSpeed += deceleration;
            }
            else
            {
                xSpeed = 0;
            }

            if (zSpeed > 0 && zSpeed - deceleration > 0)
            {
                zSpeed -= deceleration;
            }
            else if (zSpeed < 0 && zSpeed + deceleration < 0)
            {
                zSpeed += deceleration;
            }
            else
            {
                zSpeed = 0;
            }

            movDir = new Vector3(xSpeed, 0f, zSpeed);

        }
        else
        {
            movDir = inputDir.normalized.sqrMagnitude * transform.forward * speed;
            xSpeed = movDir.x;
            zSpeed = movDir.z;
        }


        if (cController.isGrounded)
            vertSpeed = 0;
        vertSpeed -= gravity * Time.deltaTime; // apply gravity

        movDir.y = vertSpeed; // keep the current vert speed
        cController.Move(movDir * Time.deltaTime);
    }

}
