using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Trigonometric
{
    public static float Radian(float angle)
    {
        return angle * (Mathf.PI / 180f);
    }

    public static Vector3 CirclePointByRadian2D(float radian, float radius)
    {
        return new Vector3(Mathf.Cos(radian) * radius, Mathf.Sin(radian) * radius, 0f);
    }

    /// <summary>
    /// center를 기준으로 point가 있는 방향을 리턴한다. (3시 방향기준)
    /// </summary>
    public static float Angle(Vector2 center, Vector2 point)
    {
        if (center == point) { return 0; }
        Vector2 dir = point - center;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }

    /// <summary>
    /// center를 기준으로 point가 있는 방향을 리턴한다. (12시 방향기준)
    /// </summary>
    public static float EulerAngle(Vector2 center, Vector2 point)
    {
        if (center == point) { return 0f; }
        Vector2 dir = point - center;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(angle<=-90)
            return angle + 270f;
        else
            return angle - 90f;
    }

    /// <summary>
    /// 반지름이 radius인 원이 있고, 원 중심을 기준으로 unityAngle방향으로 나아갔을때 만나는 접점을 리턴한다.
    /// </summary>
    public static Vector3 CirclePoint2D(float eulerAngle, float radius)
    {
        float radian = (eulerAngle + 90f) * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(radian) * radius, Mathf.Sin(radian) * radius, 0f);
    }
}
