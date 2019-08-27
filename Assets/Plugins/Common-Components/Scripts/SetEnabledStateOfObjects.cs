// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace CandyCoded.CommonComponents
{

    public class SetEnabledStateOfObjects : MonoBehaviour
    {

        public Object[] objects;

        public void SetEnabledStateOfObjectsHandler(bool enabledState)
        {

            foreach (var obj in objects)
            {

                switch (obj)
                {
                    case MonoBehaviour behaviour:
                        behaviour.enabled = enabledState;

                        break;

                    case GameObject o:
                        o.SetActive(enabledState);

                        break;
                }

            }

        }

    }

}
