using System;
using UnityEngine;

namespace Guinea.Core.ObjectPools
{
    public class ObjectPool<T> : UnityEngine.Pool.ObjectPool<T> where T : UnityEngine.Component
    {
        public ObjectPool(Func<T> createFunc, Action<T> actionOnGet = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000) : base(createFunc, actionOnGet, null, OnDestroyPoolObject, collectionCheck, defaultCapacity, maxSize){
            
        }

        // private static void OnReleasePoolObject(T o)
        // {
        //     o.gameObject.SetActive(false);
        // }

        private static void OnDestroyPoolObject(T o)
        {
            GameObject.Destroy(o.gameObject);
        }
    }
}