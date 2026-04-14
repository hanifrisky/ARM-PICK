using UnityEngine;

public class RobotArmController_LengthBased : MonoBehaviour
{
    public RoboticJoin baseJoint;
    public RoboticJoin[] joints; // semua lengan (urut dari pangkal ke ujung)

    public float duration = 1f;

    public void MoveTo(Vector3 target)
    {
        if (joints.Length == 0) return;

        // === BASE ROTATION ===
        float angleBase = Mathf.Atan2(target.x, target.z) * Mathf.Rad2Deg;
        baseJoint.AnimateTo(angleBase, duration);

        // === HITUNG TARGET ===
        float distanceXZ = new Vector2(target.x, target.z).magnitude;
        float height = target.y;

        float targetDistance = Mathf.Sqrt(distanceXZ * distanceXZ + height * height);

        // === HITUNG TOTAL PANJANG ===
        float totalLength = 0f;
        foreach (var joint in joints)
        {
            totalLength += joint.length;
        }

        // Clamp supaya tidak overreach
        float clampedDistance = Mathf.Min(targetDistance, totalLength);

        float totalAngle = Mathf.Atan2(height, distanceXZ) * Mathf.Rad2Deg;

        // === DISTRIBUSI DINAMIS ===
        for (int i = 0; i < joints.Length; i++)
        {
            float ratio = joints[i].length / totalLength;

            float angle = totalAngle * ratio;

            // reach correction
            float reachFactor = clampedDistance / totalLength;
            angle *= reachFactor;

            // easing (opsional biar natural)
            float delayFactor = 1f - (i * 0.1f);

            joints[i].AnimateTo(angle, duration * delayFactor);
        }
    }
}