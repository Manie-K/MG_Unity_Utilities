using UnityEngine;

namespace MG_Utilities
{
    public static class MGUtilities
    {
        public static void DeleteChildren(this Transform t)
        {
            foreach(Transform child in t) Object.Destroy(child.gameObject);
        }
    }
}
