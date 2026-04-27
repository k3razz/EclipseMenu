using UnityEngine;

namespace EclipseMenu
{
    public static class FPSOverlay
    {
        public static bool Enabled = true;

        private static float _deltaTime;

        public static string GetFPS()
        {
            if (!Enabled)
                return "";

            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            return $"FPS: {Mathf.CeilToInt(1.0f / _deltaTime)}";
        }
    }
}
