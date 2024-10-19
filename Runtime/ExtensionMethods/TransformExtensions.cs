using UnityEngine;

namespace MG_Utilities
{
    public static class TransformExtensions
    {
        public static void DeleteChildren(this Transform t)
        {
            foreach(Transform child in t) 
                Object.Destroy(child.gameObject);
        }
    }
}
