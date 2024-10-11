using System;
using UnityEngine;

namespace Guinea.Core.ObjectPools
{
      public class LinkedPool<T> : UnityEngine.Pool.LinkedPool<T> where T : UnityEngine.Component
    {
        public LinkedPool(Func<T> createFunc, Action<T> actionOnGet = null, bool collectionCheck = true, int maxSize = 10000) : base(createFunc, actionOnGet, OnReleasePoolObject, OnDestroyPoolObject, collectionCheck, maxSize){
            
        }

        private static void OnReleasePoolObject(T o)
        {
            o.gameObject.SetActive(false);
        }

        private static void OnDestroyPoolObject(T o)
        {
            GameObject.Destroy(o.gameObject);
        }
    }
}