using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private OVRInput.Controller controller;
    OVRInput.Hand hand;
    [SerializeField] float playerSpeed = 1f;
    [SerializeField] float rotationAngle = 45f;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform playerRightHand;
    [SerializeField] Lamp lampScript;
    [SerializeField] Footsteps footstepsScript;

    [SerializeField] float minimumPlayerSpeed = 2.5f;
    [SerializeField] float maximumPlayerSpeed = 4.5f;

    //[SerializeField] Transform cameraTransform;
    private bool stickLeft = false;
    private bool stickRight = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        OVRInput.FixedUpdate();
        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger, controller) > 0.1f)
        {
            float playerTransformY = playerTransform.position.y;
            playerTransform.position += playerRightHand.transform.rotation * Vector3.forward * playerSpeed * Time.deltaTime;
            playerTransform.position = new Vector3(playerTransform.position.x, playerTransformY, playerTransform.position.z);
            footstepsScript.PlayFootsteps();
        }

        if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger, controller) > 0.1f)
        {
            float playerTransformY = playerTransform.position.y;
            playerTransform.position += playerRightHand.transform.rotation * Vector3.back * playerSpeed * Time.deltaTime;
            playerTransform.position = new Vector3(playerTransform.position.x, playerTransformY, playerTransform.position.z);
            footstepsScript.PlayFootsteps();
        }
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x > 0.3f & !stickRight)
        {
            stickRight = true;
            playerTransform.eulerAngles += new Vector3(0, rotationAngle, 0);
        }
        if (OVRInput.Get(OVRInput.RawButton.B, controller) || OVRInput.Get(OVRInput.RawButton.A, controller) || OVRInput.Get(OVRInput.RawButton.RThumbstick, controller))
        {
            ActivateLamp();
        }
        else if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x < -0.3f & !stickLeft)
        {
            stickLeft = true;
            playerTransform.eulerAngles -= new Vector3(0, rotationAngle, 0);
        }
        else if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x > -0.3f && OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x < 0.3f)
        {
            stickLeft = false;
            stickRight = false;
        }
    }

    void ActivateLamp()
    {
        lampScript.ActivateLamp();
    }

    public void SpeedLerp(float lerpPercentage)
    {
        playerSpeed = Mathf.Lerp(minimumPlayerSpeed, maximumPlayerSpeed, lerpPercentage);
        footstepsScript.FootstepsAudioLerp(lerpPercentage);
    }
}
