// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace CandyCoded.CommonComponents
{

    public class DragOnGrid : DragMonoBehaviour
    {

        [SerializeField]
        private float gridSize = 1;

        [SerializeField]
        private Vector3 gridOffset = Vector3.zero;

        protected override void OnGrab()
        {

        }

        protected override void OnDrag()
        {

            _newPosition = new Vector3(
                    Mathf.Round(_newPosition.x / gridSize) * gridSize,
                    gameObject.transform.position.y,
                    Mathf.Round(_newPosition.z / gridSize) * gridSize) + gridOffset;

        }

        protected override void OnRelease()
        {

        }

    }

}
