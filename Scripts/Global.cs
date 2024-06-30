using System.Collections;
using UnityEngine;

public enum ETeamCheckResult
{
    Same,
    Neutrality,
    Other,
    Null
}

public static class Global
{
    private static bool PreventInputKey = false;
    public static bool IsNearFloatCheck(float value01, float value02, float error = 1e-4f)
    {
        float m = value02 - error;
        float p = value02 + error;

        if (value01 >= m && value01 <= p)
        {
            return true;
        }
        return false;
    }
    public static bool IsNearFloatZeroCheck(float value01, float error = 1e-4f)
    {
        return IsNearFloatCheck(value01, 0.0f, error);
    }

    public static Quaternion LookAtDirection(Vector2 value)
    {
        float atan2d = (float)Mathf.Atan2(value.y, value.x);
        return Quaternion.AngleAxis(atan2d * Mathf.Rad2Deg, Vector3.forward);
    }

    public static ETeamCheckResult TeamChecker(ITeamGenerator Team01, ITeamGenerator Team02)
    {
        if (Team01 == null || Team02 == null)
        {
            return ETeamCheckResult.Null;
        }
        if (Team01.GetTeamID() == -1 || Team02.GetTeamID() == -1)
        {
            return ETeamCheckResult.Neutrality;
        }
        if (Team01.GetTeamID() == Team02.GetTeamID())
        {
            return ETeamCheckResult.Same;
        }

        return ETeamCheckResult.Other;
    }

    public static void DrawGizmoSquare(Vector2 Center , Vector2 Size , Color InColor)
    {
        Gizmos.color = InColor;
        Vector2 halfSize = Size * 0.5f;
        Vector2 leftDown = Center + new Vector2(-halfSize.x, -halfSize.y); ;
        Vector2 leftUp = Center + new Vector2(-halfSize.x , halfSize.y);
        Vector2 rightUp = Center + new Vector2(halfSize.x, halfSize.y);
        Vector2 rightDown = Center + new Vector2(halfSize.x, -halfSize.y);

        Gizmos.DrawLine(leftDown, leftUp);
        Gizmos.DrawLine(leftUp, rightUp);
        Gizmos.DrawLine(leftDown, rightDown);
        Gizmos.DrawLine(rightDown, rightUp);

        Gizmos.color = Color.white;
    }

    public static void DrawDebugSquare(Vector2 Center, Vector2 Size, Color InColor)
    {
        Vector2 halfSize = Size * 0.5f;
        Vector2 leftDown = Center + new Vector2(-halfSize.x, -halfSize.y); ;
        Vector2 leftUp = Center + new Vector2(-halfSize.x, halfSize.y);
        Vector2 rightUp = Center + new Vector2(halfSize.x, halfSize.y);
        Vector2 rightDown = Center + new Vector2(halfSize.x, -halfSize.y);

        Debug.DrawLine(leftDown, leftUp, InColor);
        Debug.DrawLine(leftUp, rightUp, InColor);
        Debug.DrawLine(leftDown, rightDown , InColor);
        Debug.DrawLine(rightDown, rightUp, InColor);

    }

    public static IEnumerator SlowMotion(float Duration, float Scale)
    {

        PreventInputKey = true;
        Time.timeScale = Scale;
        yield return new WaitForSecondsRealtime(Duration);
        Time.timeScale = 1;

        PreventInputKey = false;

    }

    public static Vector2 RandomVector2(Vector2 Size)
    {
        float x = Random.Range(Size.x, Size.y);
        float y = Random.Range(Size.x, Size.y);
        return new Vector2(x, y);
    }

    public static Vector2 RandomVector2(Vector2 Size1, Vector2 Size2)
    {
        float x = Random.Range(Size1.x, Size1.y);
        float y = Random.Range(Size2.x, Size2.y);
        return new Vector2(x, y);
    }


    public static bool IsPreventInputKey
    {
        get => PreventInputKey;
        set => PreventInputKey = value;
    }

}
