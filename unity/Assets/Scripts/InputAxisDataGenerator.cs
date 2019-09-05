#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;

public class InputAxisDataGeneratorParams : ScriptableSingleton<InputAxisDataGeneratorParams>
{
	// InputAxisDataから取り出したデータ
	public List<SystemDefines.InputAxis> m_InputAxes = null;
	// スクリプトの出力パス
	public string m_OutputScriptFilePath = "Scripts/ScriptableObject/";
	// スクリプタブルオブジェクトの出力パス
	public string m_OutputScriptableObjectPath = "ScriptableObjects/";
	// 生成クラス名
	public string m_GenerateClassName = "InputAxisData";
}

public class InputAxisDataGenerator : AssetPostprocessor
{
	private const string InputAxisDataTemplate = @"using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InputAxisData : ScriptableObject
{{
	private static InputAxisData s_Instance = null;
	public static InputAxisData Instance
	{{
		get
		{{
			if(s_Instance == null)
			{{
				var asset = Resources.Load(""{0}"");
				if(asset == null)
				{{
					Debug.AssertFormat(false,""Missing InputAxisData!"");
					asset = CreateInstance<InputAxisData>();
				}}
				s_Instance = asset as InputAxisData;
			}}
			return s_Instance;
		}}
	}}

	public List<SystemDefines.InputAxis> Config = new List<SystemDefines.InputAxis>();
}}

#if UNITY_EDITOR
[CustomEditor(typeof(InputAxisData))]
public class InputAxisDataEditor : Editor
{{
	bool m_IsInitialized = false;
	bool m_FoldingList = false;
	bool[] m_Foldings;

	public override void OnInspectorGUI()
	{{
		InputAxisData ctrl = target as InputAxisData;

		var list = ctrl.Config;

		if (!m_IsInitialized) InitializeList(list.Count);
		
		if (m_FoldingList = EditorGUILayout.Foldout(m_FoldingList, ""InputAxisData""))
		{{
			GUI.enabled = false;
			EditorGUI.indentLevel++;

			for (int i = 0; i<list.Count; i++)
			{{
				EditorGUI.indentLevel++;
				var elem = list[i];
				// 表示名をStatusの要素に変更
				if (m_Foldings[i] = EditorGUILayout.Foldout(m_Foldings[i], list[i].Name))
				{{
					EditorGUILayout.TextField(""Name"", elem.Name);
					EditorGUILayout.TextField(""DescriptiveName"", elem.DescriptiveName);
					EditorGUILayout.TextField(""DescriptiveNegativeName"", elem.DescriptiveNegativeName);
					EditorGUILayout.TextField(""NegativeButton"", elem.NegativeButton);
					EditorGUILayout.TextField(""PositiveButton"", elem.PositiveButton);
					EditorGUILayout.TextField(""AltNegativeButton"", elem.AltNegativeButton);
					EditorGUILayout.TextField(""AltPositiveButton"", elem.AltPositiveButton);
					EditorGUILayout.FloatField(""Gravity"", elem.Gravity);
					EditorGUILayout.FloatField(""Dead"", elem.Dead);
					EditorGUILayout.FloatField(""Sensitivity"", elem.Sensitivity);
					EditorGUILayout.Toggle(""Snap"", elem.Snap);
					EditorGUILayout.Toggle(""Invert"", elem.Invert);
					EditorGUILayout.EnumPopup(""Type"", elem.Type);
					EditorGUILayout.IntField(""Axis"", elem.Axis);
					EditorGUILayout.IntField(""JoyNum"",elem.JoyNum);
				}}

				EditorGUI.indentLevel--;
			}}

			// インデントを減らす
			EditorGUI.indentLevel--;
			GUI.enabled = true;
		}}
	}}

	// Listの長さを初期化
	void InitializeList(int count)
	{{
		m_Foldings = new bool[count];
		m_IsInitialized = true;
	}}

	// 指定した番号以外をキャッシュして初期化 (i = -1の時は全てキャッシュして初期化)
	void InitializeList(int i, int count)
	{{
		bool[] foldings_temp = m_Foldings;
		m_Foldings = new bool[count];
	
		for (int k = 0, j = 0; k < count; k++)
		{{
			if (i == j) j++;
			if (foldings_temp.Length - 1 < j) break;
			m_Foldings[k] = foldings_temp[j++];
		}}
	}}
}}
#endif
";

	// InputManagerが更新されたか
	private static bool s_IsInputManagerUpdated = false;

	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
	{
		var instance = InputAxisDataGeneratorParams.instance;
		s_IsInputManagerUpdated = false;
		// InputManagerの変更チェック
		var InputManagerPath = Array.Find(importedAssets, path => Path.GetFileName(path) == "InputManager.asset");
		if (InputManagerPath == null)
			return;

		s_IsInputManagerUpdated = true;
		// InputManagerの設定情報読み込み
		var serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(InputManagerPath)[0]);
		var axesProperty = serializedObject.FindProperty("m_Axes");
		instance.m_InputAxes = new List<SystemDefines.InputAxis>();

