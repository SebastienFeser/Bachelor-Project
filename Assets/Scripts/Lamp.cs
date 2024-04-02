using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] GameObject litLamp;
    [SerializeField] GameObject normalLamp;
    [SerializeField] float lightTimer = 0.5f;
    [SerializeField] float lightCooldown = 5f;
    [SerializeField] AudioSource flashOnSource;
    [SerializeField] AudioSource flashOffSource;
    bool activateLamp = false;
    bool lampActive = false;
    bool coolDown = false;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activateLamp && !lampActive)
        {
            time = 0;
            lampActive = true;
            activateLamp = false;
            litLamp.SetActive(true);
            normalLamp.SetActive(false);
            flashOnSource.Play();
        }
        else if (lampActive)
        {
            time += Time.deltaTime;
            if (time >= lightTimer)
            {
                lampActive = false;
                coolDown = true;
                time = 0;
                litLamp.SetActive(false);
                normalLamp.SetActive(true);
                flashOffSource.Play();
            }
        }
        else if (coolDown)
        {
            time += Time.deltaTime;
            if (time >= lightCooldown)
            {
                time = 0;
                coolDown = false;
            }
        }
    }
    public void ActivateLamp()
    {
        if(!coolDown && !lampActive)
        {
            activateLamp = true;
        }

    }
}
