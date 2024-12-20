using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Concurrent;

namespace Guinea.Core.ObjectPools
{
    public static class PoolManager
    {
        private static ConcurrentDictionary<string, Pool> s_pools = new();
        public static bool TryAdd(string key, Transform prefab, PoolType poolType, int defaultCapacity=10, int maxSize=10000, bool initializedDefault=false)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException(nameof(prefab), "Prefab cannot be null.");
            }
             var pool = new Pool(prefab, poolType, defaultCapacity, maxSize, initializedDefault);
            return s_pools.TryAdd(key, pool);
        }

        public static bool TryRemove(string key)
        {
             if (s_pools.TryRemove(key, out Pool pool))
            {
                pool.pool.Clear();
                return true;
            }
            return false;     
        }

        public static Transform Get(string key)
        {
            if (s_pools.TryGetValue(key, out Pool pool))
            {
                return pool.pool.Get();
            }
            throw new KeyNotFoundException($"The pool with key '{key}' does not exist.");
        }

        public class Pool
        {
            public readonly Transform prefab;
            public readonly IObjectPool<Transform> pool;

            public Pool(Transform prefab, PoolType poolType, int defaultCapacity=10, int maxSize=10000, bool initializedDefault=false)
            {
                this.prefab = prefab;
                if(poolType == PoolType.Stack)
                {
                    pool = new ObjectPool<Transform>(CreateFunc, null, true, defaultCapacity,maxSize);
                }
                else
                {
                    pool = new LinkedPool<Transform>(CreateFunc, null, true, maxSize);
                }

                List<Transform> items = new List<Transform>();

                if(initializedDefault)
                {
                    for(int i=0; i<defaultCapacity;i++)
                    {
                        Transform item = pool.Get();
                        items.Add(item);
                    }

                    for(int i=0; i<items.Count;i++)
                    {
                        pool.Release(items[i]);
                    }
                }
            }

            private Transform CreateFunc()
            {
                Transform instance = GameObject.Instantiate(prefab);
                PoolTracker poolTracker = instance.GetComponent<PoolTracker>();
                if(poolTracker==null)
                {
                    poolTracker = instance.gameObject.AddComponent<PoolTracker>();
                }
                instance.gameObject.SetActive(false);
                poolTracker.pool = pool;
                return instance;
            }

        }

        public enum PoolType
        {
            Stack,
            LinkedList
        }
    }
}
