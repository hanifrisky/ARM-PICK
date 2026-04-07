using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class PickableObject : MonoBehaviour
{
    public bool isPicked = false;

    public Transform gripAnchor; // 🔥 WAJIB DIISI

    public void OnPicked(Transform gripPoint)
    {
        if (gripAnchor == null)
        {
            Debug.LogError("GripAnchor belum diisi di " + gameObject.name);
            return;
        }

        isPicked = true;

        // Matikan physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Parent dulu
        transform.SetParent(gripPoint, true);

        // Reset
        transform.localRotation = Quaternion.identity;

        // 🔥 Align berdasarkan anchor (ANTI NGAMBANG)
        transform.localPosition = -gripAnchor.localPosition;
    }

    public void OnReleased()
    {
        isPicked = false;

        transform.SetParent(null, true);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}