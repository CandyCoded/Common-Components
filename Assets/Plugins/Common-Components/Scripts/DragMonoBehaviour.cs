// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace CandyCoded.CommonComponents
{

    public abstract class DragMonoBehaviour : MonoBehaviour
    {

        internal enum DragState
        {

            None,

            Dragging

        }

#pragma warning disable CS0649
        [SerializeField]
        internal Camera _mainCamera;

        [SerializeField]
        internal Transform _dragTransform;
#pragma warning restore CS0649

        private Vector3 _hitPosition;

        private Vector3 _hitOffset;

        internal int _currentFingerId;

        internal Vector3? _lastInputPosition;

        internal DragState _currentState = DragState.None;

        internal Vector3 _newPosition;

        internal Vector3 _delta;

        private void Update()
        {

            switch (_currentState)
            {
                case DragState.None:
                    StateNone();

                    break;

                case DragState.Dragging:
                    StateDragging();

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        internal Vector3? GetInputPosition() => InputManager.GetInputPosition(_currentFingerId);

        private void StateNone()
        {

            if (!gameObject.GetInputDown(_mainCamera, out _currentFingerId, out RaycastHit hit))
            {
                return;
            }

            _hitPosition = hit.collider.transform.position;

            _lastInputPosition = GetInputPosition();

            if (_lastInputPosition.HasValue)
            {

                _hitOffset = _hitPosition - _mainCamera.ScreenToWorldPoint(new Vector3(
                                 _lastInputPosition.Value.x,
                                 _lastInputPosition.Value.y,
                                 _hitPosition.z));

            }

            _currentState = DragState.Dragging;

            OnGrab();

        }

        private void StateDragging()
        {

            var currentInputPosition = GetInputPosition();

            if (currentInputPosition.HasValue && _lastInputPosition.HasValue)
            {

                _delta = _mainCamera.ScreenToHighPrecisionViewportPoint(currentInputPosition.Value) -
                         _mainCamera.ScreenToHighPrecisionViewportPoint(_lastInputPosition.Value);

            }

            if (currentInputPosition.HasValue)
            {

                _newPosition =
                    _mainCamera.ScreenToWorldPoint(new Vector3(
                        currentInputPosition.Value.x,
                        currentInputPosition.Value.y,
                        _hitPosition.z)) + _hitOffset;

            }

            OnDrag();

            if (_dragTransform)
            {

                _dragTransform.position = _newPosition;

            }

            _lastInputPosition = currentInputPosition;

            if (!InputManager.GetInputUp(_currentFingerId))
            {
                return;
            }

            _currentState = DragState.None;

            OnRelease();

        }

        protected abstract void OnGrab();

        protected abstract void OnDrag();

        protected abstract void OnRelease();

    }

}
