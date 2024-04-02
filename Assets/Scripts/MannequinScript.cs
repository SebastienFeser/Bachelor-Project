using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinScript : MonoBehaviour
{
    [SerializeField] MannequinHeadShake mannequinHeadShake;
    [SerializeField] AudioSource mannequinSource;

    bool mannequinActivated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMannequin()
    {
        mannequinHeadShake.Activate();
        mannequinActivated = true;
        mannequinSource.Play();
    }

    public void DeactivateMannequin()
    {
        mannequinHeadShake.Deactivate();
        mannequinActivated = false;
        mannequinSource.Stop();
    }

    public bool GetMannequinActivated()
    {
        return mannequinActivated;
    }
}
