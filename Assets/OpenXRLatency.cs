using UnityEngine;
using TMPro;

public class OpenXRLatency : MonoBehaviour
{
    [Header("Tracking")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("UI")]
    public TextMeshProUGUI text;

    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;

    public bool mulai = false;

    // SIMPAN NILAI TERAKHIR
    private float lastLatency = 0f;
    private float lastFPS = 0f;

    public void StartLatency()
    {
        mulai = true;
    }

    public void StopLatency()
    {
        mulai = false;
    }

    void Start()
    {
        mulai = false;

        if (leftHand != null) lastLeftPos = leftHand.position;
        if (rightHand != null) lastRightPos = rightHand.position;
    }

    void Update()
    {
        // kalau belum mulai → tetap tampilkan nilai terakhir
        if (!mulai)
        {
            text.text =
                lastLatency.ToString("F2") + " ms\n" +
                lastFPS.ToString("F1") + " FPS";
            return;
        }

        float latency = Time.deltaTime * 1000f;
        float fps = 1.0f / Time.deltaTime;

        float totalLatency = 0f;
        int activeHands = 0;

        bool isMoving = false;

        // LEFT HAND
        if (leftHand != null)
        {
            float moveL = Vector3.Distance(leftHand.position, lastLeftPos);

            if (moveL > 0.005f)
            {
                totalLatency += latency;
                activeHands++;
                isMoving = true;
            }

            lastLeftPos = leftHand.position;
        }

        // RIGHT HAND
        if (rightHand != null)
        {
            float moveR = Vector3.Distance(rightHand.position, lastRightPos);

            if (moveR > 0.005f)
            {
                totalLatency += latency;
                activeHands++;
                isMoving = true;
            }

            lastRightPos = rightHand.position;
        }

        // HITUNG LATENCY BARU HANYA JIKA ADA GERAKAN
        if (activeHands > 0)
        {
            lastLatency = totalLatency / activeHands;
            lastFPS = fps;
        }

        // TAMPILKAN SELALU NILAI TERAKHIR
        text.text =
            lastLatency.ToString("F2") + " ms\n" +
            lastFPS.ToString("F1") + " FPS";
    }
}