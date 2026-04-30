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
        //hitung jika sudah mulai, jika belum maka tidak dihitung
        if (!mulai) return;

        float latency = Time.deltaTime * 1000f;
        float fps = 1.0f / Time.deltaTime;

        float totalLatency = 0f;
        int activeHands = 0;

        // LEFT HAND
        if (leftHand != null)
        {
            float moveL = Vector3.Distance(leftHand.position, lastLeftPos);

            if (moveL > 0.005f)
            {
                totalLatency += latency;
                activeHands++;
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
            }

            lastRightPos = rightHand.position;
        }

        // HITUNG RATA-RATA
        float avgLatency = (activeHands > 0) ? totalLatency / activeHands : 0f;

        text.text =
             avgLatency.ToString("F2") + " ms\n" +
             fps.ToString("F1") + "FPS";
    }
}