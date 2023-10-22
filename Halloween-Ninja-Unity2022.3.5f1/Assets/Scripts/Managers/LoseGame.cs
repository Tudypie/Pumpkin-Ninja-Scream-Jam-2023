using UnityEngine;
using UnityEngine.UI;

public class LoseGame : MonoBehaviour
{
    [SerializeField] Image deathScreen;

    float lastTime, elapsed;
    float timeUntilStop = 4f;

    public bool Lose { get; set; }

    public static LoseGame Instance { get; private set; }

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
        if (!Lose) { return; }

        WaveSystem.Instance.EndWaveSystem();

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
}
