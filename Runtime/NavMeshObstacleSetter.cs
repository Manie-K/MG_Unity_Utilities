using UnityEngine;
using UnityEngine.AI;

namespace MG_Utilities
{
    /// <summary>
    /// Used to automatically set up a NavMeshObstacle component based on the collider component attached to the same GameObject.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class NavMeshObstacleSetter : MonoBehaviour
    {
        void Awake()
        {
            Collider col = GetComponent<Collider>();
            if (col == null)
            {
                Debug.LogError("There is no collider!\n");
            }

            NavMeshObstacle obstacle = gameObject.AddComponent<NavMeshObstacle>();
            obstacle.carving = true;
            if (col is CapsuleCollider)
            {
                obstacle.center = (col as CapsuleCollider).center;
                obstacle.radius = (col as CapsuleCollider).radius;
            }else if (col is BoxCollider)
            {
                obstacle.center = (col as BoxCollider).center;
                obstacle.size = (col as BoxCollider).size;
            }
        }
    }
}
