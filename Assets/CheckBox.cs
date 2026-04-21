using UnityEngine;

public class CheckBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;
        rb.isKinematic = false;
    }
}
