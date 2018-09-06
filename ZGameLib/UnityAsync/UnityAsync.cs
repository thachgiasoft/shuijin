using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ZGameLib.UnityAsync
{
    public class UnityAsync : IDisposable
    {
        private AsyncDriver AsyncDriver { get; set; }

        public UnityAsync()
        {
            AsyncDriver = new GameObject("AsyncDriver", new Type[] { typeof(AsyncDriver) }).GetComponent<AsyncDriver>();
            GameObject obj = AsyncDriver.gameObject;
            UnityEngine.Object.DontDestroyOnLoad(obj);
        }

        public void StartCoroutine(IEnumerator enumerator)
        {
            if (AsyncDriver != null)
            {
                AsyncDriver.StartCoroutine(enumerator);
            }
        }

        public void StopCoroutine(IEnumerator enumerator)
        {
            if (AsyncDriver != null)
            {
                AsyncDriver.StopCoroutine(enumerator);
            }
        }

        public void StopAllCoroutines()
        {
            if (AsyncDriver != null)
            {
                AsyncDriver.StopAllCoroutines();
            }
        }

        public void Dispose()
        {
            StopAllCoroutines();
            UnityEngine.Object.DestroyImmediate(AsyncDriver.gameObject);
            AsyncDriver = null;
        }
    }

    public class AsyncDriver : MonoBehaviour
    {
    }
}