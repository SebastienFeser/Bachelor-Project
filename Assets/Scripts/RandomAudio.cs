using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudio : MonoBehaviour
{
    [SerializeField] float swedishRhapsodyTimer;
    [SerializeField] float revertedVoiceTimer;
    [SerializeField] float ghostCryTimer;
    // Start is called before the first frame update
    [SerializeField] AudioSource swedishRhapsodySource;
    [SerializeField] AudioSource revertedVoiceSource;
    [SerializeField] AudioSource ghostCrySource;
    [SerializeField] GameManager gameManager;

    float currentTime;
    enum SoundStatus
    {
        START,
        SWEDISH_RHAPSODY,
        REVERTED_VOICE,
        GHOST_CRY
    }

    SoundStatus soundStatus;
    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        switch (soundStatus)
        {
            case SoundStatus.START:
                if (currentTime > swedishRhapsodyTimer)
                {
                    swedishRhapsodySource.Play();
                    gameManager.WriteNewEvent("Playing Swedish Rhapsody");
                    soundStatus = SoundStatus.SWEDISH_RHAPSODY;
                }
                break;
            case SoundStatus.SWEDISH_RHAPSODY:
                if (currentTime > revertedVoiceTimer)
                {
                    revertedVoiceSource.Play();
                    gameManager.WriteNewEvent("Playing Reverted Voice");
                    soundStatus = SoundStatus.REVERTED_VOICE;                        ;
                }
                break;
            case SoundStatus.REVERTED_VOICE:
                if (currentTime > ghostCryTimer)
                {
                    ghostCrySource.Play();
                    gameManager.WriteNewEvent("Playing Ghost Cry");
                    soundStatus = SoundStatus.GHOST_CRY;
                }
                break;
            case SoundStatus.GHOST_CRY:
                break;
        }
    }
}
