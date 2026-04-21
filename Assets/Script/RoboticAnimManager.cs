using TMPro;
using UnityEngine;

public class RoboticAnimManager : MonoBehaviour
{
    public static RoboticAnimManager Instance;

    [Header("UI")]
    public TMP_Text durasiTunda;
    public TMP_Text jumlahTunda;

    public bool pauseAnim = false;

    // Tracking
    private float pauseStartTime = 0f;
    private float totalPauseDuration = 0f;
    private int pauseCount = 0;
    Objectives ObjectivesInstance;

    private void Awake()
    {
        Instance = this;
        pauseAnim = false;
        ResetPauseTracking();
    }
    private void Start()
    {
        ObjectivesInstance = Objectives.instance;
    }

    private void Update()
    {
        UpdateUI();
    }

    public void PauseAnimation(bool val = true)
    {
        if (pauseAnim == val) return;

        pauseAnim = val;

        if (pauseAnim)
        {
            pauseStartTime = Time.time;
            pauseCount++;
        }
        else
        {
            float pauseDuration = Time.time - pauseStartTime;
            totalPauseDuration += pauseDuration;
        }
    }

    public bool IsPauseAnimation()
    {
        return pauseAnim;
    }

    public bool IsPlayingAnimation()
    {
        return !pauseAnim;
    }

    public int GetPauseCount()
    {
        return pauseCount;
    }

    public float GetTotalPauseDuration()
    {
        if (pauseAnim)
        {
            return totalPauseDuration + (Time.time - pauseStartTime);
        }

        return totalPauseDuration;
    }

    public void ResetPauseTracking()
    {
        pauseStartTime = 0f;
        totalPauseDuration = 0f;
        pauseCount = 0;
        UpdateUI();
    }

    // ========================
    // UI HANDLER
    // ========================

    void UpdateUI()
    {
        //jika objectives selesai tidak perlu hitung
        if (ObjectivesInstance.isFinish()) return;

        if (durasiTunda != null)
        {
            durasiTunda.text = FormatTime(GetTotalPauseDuration());
        }

        if (jumlahTunda != null)
        {
            jumlahTunda.text = pauseCount.ToString();
        }
    }

    string FormatTime(float time)
    {
        int totalSeconds = Mathf.FloorToInt(time);

        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}