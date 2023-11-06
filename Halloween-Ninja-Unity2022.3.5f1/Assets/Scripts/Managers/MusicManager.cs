using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public EventReference fmodGameplayMusicEvent;

    private FMOD.Studio.EventInstance gameplayMusicEventInstance;

    public float musicIntensity;

    private FMODAudio fmodaudio;

    public static MusicManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        fmodaudio = FMODAudio.Instance;

        fmodaudio.ambienceSoundtrack.Play();

        fmodGameplayMusicEvent = fmodaudio.gameplaySoundtrack.EventReference;
        gameplayMusicEventInstance = fmodaudio.CreateInstance(fmodGameplayMusicEvent);
        gameplayMusicEventInstance.start();
    }

    private void Update()
    {
        gameplayMusicEventInstance.setParameterByName("Wave Intensity", musicIntensity);
        Debug.Log("Music intensity: " + musicIntensity);
    }

    public void Defeat()
    {
        fmodaudio.defeatSoundtrack.Play();
        fmodaudio.gameplaySoundtrack.Stop();
        fmodaudio.ambienceSoundtrack.Stop();
        fmodaudio.bossBattleSoundtrack.Stop();
    }

    public void EnterBossBattle()
    {
        fmodaudio.gameplaySoundtrack.Stop();
        fmodaudio.ambienceSoundtrack.Stop();
        fmodaudio.bossBattleSoundtrack.Play();
    }

    public void ExitBossBattle()
    {
        fmodaudio.defeatSoundtrack.Stop();
        fmodaudio.ambienceSoundtrack.Stop();
        fmodaudio.gameplaySoundtrack.Stop();
        fmodaudio.bossBattleSoundtrack.Stop();
    }

}
