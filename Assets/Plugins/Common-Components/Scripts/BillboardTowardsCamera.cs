// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace CandyCoded.CommonComponents
{

    public class BillboardTowardsCamera : MonoBehaviour
    {

        [SerializeField]
        private Camera mainCamera;

        private Transform mainCameraTransform;

#pragma warning disable S1144

        // Disables "Unused private types or members should be removed" warning as method is part of MonoBehaviour.
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

            gameObject.transform.LookAt(gameObject.transform.position + mainCameraTransform.rotation * Vector3.forward);

        }
#pragma warning restore S1144

    }

}
