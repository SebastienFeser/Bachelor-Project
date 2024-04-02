using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.SceneManagement;
public class PulseStartMeasures : MonoBehaviour
{
    Thread thread;
    SerialPort data_stream = new SerialPort("COM3", 9600);
    string receivedData;
    List<float> allPulseData = new List<float>();
    int mediumPulse = 0;

    int actualPulse = -1;

    [SerializeField] AudioSource pulseSource;
    [SerializeField] AudioSource validationSource;

    [SerializeField] GameObject pulseInformationObject;
    [SerializeField] PulseInformations pulseInformations;

    [SerializeField] float timerToMeasurePulse = 150f;
    [SerializeField] float timeAtStart = 0f;
    [SerializeField] float timeBeforeStartingMeasuringPulse;

    JsonDataGatherer jsonDataGatherer;

    bool calculatedPulse = false;
    bool hasStartedThread = false;
    // Start is called before the first frame update
    void Start()
    {
        data_stream.Open();
        timeAtStart = 0f;
        calculatedPulse = false;
        thread = new Thread(ThreadMethod);
        thread.Start();
        jsonDataGatherer = FindObjectOfType<JsonDataGatherer>();
        jsonDataGatherer.dGpulseStartMeasures = this;
        jsonDataGatherer.gameState = JsonDataGatherer.GameState.INTRO;
        jsonDataGatherer.RestartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timeAtStart += Time.deltaTime;
        if (timeAtStart > timerToMeasurePulse && !calculatedPulse)
        {
            thread.Abort();
            CalculateMediumPulse();
        }
    }

    void ThreadMethod()
    {
        
        receivedData = data_stream.ReadLine();
        float receivedPulse = float.Parse(receivedData);
        Debug.Log(receivedPulse);
        if (timeAtStart > timeBeforeStartingMeasuringPulse)
        {

            actualPulse = (int)receivedPulse;
            allPulseData.Add(receivedPulse);
        }
        //pulseSource.Play();
        ThreadMethod();
    }

    void CalculateMediumPulse()
    {
        float total = 0;
        foreach (float data in allPulseData)
        {
            total += data;
        }
        mediumPulse = (int)(total / allPulseData.Count);
        calculatedPulse = true;
        Debug.Log("Medium Pulse :" + mediumPulse);
        pulseInformations.mediumPulse = mediumPulse;
        jsonDataGatherer.SetMediumPulse(mediumPulse);
        validationSource.Play();
        LoadNextScene();
    }

    void LoadNextScene()
    {
        jsonDataGatherer.gameState = JsonDataGatherer.GameState.WAIT;
        jsonDataGatherer.IntroDataToJSON();
        DontDestroyOnLoad(pulseInformationObject);
        SceneManager.LoadScene("Tutorial");
    }

    public int GetActualPulse()
    {
        return actualPulse;
    }

    
    void OnApplicationQuit()
    {
        thread.Abort();
    }
}
