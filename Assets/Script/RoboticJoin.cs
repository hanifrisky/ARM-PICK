//using System;
//using System.Collections;
//using UnityEngine;

//public class RoboticJoin : MonoBehaviour
//{
//    public float restAxis;

//    public enum JoinAxis
//    {
//        X, Y
//    }
//    public enum JoinDirection
//    {
//        positif, negatif
//    }

//    public JoinAxis rotationAxis = JoinAxis.X;
//    public JoinDirection direction = JoinDirection.positif;

//    public float rotationAngle = 0f;
//    [ContextMenu("Rest Axis")]
//    public void RestAxis()
//    {
//        rotationAngle = restAxis;
//        SetRotation(restAxis);
//    }
//    [ContextMenu("Set Rest Axis")]
//    public void SetRestAxis()
//    {
//        restAxis = rotationAngle;
//    }
//    private void OnValidate()
//    {
//        if(!Application.isPlaying)
//            SetRotation(rotationAngle);
//    }
//    public void SetRotation(float angle)
//    {
//        rotationAngle = angle;
//        float signedAngle = direction == JoinDirection.positif ? rotationAngle : -rotationAngle;
//        Vector3 newRotation = Vector3.zero;
//        if (rotationAxis == JoinAxis.X)
//        {
//            newRotation.x = signedAngle;
//        }
//        else if (rotationAxis == JoinAxis.Y)
//        {
//            newRotation.y = signedAngle;
//        }
//        transform.localEulerAngles = newRotation;
//    }

//    // =========================
//    // ANIMATION METHOD
//    // =========================
//    public void AnimateTo(float targetAngle, float duration, Action<bool> onComplete = null)
//    {
//        StopAllCoroutines();
//        StartCoroutine(AnimateRotationCoroutine(targetAngle, duration, onComplete));
//    }

//    private IEnumerator AnimateRotationCoroutine(float targetAngle, float duration, Action<bool> onComplete)
//    {
//        float startAngle = rotationAngle;
//        float time = 0f;

//        while (time < duration)
//        {
//            time += Time.deltaTime;
//            float t = time / duration;

//            float currentAngle = Mathf.Lerp(startAngle, targetAngle, t);
//            SetRotation(currentAngle);

//            yield return null;
//        }

//        // Ensure exact final value
//        SetRotation(targetAngle);

//        // Callback selesai
//        onComplete?.Invoke(true);
//    }
//}
using UnityEngine;
using System;
using System.Collections;
public class RoboticJoin : MonoBehaviour
{
    public float restAxis;
    public float length = 1f;
    public enum JoinAxis { X, Y }
    public enum JoinDirection { positif, negatif }

    public JoinAxis rotationAxis = JoinAxis.X;
    public JoinDirection direction = JoinDirection.positif;

    public float rotationAngle = 0f;

    [Header("Constraint")]
    public float minAngle = -90f;
    public float maxAngle = 90f;

    [Header("Speed")]
    public float rotationSpeed = 200f;

    [ContextMenu("Rest Axis")]
    public void RestAxis()
    {
        SetRotation(restAxis);
    }

    [ContextMenu("Set Rest Axis")]
    public void SetRestAxis()
    {
        restAxis = rotationAngle;
    }

    // =========================
    // ANIMATION METHOD
    // =========================
    public void AnimateTo(float targetAngle, float duration, Action<bool> onComplete = null)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateRotationCoroutine(targetAngle, duration, onComplete));
    }

    private IEnumerator AnimateRotationCoroutine(float targetAngle, float duration, Action<bool> onComplete)
    {
        float startAngle = rotationAngle;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            float currentAngle = Mathf.Lerp(startAngle, targetAngle, t);
            SetRotation(currentAngle);

            yield return null;
        }

        // Ensure exact final value
        SetRotation(targetAngle);

        // Callback selesai
        onComplete?.Invoke(true);
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
            ApplyRotation();
    }

    // SET ABSOLUTE (dipakai reset / init)
    public void SetRotation(float angle)
    {
        //rotationAngle = Mathf.Clamp(angle, minAngle, maxAngle);
        rotationAngle = angle;
        ApplyRotation();
    }

    // TAMBAH ROTASI (dipakai IK)
    public void AddRotation(float delta)
    {
        float dir = direction == JoinDirection.positif ? 1f : -1f;

        rotationAngle += delta * dir;
        rotationAngle = Mathf.Clamp(rotationAngle, minAngle, maxAngle);

        ApplyRotation();
    }

    void ApplyRotation()
    {
        Vector3 current = transform.localEulerAngles;

        float angle = direction == JoinDirection.positif ? rotationAngle : -rotationAngle;

        if (rotationAxis == JoinAxis.X)
            transform.localRotation = Quaternion.Euler(angle, 0, 0);

        else if (rotationAxis == JoinAxis.Y)
            transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    // Untuk IK: ambil axis dunia
    public Vector3 GetWorldAxis()
    {
        if (rotationAxis == JoinAxis.X)
            return transform.right;

        return transform.up;
    }
}