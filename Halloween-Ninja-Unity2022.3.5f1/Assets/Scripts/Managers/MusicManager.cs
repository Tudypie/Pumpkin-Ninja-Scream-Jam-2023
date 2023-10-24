using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
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
        fmodaudio.gameplaySoundtrack.Play();
        fmodaudio.ambienceSoundtrack.Play();
    }

    public void Defeat()
    {
        fmodaudio.defeatSoundtrack.Play();
        fmodaudio.gameplaySoundtrack.Stop();
        fmodaudio.ambienceSoundtrack.Stop();
    }

    public void EnterBossBattle()
    {
        fmodaudio.defeatSoundtrack.Play();
        fmodaudio.gameplaySoundtrack.Stop();
        fmodaudio.bossBattleSoundtrack.Play();

    }

}
