using UnityEngine;

public static class Vector3Extensions
{
    public static float SqrDistance(this Vector3 start, Vector3 end)
    {
        return (end - start).sqrMagnitude;
    }

    public static bool IsEnoughClose(this Vector3 start, Vector3 end, float distance)
    {
        return start.SqrDistance(end) <= distance * distance;
    }
}