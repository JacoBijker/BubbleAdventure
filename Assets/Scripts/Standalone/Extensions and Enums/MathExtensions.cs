using UnityEngine;

public static class MathExtensions
{
    public static bool FloatEquals(this float alpha, float beta)
    {
        return Mathf.Abs(beta - alpha) < 0.01f;
    }
}
