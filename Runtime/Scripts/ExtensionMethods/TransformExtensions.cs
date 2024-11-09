using UnityEngine;
using System.Collections.Generic;

namespace MG_Utilities
{
    public static class TransformExtensions
    {
        public static void DeleteChildren(this Transform t)
        {
            if (Application.isEditor)
            {
                foreach (Transform child in t)
                    Object.DestroyImmediate(child.gameObject);
            }
            else
            {
                foreach (Transform child in t)
                    Object.Destroy(child.gameObject);
            }
        }

        public static List<T> GetComponentsInDirectChildren<T>(this Transform t) where T : Component
        {
            List<T> components = new List<T>();
            foreach (Transform child in t)
            {
                T component = child.GetComponent<T>();
                if (component != null)
                    components.Add(component);
            }
            return components;
        }
    }
}
