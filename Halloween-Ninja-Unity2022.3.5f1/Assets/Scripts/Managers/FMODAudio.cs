using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODAudio : MonoBehaviour
{
    public static FMODAudio Instance { get; private set; }

    [Header("Soundtracks")]
    public StudioEventEmitter menuSoundtrack;
    public StudioEventEmitter gameplaySoundtrack;
    public StudioEventEmitter bossBattleSoundtrack;
    public StudioEventEmitter ambienceSoundtrack;
    public StudioEventEmitter defeatSoundtrack;

    [Header("Misc")]
    public EventReference waveStart;
    public EventReference waveEnd;
    public EventReference gateOpen;

    [Header("UI")]
    public EventReference buttons;
    public EventReference hoverOver;
    public EventReference playButton;

    [Header("Character")]
    public EventReference characterFootsteps;
    public EventReference characterJump;
    public EventReference katanaAttack;
    public EventReference katanaSlash;
    public EventReference katanaDash;
    public EventReference shurikenThrow;

    [Header("Defense Point")]
    public EventReference defensePointAlert;
    public EventReference defensePointDestruction;

    [Header("Enemies")]
    public EventReference pumpkinExplosion;

    [Header("Snapshots")]
    public StudioEventEmitter betweenWavesSnapshot;

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

    public void PlayAudio(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound, transform.position);
    }

    public void PlayAudio(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
}
