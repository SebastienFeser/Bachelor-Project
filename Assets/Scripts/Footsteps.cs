using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] AudioClip footsteps1;
    [SerializeField] AudioClip footsteps2;
    [SerializeField] AudioClip footsteps3;
    [SerializeField] AudioClip footsteps4;
    [SerializeField] AudioSource source;
    [SerializeField] float minimumAudioPitch = 1f;
    [SerializeField] float maximumAudioPitch = 1.5f;

    public void Update()
    {
    }

    public void PlayFootsteps()
    {
        if (!source.isPlaying)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    source.clip = footsteps1;
                    break;
                case 1:
                    source.clip = footsteps2;
                    break;
                case 2:
                    source.clip = footsteps3;
                    break;
                case 3:
                    source.clip = footsteps4;
                    break;
                default:
                    source.clip = footsteps1;
                    break;
            }

            source.Play();
        }
    }

    public void FootstepsAudioLerp(float lerpPercentage)
    {
        source.pitch = Mathf.Lerp(minimumAudioPitch, maximumAudioPitch, lerpPercentage);
    }
}



