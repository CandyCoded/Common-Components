// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.Events;

namespace CandyCoded.CommonComponents
{

    public class DragToRotate : MonoBehaviour
    {

        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private float _rotateSpeed = 10f;

#pragma warning disable CS0649
        [SerializeField]
        private ROTATION_AXIS _rotationAxis;

        [SerializeField]
        private UnityEvent RotationStarted;

        [SerializeField]
        private UnityEvent RotationEnded;
#pragma warning restore CS0649

        private Transform mainCameraTransform;

        private int currentFingerId;

        private Vector3? inputPreviousPosition;

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

                RotationEnded?.Invoke();

            }
            else if (inputPreviousPosition.HasValue)
            {

                var currentInputPosition = InputManager.GetInputPosition(currentFingerId);

                if (!currentInputPosition.HasValue)
                {

                    return;

                }

                gameObject.transform.RotateWithDelta(mainCamera.ScreenToHighPrecisionViewportPoint(inputPreviousPosition.Value) - mainCamera.ScreenToHighPrecisionViewportPoint(currentInputPosition.Value),
                    _rotateSpeed,
                    mainCameraTransform, _rotationAxis);

                inputPreviousPosition = currentInputPosition;

            }

        }

    }

}
