using System.Reflection;
using UnityEditor;
using UnityEngine;

public class FileIDFinder : MonoBehaviour
{
    [MenuItem("Tools/Find File ID")]
    public static void FindFileID()
    {
        GameObject obj = Selection.activeGameObject;
        if (obj == null)
        {
            Debug.Log("Please select a GameObject in the hierarchy.");
            return;
        }

        PropertyInfo inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

        SerializedObject serializedObject = new SerializedObject(obj);
        inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

        SerializedProperty localIdProp =
            serializedObject.FindProperty("m_LocalIdentfierInFile");   //note the misspelling!

        int localId = localIdProp.intValue;

        Debug.Log("File ID of " + obj.name + " is " + localId);
    }
}
