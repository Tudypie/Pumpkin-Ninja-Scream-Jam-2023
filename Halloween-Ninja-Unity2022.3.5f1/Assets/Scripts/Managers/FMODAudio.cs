using UnityEngine;
using FMODUnity;

public class FMODAudio : MonoBehaviour
{
    public static FMODAudio Instance { get; private set; }

    private void Awake()
    { 
        if(Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
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
