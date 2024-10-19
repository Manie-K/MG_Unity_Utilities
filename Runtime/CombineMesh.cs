using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MG_Utilities
{
    /// <summary>
    /// Used to combine multiple meshes into a single mesh, in order to create an asset.
    /// DISCLAIMER: This script is not intended to be used in production, it's just a utility script to help with the creation of assets.
    /// </summary>
    public class CombineMesh : MonoBehaviour
    {
        [SerializeField] bool allMeshesInChildren;
        [SerializeField] string targetMeshName;
        [SerializeField] List<MeshFilter> meshesToCombine;

        [ContextMenu("Combine meshes")]
        void CombineIntoMesh()
        {
            var combine = new CombineInstance[meshesToCombine.Count];

            if (allMeshesInChildren)
            {
                meshesToCombine = GetComponentsInChildren<MeshFilter>().ToList();
            }
            
            for (int i = 0; i < meshesToCombine.Count; i++)
            {
                combine[i].mesh = meshesToCombine[i].sharedMesh;
                combine[i].transform = Matrix4x4.TRS(meshesToCombine[i].transform.localPosition,
                    meshesToCombine[i].transform.rotation, 
                    meshesToCombine[i].transform.localScale
                    );
            }

            var mesh = new Mesh();
            mesh.CombineMeshes(combine);

            string path = $"Assets/_Project/Meshes/{targetMeshName}.asset";
            
            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
