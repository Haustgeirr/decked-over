using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math3D
{
    public static IEnumerator CardAnim(GameObject card, Vector3 endPos, Quaternion endRot, float moveDuration)
    {
        var tf = card.transform;
        var startPosition = tf.position;
        var startRotation = tf.rotation;

        var moveTimer = 0f;
        // var moveDuration = 0.1f;

        while (moveTimer < moveDuration)
        {
            tf.position = Vector3.Lerp(startPosition, endPos, Math3D.CubicEaseOut(moveTimer, moveDuration));
            tf.rotation = Quaternion.Lerp(startRotation, endRot, Math3D.CubicEaseOut(moveTimer, moveDuration));
            moveTimer += Time.deltaTime;
            yield return null;
        }

        tf.position = endPos;
        yield return null;
    }

    public static float CubicEaseOut(float t, float duration)
    {
        t /= duration;
        t--;

        return (t * t * t + 1);
    }

    public static void Arrow2D(Vector2 point, Vector2 direction, float scale)
    {
        float angle = 15.0f;

        Vector2 forward = (direction - point).normalized;
        Vector2 right = new Vector2(forward.y, -forward.x).normalized;
        Vector2 endPoint = forward * scale + point;

        Gizmos.DrawLine(point, endPoint);
        Gizmos.DrawLine(endPoint, Vector3.Slerp(-forward, right, angle / 90.0f) + (Vector3)endPoint);
        Gizmos.DrawLine(endPoint, Vector3.Slerp(-forward, -right, angle / 90.0f) + (Vector3)endPoint);
        Gizmos.DrawLine(Vector3.Slerp(-forward, right, angle / 90.0f) + (Vector3)endPoint, Vector3.Slerp(-forward, -right, angle / 90.0f) + (Vector3)endPoint);
    }
}
