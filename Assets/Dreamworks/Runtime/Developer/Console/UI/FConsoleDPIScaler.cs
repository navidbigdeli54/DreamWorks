using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.UI
{
    public static class FConsoleDPIScaler
    {
        private const float ReferenceDPI = 160.0f;

        public static float GetScale()
        {
            float dpi = Screen.dpi;

            if (dpi <= 0.0f)
            {
                return 1.0f;
            }

            return Mathf.Clamp(dpi / ReferenceDPI, 1.0f, 3.0f);
        }

        public static float Scale(float value)
        {
            return value * GetScale();
        }

        public static int ScaleInt(int value)
        {
            return Mathf.RoundToInt(value * GetScale());
        }
    }
}
