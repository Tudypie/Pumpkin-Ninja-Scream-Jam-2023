using UnityEngine;
using UnityEngine.UI;

public class LoseSystem : MonoBehaviour
{
    [SerializeField] Image deathScreen;
    [SerializeField] Image winScreen;

    float lastTime, elapsed;
    float timeUntilStop = 4f;

    public bool Lose { get; set; }
    public bool Win { get; set; }

    public static LoseSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    void FixedUpdate()
    {
        if (Lose) LoseFixedUpdate();
        if (Win) WinFixedUpdate();

        
    }

    void LoseFixedUpdate()
    {
        if (lastTime == 0)
        {
            lastTime = Time.realtimeSinceStartup;
        }
        else
        {
            elapsed += Time.realtimeSinceStartup - lastTime;
            lastTime = Time.realtimeSinceStartup;
            Time.timeScale = Mathf.Lerp(1f, 0f, elapsed / timeUntilStop);
            deathScreen.color = new Color(deathScreen.color.r, deathScreen.color.g, deathScreen.color.b, Mathf.Lerp(0, 1f, elapsed / timeUntilStop));
        }

        if (elapsed >= timeUntilStop)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneLoader.Instance.LoadScene("DeathScreen");
        }
    }

    void WinFixedUpdate()
    {
        if (lastTime == 0)
        {
            lastTime = Time.realtimeSinceStartup;
        }
        else
        {
            elapsed += Time.realtimeSinceStartup - lastTime;
            lastTime = Time.realtimeSinceStartup;
            Time.timeScale = Mathf.Lerp(1f, 0f, elapsed / timeUntilStop);
            winScreen.color = new Color(winScreen.color.r, winScreen.color.g, winScreen.color.b, Mathf.Lerp(0, 1f, elapsed / timeUntilStop));
        }

        if (elapsed >= timeUntilStop)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneLoader.Instance.LoadScene("WinScreen");
        }
    }


    public void Defeat()
    {
        Lose = true;
        WaveSystem.Instance.Defeat();
        MusicManager.Instance.Defeat();
    }

    public void Victory()
    {
        MusicManager.Instance.ExitBossBattle();
    }
}
