using System.Text;
using UnityEngine;
using UnityEditor;

class ShowAssetIds
{
    [MenuItem("Tools/Show Asset Ids")]
    static void MenuShowIds()
    {
        var stringBuilder = new StringBuilder();

        foreach (var obj in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(Selection.activeObject)))
        {
            string guid;
            long file;

            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out guid, out file))
            {
                stringBuilder.Append("Asset: " + obj.name +
                    "\n  Instance ID: " + obj.GetInstanceID() +
                    "\n  GUID: " + guid +
                    "\n  File ID: " + file);
            }
        }

        Debug.Log("Asset ids :" + stringBuilder.ToString());
    }
}