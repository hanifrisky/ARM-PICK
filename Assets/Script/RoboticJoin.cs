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
        onComplete?.Invoke(false);//callback mulai

        while (time < duration)
        {
            if(RoboticAnimManager.Instance != null && RoboticAnimManager.Instance.IsPauseAnimation())
            {
                yield return null; // Skip frame jika animasi sedang dipause
                continue;
            }
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