using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODAudio : MonoBehaviour
{
    public static FMODAudio Instance { get; private set; }

    private void Awake()
    { 
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayAudio(StudioEventEmitter audioEvent)
    {
        audioEvent.Play();
    }

    public void PlayAudio(StudioEventEmitter audioEvent, Transform location)
    {
        StudioEventEmitter audioEventAtLocation = Instantiate(audioEvent, location);
        audioEventAtLocation.Play();
    }
}
