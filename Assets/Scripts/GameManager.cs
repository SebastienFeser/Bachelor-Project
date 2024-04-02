using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] MannequinScript[] mannequinScripts;
    [SerializeField] PlayerController playerController;
    [SerializeField] int[] mannequinsOrder;
    [SerializeField] AudioSource mannequinHitSource;
    [SerializeField] AudioSource ambient2Source;
    [SerializeField] GameObject mannequinJumpscare;
    [SerializeField] Transform playerHeadTransform;
    [SerializeField] float timeUntilJumpscare = 10f;
    [SerializeField] ArduinoDataReader arduinoDataReader;
    float currentMannequinTime = 0f;
    int currentMannequinIndex = 0;
    [SerializeField] float minimumPulse;
    [SerializeField] float maximumPulse;

    float actualLerp = 0;
    //[SerializeField] float lerpTarget = 0;
    float lerpTransitionPerSecond = 0.03f;

    JsonDataGatherer jsonDataGatherer;

    enum ScareEvolutionState
    {
        NORMAL,
        INVERSE,
    }

    ScareEvolutionState scareEvolutionState;

    [SerializeField] float timeToChangeState = 150f;
    float actualTimerToChangeState = 0f;
    //GameState gameState;
        // Start is called before the first frame update
    void Start()
    {
        minimumPulse = FindObjectOfType<PulseInformations>().mediumPulse;
        Destroy(FindObjectOfType<PulseInformations>().gameObject);
        currentMannequinIndex = 0;
        currentMannequinTime = 0f;
        mannequinScripts[mannequinsOrder[currentMannequinIndex]].ActivateMannequin();
        maximumPulse = minimumPulse + 30f;
        actualTimerToChangeState = 0f;
        scareEvolutionState = ScareEvolutionState.NORMAL;
        jsonDataGatherer = FindObjectOfType<JsonDataGatherer>();
        jsonDataGatherer.gameState = JsonDataGatherer.GameState.GAME;
        jsonDataGatherer.arduinoDataReader = arduinoDataReader;
        jsonDataGatherer.RestartTimer();
    }

    // Update is called once per frame
    void Update()
    {
            currentMannequinTime += Time.deltaTime;
            LerpWithStress();
            if (currentMannequinTime >= timeUntilJumpscare)
            {
                JumpScarePlayer();
                SwitchMannequin();
            }
            ScareStateManager();
    }

    void ScareStateManager()
    {
        actualTimerToChangeState += Time.deltaTime;

        if (actualTimerToChangeState > timeToChangeState)
        {
            switch (scareEvolutionState)
            {
                case ScareEvolutionState.NORMAL:
                    actualTimerToChangeState = 0;
                    scareEvolutionState = ScareEvolutionState.INVERSE;
                    jsonDataGatherer.AddEventData("Invert Stress");
                    break;
                case ScareEvolutionState.INVERSE:
                    arduinoDataReader.StopThread();
                    jsonDataGatherer.gameState = JsonDataGatherer.GameState.WAIT;
                    jsonDataGatherer.GameDataToJSON();
                    jsonDataGatherer.EventDataToJSON();
                    SceneManager.LoadScene("EndScene");
                    break;
            }
        }
    }
    public void HitMannequin()
    {
        mannequinHitSource.Play();
        jsonDataGatherer.AddEventData("Mannequin Hit");
        SwitchMannequin();
    }

    public void SwitchMannequin()
    {
        currentMannequinTime = 0f;
        mannequinScripts[mannequinsOrder[currentMannequinIndex]].DeactivateMannequin();
        currentMannequinIndex++;
        if (mannequinsOrder.Length <= currentMannequinIndex)
        {
            currentMannequinIndex = 0;
        }
        mannequinScripts[mannequinsOrder[currentMannequinIndex]].ActivateMannequin();
    }

    public void LerpWithStress()
    {
        float lerpTarget = CalculateLerpTarget();
        if (actualLerp > lerpTarget)
        {
            actualLerp -= lerpTransitionPerSecond * Time.deltaTime;
            if (actualLerp <= lerpTarget)
            {
                actualLerp = lerpTarget;
            }
        }
        else if (actualLerp <= lerpTarget)
        {
            actualLerp += lerpTransitionPerSecond * Time.deltaTime;
            if (actualLerp >= lerpTarget)
            {
                actualLerp = lerpTarget;
            }
        }

        //Debug.Log(actualLerp);
        playerController.SpeedLerp(actualLerp);
        LerpAmbient(actualLerp);
        FogLerp(actualLerp);
        //add functions with actual lerp
    }

    public void JumpScarePlayer()
    {
        GameObject currentMannequinJumpscare = new GameObject();
        currentMannequinJumpscare = mannequinJumpscare;
        currentMannequinJumpscare.GetComponent<MannequinJumpscare>().SetPlayerHeadTransform(playerHeadTransform);

        Instantiate(currentMannequinJumpscare);
        jsonDataGatherer.AddEventData("Jumpscare");
    }

    public void LerpAmbient(float lerpValue)
    {
        ambient2Source.volume = lerpValue;
    }

    public void FogLerp(float lerpValue)
    {
        RenderSettings.fogColor = new Color32((byte)Mathf.Lerp(75, 255, lerpValue), (byte)Mathf.Lerp(75, 0, lerpValue), (byte)Mathf.Lerp(75, 0, lerpValue), 255);
    }


    float CalculateLerpTarget()
    {
        float currentPulseSimplified = arduinoDataReader.GetPulse();
        if (currentPulseSimplified > maximumPulse)
        {
            currentPulseSimplified = maximumPulse;
        }
        else if (currentPulseSimplified < minimumPulse)
        {
            currentPulseSimplified = minimumPulse;
        }
        float currentLerpTarget = (currentPulseSimplified - minimumPulse)/ (maximumPulse - minimumPulse);

        if (scareEvolutionState == ScareEvolutionState.INVERSE)
        {
            currentLerpTarget = 1 - currentLerpTarget;
            currentLerpTarget = Mathf.Abs(currentLerpTarget);
        }

        return currentLerpTarget;
    }

    public void WriteNewEvent(string eventName)
    {
        jsonDataGatherer.AddEventData(eventName);
    }
}
