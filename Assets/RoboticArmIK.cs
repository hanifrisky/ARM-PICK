using UnityEngine;

public class RoboticArmIK : MonoBehaviour
{
    [Header("Chain")]
    public Transform[] joints; // base -> end effector
    public Transform target;

    [Header("Offset")]
    public Vector3 targetOffset = new Vector3(0, 0.2f, 0);

    [Header("Settings")]
    public int iterations = 10;
    public float threshold = 0.01f;

    [Header("Smoothing")]
    public float rotationSpeed = 5f;

    [Header("End Effector")]
    public float effectorRotationSpeed = 5f;

    void LateUpdate()
    {
        if (target == null || joints.Length == 0) return;

        Vector3 desiredTargetPos = target.position + targetOffset;

        // STEP 1: Base rotate Y
        RotateBaseToTarget(desiredTargetPos);

        // STEP 2: CCD IK
        for (int iter = 0; iter < iterations; iter++)
        {
            for (int i = joints.Length - 2; i >= 0; i--)
            {
                Transform joint = joints[i];
                Transform endEffector = joints[joints.Length - 1];

                Vector3 toEnd = endEffector.position - joint.position;
                Vector3 toTarget = desiredTargetPos - joint.position;

                Vector3 axis = joint.right; // local X only

                toEnd = Vector3.ProjectOnPlane(toEnd, axis);
                toTarget = Vector3.ProjectOnPlane(toTarget, axis);

                float angle = Vector3.SignedAngle(toEnd, toTarget, axis);

                Quaternion deltaRot = Quaternion.AngleAxis(angle, axis);
                Quaternion targetRot = deltaRot * joint.rotation;

                joint.rotation = Quaternion.Slerp(
                    joint.rotation,
                    targetRot,
                    Time.deltaTime * rotationSpeed
                );
            }

            if (Vector3.Distance(joints[joints.Length - 1].position, desiredTargetPos) < threshold)
                break;
        }

        // STEP 3: End-effector menghadap ke bawah
        AlignEndEffectorDown();
    }

    void RotateBaseToTarget(Vector3 targetPos)
    {
        Transform baseJoint = joints[0];

        Vector3 direction = targetPos - baseJoint.position;
        direction.y = 0;

        if (direction.sqrMagnitude < 0.0001f) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 euler = lookRotation.eulerAngles;

        Quaternion targetRotation = Quaternion.Euler(0, euler.y, 0);

        baseJoint.rotation = Quaternion.Slerp(
            baseJoint.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    void AlignEndEffectorDown()
    {
        Transform endEffector = joints[joints.Length - 1];

        Quaternion targetRot = Quaternion.LookRotation(
            endEffector.forward,
            Vector3.down
        );

        endEffector.rotation = Quaternion.Slerp(
            endEffector.rotation,
            targetRot,
            Time.deltaTime * effectorRotationSpeed
        );
    }
}