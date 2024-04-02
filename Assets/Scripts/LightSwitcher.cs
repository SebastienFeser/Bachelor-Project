using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitcher : MonoBehaviour
{
    
    [SerializeField] List<GameObject> LightOnGameObjects = new List<GameObject>();
    [SerializeField] List<GameObject> LightOffGameObjects = new List<GameObject>();
    [SerializeField] AudioSource lightCutSource;
    [SerializeField] GameManager gameManager;

    bool hasSwitchedLightOff = false;
    [SerializeField] float timeUnitilBlackout = 60f;
    float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bool hasSwitchedLightOff = false;
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timeUnitilBlackout && !hasSwitchedLightOff)
        {
            SwitchLightOff();
            hasSwitchedLightOff = true;
        }
    }

    void SwitchLightOff()
    {
        lightCutSource.Play();
        gameManager.WriteNewEvent("Lights off");
        foreach (GameObject _object in LightOnGameObjects)
        {
            _object.SetActive(false);
        }
        foreach (GameObject _object in LightOffGameObjects)
        {
            _object.SetActive(true);
        }
    }

    void SwitchLightOn()
    {
        foreach (GameObject _object in LightOnGameObjects)
        {
            _object.SetActive(true);
        }
        foreach (GameObject _object in LightOffGameObjects)
        {
            _object.SetActive(false);
        }
    }
}
