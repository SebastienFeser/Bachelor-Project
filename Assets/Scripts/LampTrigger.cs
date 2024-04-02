using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampTrigger : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Tutorial tutorialScript;
    [SerializeField] bool isTutorial = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mannequin")
        {
            
            if (other.GetComponent<MannequinScript>().GetMannequinActivated())
            {
                if (isTutorial)
                {
                    tutorialScript.HitMannequin();
                }
                else
                {
                    gameManager.HitMannequin();
                }
            }
            
        }
    }
}
