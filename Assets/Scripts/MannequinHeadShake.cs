using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinHeadShake : MonoBehaviour
{
    [SerializeField] Transform neckTransform;
    [SerializeField] Transform armLeftAngle;
    [SerializeField] Transform armRightAngle;
    [SerializeField] bool isJumpscare = false;
    bool isActive = false;
    bool wasActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isJumpscare)
        {
            if (isActive)
            {
                if (!wasActive)
                {
                    armLeftAngle.eulerAngles = new Vector3(220, armLeftAngle.eulerAngles.y, armLeftAngle.eulerAngles.z);
                    armRightAngle.eulerAngles = new Vector3(180, armRightAngle.eulerAngles.y, armRightAngle.eulerAngles.z);
                    wasActive = true;
                }
                neckTransform.eulerAngles = new Vector3(neckTransform.eulerAngles.x, neckTransform.eulerAngles.y, Random.Range(-45, 45));
            }
            else if (wasActive)
            {
                neckTransform.eulerAngles = new Vector3(neckTransform.eulerAngles.x, neckTransform.eulerAngles.y, 0);
                armLeftAngle.eulerAngles = new Vector3(0, armLeftAngle.eulerAngles.y, armLeftAngle.eulerAngles.z);
                armRightAngle.eulerAngles = new Vector3(0, armRightAngle.eulerAngles.y, armRightAngle.eulerAngles.z);
                wasActive = false;
            }
        }
        else
        {
            if (!wasActive)
            {
                armLeftAngle.eulerAngles = new Vector3(220, armLeftAngle.eulerAngles.y, armLeftAngle.eulerAngles.z);
                armRightAngle.eulerAngles = new Vector3(180, armRightAngle.eulerAngles.y, armRightAngle.eulerAngles.z);
                wasActive = true;
            }
            neckTransform.eulerAngles = new Vector3(neckTransform.eulerAngles.x, neckTransform.eulerAngles.y, Random.Range(-20, 20));
        }
        
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }
}
