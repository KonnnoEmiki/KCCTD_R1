using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InputAxisData : ScriptableObject
{
	private static InputAxisData s_Instance = null;
	public static InputAxisData Instance
	{
		get
		{
			if(s_Instance == null)
			{
				var asset = Resources.Load("ScriptableObjects//InputAxisData");
				if(asset == null)
				{
					Debug.AssertFormat(false,"Missing InputAxisData!");
					asset = CreateInstance<InputAxisData>();
				}
				s_Instance = asset as InputAxisData;
			}
			return s_Instance;
		}
	}

	public List<SystemDefines.InputAxis> Config = new List<SystemDefines.InputAxis>();
}

#if UNITY_EDITOR
[CustomEditor(typeof(InputAxisData))]
public class InputAxisDataEditor : Editor
{
	bool m_IsInitialized = false;
	bool m_FoldingList = false;
	bool[] m_Foldings;

	public override void OnInspectorGUI()
	{
		InputAxisData ctrl = target as InputAxisData;

		var list = ctrl.Config;

		if (!m_IsInitialized) InitializeList(list.Count);
		
		if (m_FoldingList = EditorGUILayout.Foldout(m_FoldingList, "InputAxisData"))
		{
			GUI.enabled = false;
			EditorGUI.indentLevel++;

			for (int i = 0; i<list.Count; i++)
			{
				EditorGUI.indentLevel++;
				var elem = list[i];
				// 表示名をStatusの要素に変更
				if (m_Foldings[i] = EditorGUILayout.Foldout(m_Foldings[i], list[i].Name))
				{
					EditorGUILayout.TextField("Name", elem.Name);
					EditorGUILayout.TextField("DescriptiveName", elem.DescriptiveName);
					EditorGUILayout.TextField("DescriptiveNegativeName", elem.DescriptiveNegativeName);
					EditorGUILayout.TextField("NegativeButton", elem.NegativeButton);
					EditorGUILayout.TextField("PositiveButton", elem.PositiveButton);
					EditorGUILayout.TextField("AltNegativeButton", elem.AltNegativeButton);
					EditorGUILayout.TextField("AltPositiveButton", elem.AltPositiveButton);
					EditorGUILayout.FloatField("Gravity", elem.Gravity);
					EditorGUILayout.FloatField("Dead", elem.Dead);
					EditorGUILayout.FloatField("Sensitivity", elem.Sensitivity);
					EditorGUILayout.Toggle("Snap", elem.Snap);
					EditorGUILayout.Toggle("Invert", elem.Invert);
					EditorGUILayout.EnumPopup("Type", elem.Type);
					EditorGUILayout.IntField("Axis", elem.Axis);
					EditorGUILayout.IntField("JoyNum",elem.JoyNum);
				}

				EditorGUI.indentLevel--;
			}

			// インデントを減らす
			EditorGUI.indentLevel--;
			GUI.enabled = true;
		}
	}

	// Listの長さを初期化
	void InitializeList(int count)
	{
		m_Foldings = new bool[count];
		m_IsInitialized = true;
	}

	// 指定した番号以外をキャッシュして初期化 (i = -1の時は全てキャッシュして初期化)
	void InitializeList(int i, int count)
	{
		bool[] foldings_temp = m_Foldings;
		m_Foldings = new bool[count];
	
		for (int k = 0, j = 0; k < count; k++)
		{
			if (i == j) j++;
			if (foldings_temp.Length - 1 < j) break;
			m_Foldings[k] = foldings_temp[j++];
		}
	}
}
#endif
