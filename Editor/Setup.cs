using System.IO;
using UnityEngine;
using UnityEditor;

namespace MG_Utilities
{
    public static class MGSetup 
    {
        [MenuItem("Tools/Setup/Create default folders")]
        public static void CreateDefaultFolders()
        {
            FolderHelpers.CreateFolders("_Project", "_Scripts", "Materials", "Prefabs", 
                "Settings", "Scenes", "Shaders", "Meshes");
            FolderHelpers.CreateFolders("_Project/_Scripts", "ScriptableObjects");
            AssetDatabase.Refresh();
        }   
        
        [MenuItem("Tools/Setup/Import Unity Packages")]
        public static void ImportUnityPackages()
        {
            PackageHelpers.AddPackage("com.unity.cinemachine");
            PackageHelpers.AddPackage("com.unity.ai.navigation");
        }
        
        
        static class FolderHelpers
        {
            public static void CreateFolders(string root, params string[] folders) 
            {
                var fullPath = Path.Combine(Application.dataPath, root);
                foreach (var folder in folders)
                {
                    var path = Path.Combine(fullPath, folder);
                    if(Directory.Exists(path))
                    {
                        Debug.LogError("Folder '" + folder + "' already exists!");
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                        Debug.Log("Folder '" + folder + "' created successfully!");
                    }
                }

            }
        }
        static class PackageHelpers
        {
            public static void AddPackage(string packageName)
            {
                var request = UnityEditor.PackageManager.Client.Add(packageName);
                while (!request.IsCompleted)
                {
                    System.Threading.Thread.Sleep(10);
                }
        
                if (request.Status == UnityEditor.PackageManager.StatusCode.Success)
                {
                    Debug.Log($"Package '{packageName}' imported successfully.");
                }
                else if (request.Status >= UnityEditor.PackageManager.StatusCode.Failure)
                {
                    Debug.LogError($"Failed to import package '{packageName}': {request.Error.message}");
                }
            }
        }
    }
}
