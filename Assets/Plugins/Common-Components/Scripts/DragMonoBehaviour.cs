// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace CandyCoded.CommonComponents
{

    public abstract class DragMonoBehaviour<T> : MonoBehaviour
    {

        private const float DAMPEN_INPUT_POSITION_SPEED = 0.01f;

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

        private Vector3 _hitOffset;

        internal int _currentFingerId;

        internal Vector3? _lastInputPosition;

        private Vector3? _dampenedInputPosition;

        internal DragState _currentState = DragState.None;

        internal Vector3 _newPosition;

        internal Vector3 _delta;

        internal Vector3 _velocity;

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

            if (typeof(T) == typeof(Rigidbody) &&
                !gameObject.GetInputDown(_mainCamera, out _currentFingerId, out RaycastHit _))
            {
                return;
            }

            if (typeof(T) == typeof(Rigidbody2D) &&
                !gameObject.GetInputDown(_mainCamera, out _currentFingerId, out RaycastHit2D _))
            {
                return;
            }

            _lastInputPosition = GetInputPosition();

            _dampenedInputPosition = _lastInputPosition;

            _hitOffset = _dragTransform.position - _mainCamera.ScreenToWorldPoint(
                             new Vector3(
                                 _lastInputPosition.Value.x,
                                 _lastInputPosition.Value.y,
                                 _dragTransform.position.z));

            _currentState = DragState.Dragging;

            OnGrab();

        }

        private void StateDragging()
        {

            var currentInputPosition = GetInputPosition();

            if (currentInputPosition.HasValue && _lastInputPosition.HasValue)
            {

                _delta = _mainCamera.ScreenToHighPrecisionViewportPoint(_lastInputPosition.Value) -
                         _mainCamera.ScreenToHighPrecisionViewportPoint(currentInputPosition.Value);

            }

            if (currentInputPosition.HasValue)
            {

                _newPosition =
                    _mainCamera.ScreenToWorldPoint(new Vector3(
                        currentInputPosition.Value.x,
                        currentInputPosition.Value.y,
                        _dragTransform.position.z)) + _hitOffset;

            }

            if (_dampenedInputPosition.HasValue)
            {

                _dampenedInputPosition = Vector3.Lerp(
                    _dampenedInputPosition.Value,
                    currentInputPosition.Value,
                    DAMPEN_INPUT_POSITION_SPEED);

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

            if (_lastInputPosition.HasValue && _dampenedInputPosition.HasValue)
            {

                _velocity = _mainCamera.ScreenToViewportPoint(_lastInputPosition.Value) -
                            _mainCamera.ScreenToViewportPoint(_dampenedInputPosition.Value);

            }

            _currentState = DragState.None;

            OnRelease();

        }

        protected abstract void OnGrab();

        protected abstract void OnDrag();

        protected abstract void OnRelease();

    }

}
