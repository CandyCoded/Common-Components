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

        private float _hitDistance;

        private Vector3 _hitOffset;

        internal int _currentFingerId;

        internal Vector3? _lastInputPosition;

        internal DragState _currentState = DragState.None;

        internal Vector3 _newPosition;

        internal Vector3 _delta;

        private void Awake()
        {

            if (_mainCamera == null)
            {

                _mainCamera = Camera.main;

            }

        }

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

            _hitDistance = hit.distance;

            _lastInputPosition = GetInputPosition();

            if (_lastInputPosition.HasValue && _dragTransform)
            {

                _hitOffset = _dragTransform.position - _mainCamera.ScreenToWorldPoint(new Vector3(
                                 _lastInputPosition.Value.x,
                                 _lastInputPosition.Value.y,
                                 _hitDistance));

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

            if (currentInputPosition.HasValue && _dragTransform)
            {

                _newPosition =
                    _mainCamera.ScreenToWorldPoint(new Vector3(
                        currentInputPosition.Value.x,
                        currentInputPosition.Value.y,
                        _hitDistance)) + _hitOffset;

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
