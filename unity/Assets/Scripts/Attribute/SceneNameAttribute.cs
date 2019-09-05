using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Build Settingsで登録されているシーン名をInspector上からPopupで選択,取得出来るようにする属性
public class SceneNameAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameAttributeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		
		if(property.propertyType != SerializedPropertyType.String)
		{
			// 属性を付加した変数がstring型じゃなければ通常のインスペクター表示
			EditorGUI.PropertyField(position, property, label, true);
			return;
		}

		int numScenes = EditorBuildSettings.scenes.Length;
		string[] sceneNames = new string[numScenes];
		int selectIndex = 0;
		for (int i = 0; i < numScenes; ++i)
		{
			sceneNames[i] = Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
			if (sceneNames[i] == property.stringValue)
				selectIndex = i;
		}
		selectIndex = EditorGUI.Popup(position,property.name,selectIndex,sceneNames);
		property.stringValue = sceneNames[selectIndex];

		EditorGUI.EndProperty();
	}
}

#endif
