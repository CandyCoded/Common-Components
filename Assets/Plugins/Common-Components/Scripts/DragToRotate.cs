// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using UnityEngine;

namespace CandyCoded.CommonComponents
{

    public class DragToRotate : DragMonoBehaviour<Rigidbody>
    {

        [SerializeField]
        private float _rotateSpeed = 100f;

        [SerializeField]
        private float _rotationGravity = 5f;

        [SerializeField]
        private float _movementGravity = 5f;

#pragma warning disable CS0649
        [SerializeField]
        private RotationAxis _rotationAxis;

        [SerializeField]
        private SnapToRotation _snapToRotation;
#pragma warning restore CS0649

        private Coroutine _coroutine;

        protected override void OnGrab()
        {

            if (_coroutine == null)
            {
                return;
            }

            Animate.StopAll(gameObject);

            StopCoroutine(_coroutine);

            _coroutine = null;

        }

        protected override void OnDrag()
        {

            gameObject.transform.RotateWithInputDelta(_delta,
                _rotateSpeed,
                _mainCamera.transform, _rotationAxis);

        }

        protected override void OnRelease()
        {

            _coroutine = StartCoroutine(ContinueRotatingWithGravity());

        }

        private IEnumerator ContinueRotatingWithGravity()
        {

            while (!(_delta.magnitude < 0.01f) && !(_velocity.magnitude < 0.01f))
            {

                _delta = Vector3.Lerp(_delta, Vector3.zero, _rotationGravity * Time.deltaTime);

                _velocity = Vector3.Lerp(_velocity, Vector3.zero, _movementGravity * Time.deltaTime);

                gameObject.transform.RotateWithInputDelta(_delta,
                    _rotateSpeed,
                    _mainCamera.transform, _rotationAxis);

                gameObject.transform.position += _velocity;

                yield return null;

            }

            _snapToRotation.SnapToRotationHandler();

        }

    }

}
