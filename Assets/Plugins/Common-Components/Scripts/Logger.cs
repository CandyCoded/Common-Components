// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using UnityEngine;

namespace CandyCoded.CommonComponents
{

    public class Logger : MonoBehaviour
    {

#pragma warning disable CS0649
        [SerializeField]
        private string url;
#pragma warning restore CS0649

        private int failedConnections;

        private readonly int maxFailedConnections = 10;

#if UNITY_EDITOR || DEVELOPMENT_BUILD

        private void OnEnable()
        {

            Application.logMessageReceived += HandleLog;

        }

        private void OnDisable()
        {

            Application.logMessageReceived -= HandleLog;

        }

#endif

        private void HandleLog(string logString, string stackTrace, LogType type)
        {

            if (url != null && failedConnections < maxFailedConnections)
            {

                var loggingForm = new WWWForm();

                loggingForm.AddField("Type", type.ToString());
                loggingForm.AddField("Message", logString);
                loggingForm.AddField("Stack_Trace", stackTrace);
                loggingForm.AddField("Device_Model", SystemInfo.deviceModel);

                StartCoroutine(SendDataToLumberLog(loggingForm));

            }

        }

        private IEnumerator SendDataToLumberLog(WWWForm form)
        {
            using (WWW www = new WWW(url, form))
            {

                yield return www;

                if (!string.IsNullOrEmpty(www.error))
                {

                    Debug.LogError(www.error);

                    failedConnections += 1;

                }

            }

        }

    }

}
