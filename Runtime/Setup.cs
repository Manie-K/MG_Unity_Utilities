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
            Folders.CreateFolders("_Project", "_Scripts", "Materials", "Prefabs", "Settings", "Scenes");
            Folders.CreateFolders("_Project/_Scripts", "ScriptableObjects");
            AssetDatabase.Refresh();
        }   
        
        static class Folders
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
    
    }
}
