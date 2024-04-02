using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinJumpscare : MonoBehaviour
{
    [SerializeField] GameObject jumpscareObject;
    [SerializeField] MannequinScript mannequinScript;
    [SerializeField] float mannequinApparitionSeconds = 1.0f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Transform playerHeadTransform;
    float timeSinceIntantiated = 0;

    private void Start()
    {
        mannequinScript.ActivateMannequin();
        timeSinceIntantiated = 0;
        //mannequinApparitionSeconds = audioSource.clip.length;
    }

    private void Update()
    {
        jumpscareObject.transform.rotation = playerHeadTransform.rotation;
        jumpscareObject.transform.eulerAngles += new Vector3(0, 90, 0);
        jumpscareObject.transform.position = new Vector3(playerHeadTransform.position.x, playerHeadTransform.position.y, playerHeadTransform.position.z);
    }

    private void FixedUpdate()
    {
        timeSinceIntantiated += Time.deltaTime;
        if (timeSinceIntantiated >= mannequinApparitionSeconds)
        {
            Destroy(jumpscareObject);
        }
    }

    public void SetPlayerHeadTransform(Transform thisPlayerHeadTransform)
    {
        playerHeadTransform = thisPlayerHeadTransform;
    }
}
