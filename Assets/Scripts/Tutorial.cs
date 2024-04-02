using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] List<MannequinScript> mannequinScripts;
    [SerializeField] GameObject pulseInformationObject;
    [SerializeField] ArduinoDataReader arduinoDataReader;
    // Start is called before the first frame update

    JsonDataGatherer jsonDataGatherer;

    enum TutorialState
    {
        EXIT_FIRST_ROOM_TRIGGER,
        ENTER_MANNEQUIN_ROOM_TRIGGER,
        SECOND_MANNEQUIN_TRIGGER,
        THIRD_MANNEQUIN_TRIGGER

    }

    TutorialState tutorialState;
    void Start()
    {
        pulseInformationObject = FindObjectOfType<PulseInformations>().gameObject;
        tutorialState = TutorialState.ENTER_MANNEQUIN_ROOM_TRIGGER;
        mannequinScripts[0].ActivateMannequin();
        jsonDataGatherer = FindObjectOfType<JsonDataGatherer>();
        jsonDataGatherer.arduinoDataReader = arduinoDataReader;

        jsonDataGatherer.gameState = JsonDataGatherer.GameState.TUTO;
        jsonDataGatherer.RestartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitMannequin()
    {
        if (tutorialState == TutorialState.ENTER_MANNEQUIN_ROOM_TRIGGER)
        {
            mannequinScripts[0].DeactivateMannequin();
            mannequinScripts[2].ActivateMannequin();
            tutorialState = TutorialState.SECOND_MANNEQUIN_TRIGGER;
        }
        else if (tutorialState == TutorialState.SECOND_MANNEQUIN_TRIGGER)
        {
            mannequinScripts[0].DeactivateMannequin();
            DontDestroyOnLoad(pulseInformationObject);
            jsonDataGatherer.gameState = JsonDataGatherer.GameState.WAIT;
            jsonDataGatherer.TutoDataToJSON();
            SceneManager.LoadScene("SampleScene");
        }

    }
}
