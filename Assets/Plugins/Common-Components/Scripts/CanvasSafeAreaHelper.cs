// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace CandyCoded.CommonComponents
{

    [RequireComponent(typeof(Canvas))]
    [AddComponentMenu("CandyCoded/Common Components/Canvas SafeArea Helper")]
    public class CanvasSafeAreaHelper : MonoBehaviour
    {

        private Canvas _canvas;

        private RectTransform _wrapperTransform;

        private void Start()
        {

            SetSafeArea();

        }

        private void SetSafeArea()
        {

            if (_wrapperTransform == null)
            {

                _canvas = gameObject.GetComponent<Canvas>();

                var wrapper = new GameObject("SafeArea");

                _wrapperTransform = wrapper.AddComponent<RectTransform>();

                for (var i = 0; i < _canvas.transform.childCount; i++)
                {

                    var child = transform.GetChild(i);

                    child.SetParent(_wrapperTransform, false);

                }

                _wrapperTransform.SetParent(_canvas.transform, false);

                _wrapperTransform.offsetMin = Vector2.zero;
                _wrapperTransform.offsetMax = Vector2.zero;

            }

            var safeArea = Screen.safeArea;

            _wrapperTransform.anchorMin = safeArea.position / _canvas.pixelRect.size;
            _wrapperTransform.anchorMax = (safeArea.position + safeArea.size) / _canvas.pixelRect.size;

        }

        private void OnRectTransformDimensionsChange()
        {

            SetSafeArea();

        }

    }

}
