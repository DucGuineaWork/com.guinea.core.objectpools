using UnityEngine;

namespace Guinea.Core.ObjectPools
{
    using PoolContainerTransformType = PoolContainer<Transform>;
    public class PoolContainerTransform : PoolContainerTransformType
    {
        private static PoolContainerTransformType s_instance;

        public static PoolContainerTransformType GetInstance()
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<PoolContainerTransformType>();
            }
            return s_instance;
        }

        new public static void AddPool(string key, Transform prefab, int count = 0)
        {
            GetInstance().AddPool(key, prefab, count);
        }

        new public static void CleanPool(string key)
        {
            GetInstance()?.CleanPool(key);
        }

        new public static void ClearPool(string key)
        {
            GetInstance()?.ClearPool(key);
        }

        new public static Transform GetPool(string key, Transform parent = null, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
        {
            return GetInstance().GetPool(key, parent, position, rotation);
        }
    }

}