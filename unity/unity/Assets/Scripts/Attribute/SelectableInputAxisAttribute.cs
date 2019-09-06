using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Inspector上からSystemDefines.InputAxisクラスまたはstringに
// Project SettingsのInputで設定したキーからPopupで設定 or Nameを取得出来るようにする属性
public class SelectableInputAxisAttribute: PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SelectableInputAxisAttribute))]
public class SelectableInputAxisDrawer : PropertyDrawer
{
	ScriptableObject m_InputAxisData;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var instance = InputAxisDataGeneratorParams.instance;
		m_InputAxisData = Resources.Load(instance.m_OutputScriptableObjectPath + instance.m_GenerateClassName) as ScriptableObject;

		bool result = false;

		if (m_InputAxisData != null)
		{
			if (fieldInfo.FieldType == typeof(string))
				result = OnGui_String(position, property, label);
			else if (fieldInfo.FieldType == typeof(SystemDefines.InputAxis))
				result = OnGui_InputAxis(position, property, label);
		}

		if(result == false)
			EditorGUI.PropertyField(position, property, label, true);
	}


	private bool OnGui_String(Rect position, SerializedProperty property, GUIContent label)
	{
		// InputAxisDataスクリプト自体生成されていないとこのスクリプトのコンパイルも通らないので
		// リフレクションでメンバ変数を取得する
		var field = m_InputAxisData.GetType().GetField("Config");
		List<SystemDefines.InputAxis> inputAxes = field.GetValue(m_InputAxisData) as List<SystemDefines.InputAxis>;

		if (inputAxes == null) return false;

		int selectIndex = 0;

		string[] inputAxisNames = new string[inputAxes.Count];
		for (int i = 0; i < inputAxes.Count; ++i)
		{
			inputAxisNames[i] = (i + 1) + ": " + inputAxes[i].Name;
			if (property.stringValue != inputAxes[i].Name)
				continue;
			selectIndex = i;
		}
		selectIndex = EditorGUI.Popup(position,property.name, selectIndex, inputAxisNames);

		if (selectIndex < inputAxisNames.Length)
			property.stringValue = inputAxes[selectIndex].Name;

		return true;
	}

	private bool OnGui_InputAxis(Rect position, SerializedProperty property, GUIContent label)
	{
		var targetObj = property.serializedObject.targetObject;
		var propertyField = targetObj.GetType().GetField(property.name,BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		SystemDefines.InputAxis propertyInputAxis = propertyField.GetValue(targetObj) as SystemDefines.InputAxis;

		// InputAxisDataスクリプト自体生成されていないとこのスクリプトのコンパイルも通らないので
		// リフレクションでメンバ変数を取得する
		var field = m_InputAxisData.GetType().GetField("Config");
		List<SystemDefines.InputAxis> inputAxes = field.GetValue(m_InputAxisData) as List<SystemDefines.InputAxis>;

		if (inputAxes == null) return false;

		int selectIndex = 0;

		string[] inputAxisNames = new string[inputAxes.Count];
		for (int i = 0; i < inputAxes.Count; ++i)
		{
			inputAxisNames[i] = (i + 1) + ": " + inputAxes[i].Name;
			if (propertyInputAxis != inputAxes[i])
				continue;
			selectIndex = i;
		}
		selectIndex = EditorGUI.Popup(position, property.name, selectIndex, inputAxisNames);

		if (selectIndex < inputAxes.Count)
			propertyInputAxis = inputAxes[selectIndex];

		propertyField.SetValue(targetObj, propertyInputAxis);
		return true;
	}

}
#endif