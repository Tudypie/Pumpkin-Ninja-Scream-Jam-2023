using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animator blackImageAnimator;
    [SerializeField] GameObject loadingText;

    private void Start()
    {
        FMODAudio.Instance.menuSoundtrack.Play();
    }

    public void OnButtonHover()
    {
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.hoverOver);
    }

    public void OnHoverButton()
    {
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.hoverOver);
    }

    public void OnPlayButtonClick()
    {
        StartCoroutine(StartGame());
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.playButton);
    }
    public void OnQuitButtonClick()
    {
        blackImageAnimator.Play("ImageFadeIn");
        FMODAudio.Instance.menuSoundtrack.Stop();
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.buttons);
        Invoke(nameof(QuitGame), 3f);
    }

    public void OnCreditsButtonClick()
    {
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.buttons);
    }

    private IEnumerator StartGame()
    {
        FMODAudio.Instance.menuSoundtrack.Stop();
        blackImageAnimator.Play("ImageFadeIn");
        yield return new WaitForSeconds(2.5f);
        loadingText.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        LoadGameScene();
    }

    private void LoadGameScene() => SceneLoader.Instance.LoadScene(1);
    private void QuitGame() => Application.Quit();
}
