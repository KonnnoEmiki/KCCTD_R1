using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Prefabなどのアセットのパス取得用属性
public class AssetPathAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(AssetPathAttribute))]
public class AssetPathDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		Object sourceObject = null;
		if (string.IsNullOrEmpty(property.stringValue) == false)
			sourceObject = Resources.Load(property.stringValue);

		sourceObject = EditorGUI.ObjectField(position,label, sourceObject, typeof(GameObject),true);

		property.stringValue = string.Empty;
		if (sourceObject == null) return;
		
		// オブジェクトのパスを取得
		var objectPath = AssetDatabase.GetAssetPath(sourceObject);

		// resources以下のパス取得
		List<string> parsedPath = new List<string>(objectPath.Split('/'));
		int index = parsedPath.FindIndex(dir => dir.ToLower() == "resources") + 1;
		property.stringValue = parsedPath[index];
		for (++index;index < parsedPath.Count; ++index)
			property.stringValue += "/" + parsedPath[index];
		
		// 拡張子削除
		property.stringValue = GetPathWithoutExtension(property.stringValue);
	}

	// パスから拡張子を削除
	public string GetPathWithoutExtension(string path)
	{
		var ext = Path.GetExtension(path);
		if (string.IsNullOrEmpty(ext) == false)
			return path.Replace(ext, string.Empty);
		return path;
	}

}
#endif