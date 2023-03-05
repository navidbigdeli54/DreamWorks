/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IdentityModel.Tokens;

namespace DreamMachineGameStudio.Dreamworks.Tween
{
    public static partial class FTween
    {
        #region Public Methods
        public static void DoMove(this Transform transform, Vector3 endPosition, float duration)
        {
            _coroutine.Start(Move(transform, endPosition, duration));
        }

        public static void DoLocalMove(this Transform transform, Vector2 endPosition, float duration)
        {
            _coroutine.Start(LocalMove(transform, endPosition, duration));
        }

        public static void DoRotate(this Transform transform, Quaternion endRotation, float duration)
        {
            _coroutine.Start(Rotate(transform, endRotation, duration, null));
        }

        public static void DoRotate(this Transform transform, Quaternion endRotation, float duration, Action callback)
        {
            _coroutine.Start(Rotate(transform, endRotation, duration, callback));
        }

        public static void DoScale(this Transform transform, Vector3 endScale, float duration)
        {
            _coroutine.Start(Scale(transform, endScale, duration));
        }
        #endregion

        #region Private Methods
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

        private static IEnumerator Rotate(Transform transform, Quaternion endRotation, float duration, Action callback)
        {
            Quaternion startRotation = transform.rotation;

            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);

                yield return _waitForEndOfFrame;

                elapsedTime += Time.deltaTime;
            }

            transform.rotation = endRotation;

            callback?.Invoke();
        }

        private static IEnumerator Scale(Transform transform, Vector3 endScale, float duration)
        {
            Vector3 startScale = transform.localScale;

            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);

                yield return _waitForEndOfFrame;

                elapsedTime += Time.deltaTime;
            }

            transform.localScale = endScale;
        }
        #endregion
    }
}