		for (int i = 0; i < axesProperty.arraySize; ++i)
		{
			var axis = new SystemDefines.InputAxis();
			var axisProperty = axesProperty.GetArrayElementAtIndex(i);

			axis.Name = GetChildProperty(axisProperty, "m_Name").stringValue;
			axis.DescriptiveName = GetChildProperty(axisProperty, "descriptiveName").stringValue;
			axis.DescriptiveNegativeName = GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue;
			axis.NegativeButton = GetChildProperty(axisProperty, "negativeButton").stringValue;
			axis.PositiveButton = GetChildProperty(axisProperty, "positiveButton").stringValue;
			axis.AltNegativeButton = GetChildProperty(axisProperty, "altNegativeButton").stringValue;
			axis.AltPositiveButton = GetChildProperty(axisProperty, "altPositiveButton").stringValue;
			axis.Gravity = GetChildProperty(axisProperty, "gravity").floatValue;
			axis.Dead = GetChildProperty(axisProperty, "dead").floatValue;
			axis.Sensitivity = GetChildProperty(axisProperty, "sensitivity").floatValue;
			axis.Snap = GetChildProperty(axisProperty, "snap").boolValue;
			axis.Invert = GetChildProperty(axisProperty, "invert").boolValue;
			axis.Type = (SystemDefines.AxisType)GetChildProperty(axisProperty, "type").intValue;
			axis.Axis = GetChildProperty(axisProperty, "axis").intValue;
			axis.JoyNum = GetChildProperty(axisProperty, "joyNum").intValue;

			instance.m_InputAxes.Add(axis);
		}
		
		var inputAxisDataResult = Application.dataPath + "/" + instance.m_OutputScriptFilePath + instance.m_GenerateClassName + ".cs";
		bool isExist = File.Exists(inputAxisDataResult);
		string outputTemplateSource = string.Format(InputAxisDataTemplate, instance.m_OutputScriptableObjectPath + "/" + instance.m_GenerateClassName);

		if (isExist)
		{
			var fs = new FileStream(inputAxisDataResult, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using (var reader = new StreamReader(fs))
			{
				string sourcefile = reader.ReadToEnd();
				if (sourcefile != outputTemplateSource) // 一致しなければ
					OutputInputAxisDataTemplate(inputAxisDataResult, outputTemplateSource);
				else // 一致する場合コンパイルが通らないのでコンパイル後に通る想定だった関数を自前で呼ぶ
					OnComplied();
			}
		}
		else
			OutputInputAxisDataTemplate(inputAxisDataResult, outputTemplateSource);
		
	}

	// InputAxisDataスクリプト書き出し
	private static void OutputInputAxisDataTemplate(string path,string sourceText)
	{
		var instance = InputAxisDataGeneratorParams.instance;
		var sr = new StreamWriter(path);
		sr.Write(sourceText);
		sr.Close();

		// 生成したInputAxisData.csをインポート
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
	}

	// 親プロパティから特定の名前の子プロパティを取得
	private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
	{
		SerializedProperty child = parent.Copy();
		child.Next(true);
		do
		{
			if (child.name == name)
				return child;
		}
		while (child.Next(false));
		return null;
	}

	// スクリプコンパイル後
	[DidReloadScripts]
	private static void OnComplied()
	{
		// InputManagerが更新されていないなら処理しない
		if (s_IsInputManagerUpdated == false)
			return;

		var instance = InputAxisDataGeneratorParams.instance;

		if (File.Exists(Application.dataPath + "/" + instance.m_OutputScriptFilePath + instance.m_GenerateClassName + ".cs") == false)
			return;

		// コードのコンパイルが完了したらScriptableObject生成
		object o = Activator.CreateInstance(Type.GetType(instance.m_GenerateClassName));

		ScriptableObject scriptableObject = o as ScriptableObject;
		if (scriptableObject == null)
		{
			Debug.Log("error");
			return;
		}
		
		CreateScriptableObject(scriptableObject);
		s_IsInputManagerUpdated = false;
	}

	// スクリプタブルオブジェクトのインスタンス生成
	private static void CreateScriptableObject(ScriptableObject scriptableObj)
	{
		var instance = InputAxisDataGeneratorParams.instance;
		
		string outputDir = "Assets/Resources/" + instance.m_OutputScriptableObjectPath;
		string assetPath = outputDir + instance.m_GenerateClassName + ".asset";
		if (Directory.Exists(outputDir) == false)
			Directory.CreateDirectory(outputDir);
		
		if (File.Exists(assetPath) == false) // 存在しなければ生成
		{
			string uniquePath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
			AssetDatabase.CreateAsset(scriptableObj, uniquePath);
		}
		else
			scriptableObj = Resources.Load(instance.m_OutputScriptableObjectPath + instance.m_GenerateClassName) as ScriptableObject;
		AssetDatabase.Refresh();
		
		// InputManagerの設定情報セット
		SetInputAxesData(scriptableObj);
	}

	// 抜き出したInputManagerの設定情報を変数へセット
	private static void SetInputAxesData(ScriptableObject scriptableObj)
	{
		var instance = InputAxisDataGeneratorParams.instance;
		if (instance.m_InputAxes == null || instance.m_InputAxes.Count <= 0)
			return;

		var field = scriptableObj.GetType().GetField("Config");
		field.SetValue(scriptableObj, instance.m_InputAxes);

		// スクリプトからデータを変更した場合自動で保存されないので保存処理呼び出し
		EditorUtility.SetDirty(scriptableObj);
		AssetDatabase.SaveAssets();

		Debug.Log("Update `InputAxisData` Object Parameters"); // 一応ログ出力
	}

}

#endif