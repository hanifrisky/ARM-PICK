using UnityEngine;
using TMPro;
using Leap;

public class LeapLatencyDisplay : MonoBehaviour
{
    public TextMeshProUGUI latencyText;
    public LeapProvider leapProvider;

    private long lastTimestamp = 0;
    private float latency = 0f;

    private bool isStarted = false;
    Objectives ObjectivesInstance;
    void Start()
    {
        UnityEngine.Application.targetFrameRate = 90;
        isStarted = false;
        // Awal kosong
        latencyText.text = "0.00";
        ObjectivesInstance = Objectives.instance;
    }

    void Update()
    {
        //jika objectives selesai tidak perlu hitung
        if (ObjectivesInstance.isFinish()) return;
        
        // Belum start → jangan hitung
        if (!isStarted) return;

        if (leapProvider == null) return;

        Frame frame = leapProvider.CurrentFrame;
        if (frame == null) return;

        if (lastTimestamp != 0)
        {
            long deltaMicro = frame.Timestamp - lastTimestamp;
            float deltaMs = deltaMicro / 1000f;

            // filter nilai aneh
            if (deltaMs > 0 && deltaMs < 50f)
            {
                latency = Mathf.Lerp(latency, deltaMs, 0.2f);
            }
        }

        lastTimestamp = frame.Timestamp;

        // OUTPUT: hanya angka
        latencyText.text = latency.ToString("F2");
    }

    // =========================
    // FUNCTION UNTUK BUTTON START
    // =========================
    public void StartLatency()
    {
        isStarted = true;
        lastTimestamp = 0; // reset biar tidak loncat
    }
}