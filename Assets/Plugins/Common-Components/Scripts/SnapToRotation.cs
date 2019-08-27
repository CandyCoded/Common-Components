// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace CandyCoded.CommonComponents
{

    public class SnapToRotation : MonoBehaviour
    {

        [SerializeField]
        private float _rotationSnapToAngle = 45f;

        [SerializeField]
        private float _secondsToSnapToAngle = 0.25f;

        public void SnapToRotationHandler()
        {

            Animate.Stop(gameObject, "RotateTo");

            Animate.RotateTo(gameObject, gameObject.transform.rotation.SnapRotation(_rotationSnapToAngle),
                _secondsToSnapToAngle);

        }

    }

}
