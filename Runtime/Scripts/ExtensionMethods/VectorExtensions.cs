using UnityEngine;

namespace MG_Utilities
{
    //inspired from "git-amend" YouTube channel.
    public static class Vector3Extensions
    {
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null) 
        { 
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }

        public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(vector.x + (x ?? 0), vector.y + (y ?? 0), vector.z + (z ?? 0));
        }
    }

    public static class Vector2Extensions
    {
        public static Vector2 With(this Vector2 vector, float? x = null, float? y = null)
        {
            return new Vector2(x ?? vector.x, y ?? vector.y);
        }

        public static Vector2 Add(this Vector3 vector, float? x = null, float? y = null)
        {
            return new Vector2(vector.x + (x ?? 0), vector.y + (y ?? 0));
        }
    }
}
