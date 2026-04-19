using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmManualController : MonoBehaviour
{
    [Header("Joint (1 - 5)")]
    public Transform joint1;
    public Transform joint2;
    public Transform joint3;
    public Transform joint4;
    public Transform joint5;

    [Header("Gripper")]
    public Transform gripPoint; // WAJIB DIISI

    [Header("Finger")]
    public Transform leftFinger;
    public Transform rightFinger;

    public float openAngle = 30f;
    public float closeAngle = 0f;
    public float fingerSpeed = 5f;

    [Header("Speed")]
    public float moveSpeed = 2f;

    [Header("Targets")]
    public List<PickableObject> targets = new List<PickableObject>();

    [Header("POSE - START")]
    public Vector3 j1_start, j2_start, j3_start, j4_start, j5_start;

    [Header("POSE - PICK")]
    public Vector3 j1_pick, j2_pick, j3_pick, j4_pick, j5_pick;

    [Header("POSE - PLACE")]
    public Vector3 j1_place, j2_place, j3_place, j4_place, j5_place;

    private int index = 0;
    private bool isRunning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRunning)
        {
            if (gripPoint == null)
            {
                UnityEngine.Debug.LogError("GRIP POINT BELUM DIISI!");
                return;
            }

            isRunning = true;
            StartCoroutine(RobotRoutine());
        }
    }

    IEnumerator RobotRoutine()
    {
        index = 0;

        while (index < targets.Count)
        {
            PickableObject obj = targets[index];

            if (obj != null && !obj.isPicked)
            {
                yield return MoveToPose(j1_pick, j2_pick, j3_pick, j4_pick, j5_pick);

                yield return SetFinger(closeAngle);

                obj.OnPicked(gripPoint);

                yield return new WaitForSeconds(0.3f);

                yield return MoveToPose(j1_place, j2_place, j3_place, j4_place, j5_place);

                yield return SetFinger(openAngle);

                obj.OnReleased();

                yield return new WaitForSeconds(0.2f);

                yield return MoveToPose(j1_start, j2_start, j3_start, j4_start, j5_start);
            }

            index++;
            yield return null;
        }

        UnityEngine.Debug.Log("SELESAI SEMUA");
        isRunning = false;
    }

    IEnumerator MoveToPose(Vector3 t1, Vector3 t2, Vector3 t3, Vector3 t4, Vector3 t5)
    {
        float time = 0f;
        float duration = 1f;

        Vector3 s1 = joint1.localEulerAngles;
        Vector3 s2 = joint2.localEulerAngles;
        Vector3 s3 = joint3.localEulerAngles;
        Vector3 s4 = joint4.localEulerAngles;
        Vector3 s5 = joint5.localEulerAngles;

        while (time < duration)
        {
            time += Time.deltaTime * moveSpeed;
            float t = Mathf.Clamp01(time / duration);

            joint1.localEulerAngles = LerpEuler(s1, t1, t);
            joint2.localEulerAngles = LerpEuler(s2, t2, t);
            joint3.localEulerAngles = LerpEuler(s3, t3, t);
            joint4.localEulerAngles = LerpEuler(s4, t4, t);
            joint5.localEulerAngles = LerpEuler(s5, t5, t);

            yield return null;
        }
    }

    IEnumerator SetFinger(float target)
    {
        float time = 0f;
        float duration = 1f;

        float startL = leftFinger.localEulerAngles.z;
        float startR = rightFinger.localEulerAngles.z;

        while (time < duration)
        {
            time += Time.deltaTime * fingerSpeed;
            float t = Mathf.Clamp01(time / duration);

            float l = Mathf.LerpAngle(startL, target, t);
            float r = Mathf.LerpAngle(startR, -target, t);

            leftFinger.localEulerAngles = new Vector3(0f, 0f, l);
            rightFinger.localEulerAngles = new Vector3(0f, 0f, r);

            yield return null;
        }
    }

    Vector3 LerpEuler(Vector3 a, Vector3 b, float t)
    {
        return new Vector3(
            Mathf.LerpAngle(a.x, b.x, t),
            Mathf.LerpAngle(a.y, b.y, t),
            Mathf.LerpAngle(a.z, b.z, t)
        );
    }
}