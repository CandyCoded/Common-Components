// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.Events;

namespace CandyCoded.CommonComponents
{

    [AddComponentMenu("CandyCoded/Drag To Rotate")]
    public class DragToRotate : MonoBehaviour
    {

        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private float _rotateSpeed = 100f;

        [SerializeField]
        private float _rotationGravity = 5f;

#pragma warning disable CS0649
        [SerializeField]
        private RotationAxis _rotationAxis;

        [SerializeField]
        private UnityEvent RotationStarted;

        [SerializeField]
        private UnityEvent RotationEnded;
#pragma warning restore CS0649

        private Transform mainCameraTransform;

        private int currentFingerId;

        private Vector3? inputPreviousPosition;

        private Vector3? delta;

        private void Awake()
        {

            if (mainCamera == null)
            {

                mainCamera = Camera.main;

            }

            mainCameraTransform = mainCamera.transform;

        }

        private void Update()
        {

            if (gameObject.GetInputDown(mainCamera, out currentFingerId, out RaycastHit hit))
            {

                inputPreviousPosition = InputManager.GetInputPosition(currentFingerId);

                Animate.StopAll(gameObject);

                RotationStarted?.Invoke();

            }
            else if (InputManager.GetInputUp(currentFingerId))
            {

                inputPreviousPosition = null;

            }
            else if (inputPreviousPosition.HasValue)
            {

                var currentInputPosition = InputManager.GetInputPosition(currentFingerId);

                if (!currentInputPosition.HasValue)
                {

                    return;

                }

                delta = mainCamera.ScreenToHighPrecisionViewportPoint(inputPreviousPosition.Value) -
                        mainCamera.ScreenToHighPrecisionViewportPoint(currentInputPosition.Value);

                gameObject.transform.RotateWithInputDelta(delta.Value,
                    _rotateSpeed,
                    mainCameraTransform, _rotationAxis);

                inputPreviousPosition = currentInputPosition;

            }
            else if (delta.HasValue)
            {

                delta = Vector3.Lerp(delta.Value, Vector3.zero, _rotationGravity * Time.deltaTime);

                gameObject.transform.RotateWithInputDelta(delta.Value,
                    _rotateSpeed,
                    mainCameraTransform, _rotationAxis);

                if (!(delta.Value.magnitude < 0.01f))
                {
                    return;
                }

                delta = null;

                RotationEnded?.Invoke();

            }

        }

    }

}
