using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.UI
{
    public static class FConsoleTouchInput
    {
        private static bool wasPressed;

        public static bool IsFourFingerTap()
        {
            if (Input.touchCount < 4)
            {
                wasPressed = false;
                return false;
            }

            bool allBegan = true;

            for (int i = 0; i < 4; ++i)
            {
                if (Input.GetTouch(i).phase != TouchPhase.Began)
                {
                    allBegan = false;
                    break;
                }
            }

            if (!allBegan)
            {
                return false;
            }

            if (wasPressed)
            {
                return false;
            }

            wasPressed = true;

            return true;
        }
    }
}
