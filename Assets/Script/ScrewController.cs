using UnityEngine;

public class ScrewController : MonoBehaviour
{
    [Header("Setup")]
    public string screwdriverTag = "Screwdriver";
    public string trayTag = "Tray";
    public Transform detachTarget;

    [Header("Rotation Settings")]
    public float maxTurns = 5f;
    public float currentTurns;

    [Header("Sensitivity")]
    public float rotationMultiplier = 1f;
    public float minDeltaThreshold = 0.2f;

    [Header("Axis Rotation Baut")]
    public Vector3 rotationAxis = Vector3.forward;

    [Header("Alignment")]
    public Transform screwNormal; // arah lubang baut
    public float maxAlignmentAngle = 20f;

    [SerializeField] private Transform activeDriver;
    private float lastAngle;
    [SerializeField] private bool isTracking = false;

    [SerializeField] public bool isDetached = false;
    public GameObject putarObeng;
    public GameObject angkat;

    void Start()
    {
        currentTurns = maxTurns;

        if (screwNormal == null)
            screwNormal = transform;
    }

    void Update()
    {
        if (!isTracking || activeDriver == null || isDetached) return;

        // Cek alignment
        if (!IsAligned()) return;

        float currentAngle = GetAngleFromAxis(activeDriver);
        float delta = Mathf.DeltaAngle(lastAngle, currentAngle);

        // filter noise Leap Motion
        if (Mathf.Abs(delta) < minDeltaThreshold) return;

        lastAngle = currentAngle;
        Debug.Log(delta);

        float turnAmount = (delta / 360f) * rotationMultiplier;

        // LOGIC PUTAR
        if (turnAmount < 0)
        {
            // membuka (clockwise)
            currentTurns += turnAmount;
        }
        else
        {
            // mengencangkan
            currentTurns += turnAmount;
            currentTurns = Mathf.Min(currentTurns, maxTurns);
        }

        currentTurns = Mathf.Max(currentTurns, 0);

        // Rotasi visual baut
        //transform.Rotate(rotationAxis * delta, Space.Self);

        // Lepas jika selesai
        if (currentTurns <= 0)
        {
            Detach();
        }
    }

    // =========================
    // DETACH BAUT
    // =========================
    void Detach()
    {
        isDetached = true;
        angkat.SetActive(true);
        putarObeng.SetActive(false);
        Debug.Log("Baut lepas");

        if (detachTarget != null)
        {
            transform.SetParent(detachTarget);
        }
    }
    public void TaruhDiTray()
    {
        angkat.SetActive(false);
        putarObeng.SetActive(false);
    }
    // =========================
    // DETEKSI OBENG MASUK
    // =========================
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (!isDetached)
        {
            putarObeng.SetActive(true);
        }
        if (other.CompareTag(trayTag))
        {
            TaruhDiTray();
        }
        if (!other.CompareTag(screwdriverTag)) return;

        activeDriver = other.transform;

        lastAngle = GetAngleFromAxis(activeDriver);
        isTracking = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isDetached)
        {
            putarObeng.SetActive(false);
        }
        //Debug.Log("Exit " + other.name);
        if (!other.CompareTag(screwdriverTag)) return;

        if (other.transform == activeDriver)
        {
            activeDriver = null;
            isTracking = false;
        }
    }

    // =========================
    // CEK ALIGNMENT BERDASARKAN NORMAL
    // =========================
    bool IsAligned()
    {
        if (activeDriver == null) return false;

        Vector3 screwDir = screwNormal.forward;
        Vector3 driverDir = activeDriver.forward;

        float dot = Vector3.Dot(screwDir.normalized, driverDir.normalized);

        // pakai abs biar 180 derajat tetap valid
        float angle = Mathf.Abs((Mathf.Acos(Mathf.Abs(dot)) * Mathf.Rad2Deg) - 90);

        return angle <= maxAlignmentAngle;
    }

    // =========================
    // AMBIL SUDUT BERDASARKAN AXIS
    // =========================
    float GetAngleFromAxis(Transform t)
    {
        Vector3 euler = t.localRotation.eulerAngles;

        Debug.Log(euler);
        if (rotationAxis == Vector3.right) return euler.x;
        if (rotationAxis == Vector3.up) return euler.y;

        return euler.z;
    }

    // =========================
    // DEBUG VISUAL (GIZMOS)
    // =========================
    void OnDrawGizmos()
    {
        if (screwNormal != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                screwNormal.position,
                screwNormal.position + screwNormal.forward * 0.1f
            );
        }

        if (activeDriver != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                activeDriver.position,
                activeDriver.position + activeDriver.forward * 0.1f
            );
        }
    }
}