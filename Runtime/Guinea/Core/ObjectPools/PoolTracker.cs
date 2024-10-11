using UnityEngine;
using UnityEngine.Pool;

namespace Guinea.Core.ObjectPools
{
    public class PoolTracker: MonoBehaviour
    {
        public IObjectPool<Transform> pool;

        void OnDisable()
        {
            pool?.Release(transform);
        }
    }
}