using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class RoboticGrabObject : MonoBehaviour
{
    public Transform follow;
    public bool isFollowing = false;
    public bool faseDua;
    private Rigidbody rb;
    public bool snap;

    [Header("Finger")]
    public List<RoboticAnim> grab;
    public List<RoboticAnim> lepas;

    [Header("Join")]
    public List<RoboticAnim> ambil;
    public List<RoboticAnim> ambilFaseDua;
    public List<RoboticAnim> transisi;
    public List<RoboticAnim> taruh;

    Vector3 offsetPosition;
    Vector3 offsetRotation;
    Transform parentAsli;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        parentAsli = transform.parent;
    }
    public void Simulasi(bool value = true)
    {
        rb.isKinematic = !value;
    }

    public void FollowTarget(bool value = true)
    {
        if (value)
        {
            transform.parent = follow;
            if (snap)
            {
                transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            transform.parent = parentAsli;
        }
        //offsetPosition = transform.position - follow.position;
        //offsetRotation = transform.rotation.eulerAngles - follow.rotation.eulerAngles;
        isFollowing = value;
    }
    private void Update()
    {
        //if (isFollowing)
        //{
        //    transform.position = follow.position + offsetPosition;
        //    transform.rotation = Quaternion.Euler(follow.rotation.eulerAngles + offsetRotation);
        //}
    }
    [ContextMenu("Pose Grab")]
    public void PoseGrab()
    {
        foreach (var anim in grab)
        {
            anim.join.SetRotation(anim.angle);
        }
    }
    [ContextMenu("Pose Lepas")]
    public void PoseLepas()
    {
        foreach (var anim in lepas)
        {
            anim.join.SetRotation(anim.angle);
        }
    }
    [ContextMenu("Pose Ambil")]
    public void PoseAmbil()
    {
        foreach (var anim in ambil)
        {
            anim.join.SetRotation(anim.angle);
        }
    }
    [ContextMenu("Pose Ambil Fase Dua")]
    public void PoseAmbilFAseDua()
    {
        foreach (var anim in ambilFaseDua)
        {
            anim.join.SetRotation(anim.angle);
        }
    }
    [ContextMenu("Pose Transisi")]
    public void PoseTransisi()
    {
        foreach (var anim in transisi)
        {
            anim.join.SetRotation(anim.angle);
        }
    }
    [ContextMenu("Pose Taruh")]
    public void PoseTaruh()
    {
        foreach (var anim in taruh)
        {
            anim.join.SetRotation(anim.angle);
        }
    }
    [ContextMenu("Set Lepas")]
    public void SetLepas()
    {
        foreach (var anim in lepas)
        {
            anim.angle = anim.join.rotationAngle;
        }
    }
    [ContextMenu("Set Grab")]
    public void SetGrab()
    {
        foreach (var anim in grab)
        {
            anim.angle = anim.join.rotationAngle;
        }
    }

    [ContextMenu("Set Ambil")]
    public void SetAmbil()
    {
        foreach (var anim in ambil)
        {
            anim.angle = anim.join.rotationAngle;
        }
    }
    [ContextMenu("Set Ambil Fase Dua")]
    public void SetAmbilFaseDua()
    {
        foreach (var anim in ambilFaseDua)
        {
            anim.angle = anim.join.rotationAngle;
        }
    }
    [ContextMenu("Set Transisi")]
    public void SetTransisi()
    {
        foreach (var anim in transisi)
        {
            anim.angle = anim.join.rotationAngle;
        }
    }
    [ContextMenu("Set Taruh")]
    public void SetTaruh()
    {
        foreach (var anim in taruh)
        {
            anim.angle = anim.join.rotationAngle;
        }
    }
}

[System.Serializable]
public class RoboticAnim
{
    public RoboticJoin join;
    public float angle;
}
