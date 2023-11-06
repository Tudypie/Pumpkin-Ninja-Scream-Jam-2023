using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenButtons : MonoBehaviour
{
    public void OnButtonHover()
    {
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.hoverOver);
    }

    public void OnPlayButton()
    {
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.playButton);
    }

    public void OnButtonPress()
    {
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.buttons);
    }

}
