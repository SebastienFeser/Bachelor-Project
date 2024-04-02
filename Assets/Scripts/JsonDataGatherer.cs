using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonDataGatherer : MonoBehaviour
{
    [SerializeField] GameObject JsonDataGathererGameObject;
    public PulseStartMeasures dGpulseStartMeasures;
    public ArduinoDataReader arduinoDataReader;
    public enum GameState
    {
        BEFORE_START,
        WAIT,
        INTRO,
        TUTO,
        GAME,
        OUTRO
    }

    public GameState gameState;

    

    
    struct PulseData
    {
        public int pulse;
        public int time;
        public int index;
    }

    struct EventData
    {
        public int time;
        public string dataType;

    }

    string playerName = "emptyName";

    string playerFolderPath; 

    List<PulseData> introPulseData = new List<PulseData>();
    List<PulseData> tutoPulseData = new List<PulseData>();
    List<PulseData> gamePulseData = new List<PulseData>();
    List<PulseData> outroPulseData = new List<PulseData>();

    List<EventData> eventData = new List<EventData>();

    float currentTime;
    int timeInt;
    int mediumPulse;

    private void Awake()
    {
        gameState = GameState.BEFORE_START;
        currentTime = 0;
        timeInt = 0;
        DontDestroyOnLoad(JsonDataGathererGameObject);
        CreateStartFolder();
    }
    private void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        switch (gameState)
        {
            case GameState.BEFORE_START:
                break;
            case GameState.WAIT:
                break;
            case GameState.INTRO:
                if (currentTime >= timeInt)
                {
                    PulseData currentPulseData = new PulseData();
                    currentPulseData.pulse = dGpulseStartMeasures.GetActualPulse();
                    currentPulseData.time = (int)currentTime;
                    currentPulseData.index = 0;
                    introPulseData.Add(currentPulseData);
                    timeInt++;
                }
                break;
            case GameState.TUTO:
                if (currentTime >= timeInt)
                {
                    PulseData currentPulseData = new PulseData();
                    currentPulseData.pulse = (int)arduinoDataReader.GetPulse();
                    currentPulseData.time = (int)currentTime;
                    currentPulseData.index = 0;
                    tutoPulseData.Add(currentPulseData);
                    timeInt++;
                }
                break;
            case GameState.GAME:
                if (currentTime >= timeInt)
                {
                    PulseData currentPulseData = new PulseData();
                    currentPulseData.pulse = (int)arduinoDataReader.GetPulse();
                    currentPulseData.time = (int)currentTime;
                    currentPulseData.index = 0;
                    gamePulseData.Add(currentPulseData);
                    timeInt++;
                }
                break;
            case GameState.OUTRO:
                break;
        }
    }

    public void RestartTimer()
    {
        currentTime = 0;
        timeInt = 0;
    }

    public void CreateStartFolder()
    {
        if (!Directory.Exists(Application.dataPath + "/PlayersData"))
        {
            Directory.CreateDirectory(Application.dataPath + "/PlayersData");
        }

        playerFolderPath = Application.dataPath + "/PlayersData/" + playerName;

        Directory.CreateDirectory(playerFolderPath);

    }

    public void IntroDataToJSON()
    {
        string jsonData = "Medium Pulse: " + mediumPulse + "\n";
        foreach (PulseData pulseData in introPulseData)
        {
            
            jsonData += "Time " + pulseData.time;
            jsonData += "(" + pulseData.index + "): ";
            jsonData += pulseData.pulse + "\n";
        }
        File.WriteAllText(playerFolderPath + "/IntroData.json", jsonData);

    }

    public void TutoDataToJSON()
    {
        string jsonData = "Medium Pulse: " + mediumPulse + "\n";
        foreach (PulseData pulseData in tutoPulseData)
        {

            jsonData += "Time " + pulseData.time;
            jsonData += "(" + pulseData.index + "): ";
            jsonData += pulseData.pulse + "\n";
        }
        File.WriteAllText(playerFolderPath + "/TutoData.json", jsonData);
    }

    public void GameDataToJSON()
    {
        string jsonData = "Medium Pulse: " + mediumPulse + "\n";
        foreach (PulseData pulseData in gamePulseData)
        {

            jsonData += "Time " + pulseData.time;
            jsonData += "(" + pulseData.index + "): ";
            jsonData += pulseData.pulse + "\n";
        }
        File.WriteAllText(playerFolderPath + "/GameData.json", jsonData);
    }

    public void EventDataToJSON()
    {
        string jsonData = "";
        foreach (EventData eventData in eventData)
        {

            jsonData += "Time " + eventData.time + ": " + eventData.dataType + "\n";
        }
        File.WriteAllText(playerFolderPath + "/EventData.json", jsonData);
    }

    public void AddEventData(string name)
    {
        EventData currentEventData = new EventData();
        currentEventData.time = (int)currentTime;
        currentEventData.dataType = name;
        eventData.Add(currentEventData);
    }

    public void OutroDataToJSON()
    {

    }

    public void SetMediumPulse(int currentMediumPulse)
    {
        mediumPulse = currentMediumPulse;
    }
}
