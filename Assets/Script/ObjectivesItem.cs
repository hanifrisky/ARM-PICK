using UnityEngine;

public class ObjectivesItem : MonoBehaviour
{
    [TextArea(3, 10)]
    public string label;
    public bool robot = false;
    public bool isFinish = false;
    public string trayTag = "Tray";
    

    private void OnTriggerEnter(Collider other)
    {
        if (isFinish)
        {
            return;
        }
        Debug.Log("Triger enter: "+other.name);
        if (!other.CompareTag(trayTag))
        {
            return;
        }
        ScrewController s = other.GetComponent<ScrewController>();
        if (s != null) s.TaruhDiTray();

        Objectives.instance.Check(this);

        ubahObject();
    }

    void ubahObject()
    {
        transform.parent = null;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if(rb == null) rb = gameObject.AddComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = false;
        isFinish = true;
    }
}
