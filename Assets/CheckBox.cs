using UnityEngine;

public class CheckBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }
}
