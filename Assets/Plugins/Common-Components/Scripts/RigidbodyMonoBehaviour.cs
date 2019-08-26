// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace CandyCoded.CommonComponents
{

    [RequireComponent(typeof(Rigidbody))]
    public abstract class RigidbodyMonoBehaviour : MonoBehaviour
    {

        private Rigidbody _rb;

        protected Rigidbody rb {

            get {

                if (_rb == null)
                {

                    _rb = gameObject.GetComponent<Rigidbody>();

                }

                return _rb;

            }

        }

        private bool sleepingState;

        private void OnCollisionStay()
        {

            if (rb == null || rb.IsSleeping() == sleepingState)
            {
                return;
            }

            sleepingState = rb.IsSleeping();

            if (sleepingState)
            {

                OnCollisionSleep();

            }
            else
            {

                OnCollisionAwake();

            }

        }

        protected abstract void OnCollisionSleep();

        protected abstract void OnCollisionAwake();

    }

}
