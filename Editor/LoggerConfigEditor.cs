#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MG_Utilities
{

    [CustomEditor(typeof(LoggerConfig))]
    public class MyScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LoggerConfig logger = (LoggerConfig)target;

            EditorGUILayout.LabelField("Status: " + (logger.LoggingEnabled ? "Enabled" : "Disabled"));
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Enable logging"))
            {
                logger.EnableLogging();
            }

            if (GUILayout.Button("Disable logging"))
            {
                logger.DisableLogging();
            }
        }
    }
}
#endif
