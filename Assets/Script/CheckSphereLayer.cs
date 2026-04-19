using UnityEngine;

public class SpherePauseDetector : MonoBehaviour
{
    public float radius = 1.5f;
    public Vector3 offset;
    public LayerMask targetLayer;

    [Header("Anti Flicker Settings")]
    public float enterDelay = 0.1f; // waktu minimal untuk dianggap menyentuh
    public float exitDelay = 0.15f; // waktu minimal untuk dianggap lepas

    private float enterTimer = 0f;
    private float exitTimer = 0f;

    private bool isInside = false;     // kondisi real-time (hasil physics)
    private bool isPausedState = false; // kondisi yang sudah stabil

    void Update()
    {
        CheckSphere();
        HandleState();
    }

    void CheckSphere()
    {
        Vector3 center = transform.position + offset;
        Collider[] hits = Physics.OverlapSphere(center, radius, targetLayer);

        isInside = hits.Length > 0;
    }

    void HandleState()
    {
        if (isInside)
        {
            enterTimer += Time.deltaTime;
            exitTimer = 0f;

            if (!isPausedState && enterTimer >= enterDelay)
            {
                isPausedState = true;
                RoboticAnimManager.Instance.PauseAnimation(true);
            }
        }
        else
        {
            exitTimer += Time.deltaTime;
            enterTimer = 0f;

            if (isPausedState && exitTimer >= exitDelay)
            {
                isPausedState = false;
                RoboticAnimManager.Instance.PauseAnimation(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isPausedState ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}