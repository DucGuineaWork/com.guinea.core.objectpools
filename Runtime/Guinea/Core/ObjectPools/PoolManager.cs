using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Guinea.Core.ObjectPools
{
    public static class PoolManager
    {
        private static Dictionary<string, Pool> s_pools = new();
        public static bool TryAdd(string key, Transform prefab, PoolType poolType, int defaultCapacity=10, int maxSize=10000, bool initializedDefault=false)
        {
            if(!s_pools.ContainsKey(key))          
            {
                Pool pool = new Pool(prefab, poolType, defaultCapacity, maxSize, initializedDefault);
                s_pools.Add(key, pool);
                return true;
            }
            return false;
        }

        public static bool Remove(string key)
        {
            if(s_pools.TryGetValue(key, out Pool pool))
            {
                pool.pool.Clear();
                return true;
            }
            return false;            
        }

        public static Transform Get(string key)
        {
            return s_pools[key].pool.Get();
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

                if(initializedDefault)
                {
                    for(int i=0; i<defaultCapacity;i++)
                    {
                        Transform item = pool.Get();
                        pool.Release(item);
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