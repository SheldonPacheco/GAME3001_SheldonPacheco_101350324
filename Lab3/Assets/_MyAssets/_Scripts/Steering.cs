using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering
{
    public static Vector3 Seek(Vector3 seekerPosition, Vector3 seekerVelocity, float moveSpeed, Vector3 targetPosition)
    {
        Vector3 desiredVelocity = (targetPosition - seekerPosition).normalized * moveSpeed;
        return desiredVelocity;
    }
}
