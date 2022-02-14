/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;
using System.Collections;

namespace DreamMachineGameStudio.Dreamworks.Tween
{
    public static partial class FTween
    {
        public static void DoMove(this Transform transform, Vector3 endPosition, float duration)
        {
            _coroutine.Start(Move(transform, endPosition, duration));
        }

        public static void DoLocalMove(this Transform transform, Vector2 endPosition, float duration)
        {
            _coroutine.Start(LocalMove(transform, endPosition, duration));
        }

        private static IEnumerator Move(Transform transform, Vector3 endPosition, float duration)
        {
            Vector3 startPosition = transform.position;

            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);

                yield return null;

                elapsedTime += Time.deltaTime;
            }
        }

        private static IEnumerator LocalMove(Transform transform, Vector3 endPosition, float duration)
        {
            Vector3 startPosition = transform.localPosition;

            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);

                yield return _waitForEndOfFrame;

                elapsedTime += Time.deltaTime;
            }

            transform.localPosition = endPosition;
        }
    }
}