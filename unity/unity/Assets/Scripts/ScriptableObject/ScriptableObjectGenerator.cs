#if UNITY_EDITOR

using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

// ScriptableObjectGeneratorの各種必要パラメータ
public class ScriptableObjectGeneratorParameter : ScriptableSingleton<ScriptableObjectGeneratorParameter>
{
    // クラス名
    public string m_ClassName;

	// 出力ディレクトリ
	public string m_ScriptOutputDir = ScriptableObjectGenerator.DefaultScriptDir;
	public string m_InstanceOutputDir = ScriptableObjectGenerator.DefaultInstanceDir;

    // スクリプトファイルの出力パス
    public string m_OutputScriptFilePath;

}

// ScriptableObject動的生成ウィンドウクラス
public class ScriptableObjectGenerator : EditorWindow
{
	public static string DefaultScriptDir = "Assets/Scripts/ScriptableObject";
	public static string DefaultInstanceDir = "Assets/Resources/ScriptableObjects";

    [MenuItem("Assets/Create/Scriptable Object/Generate")]
    static void OpenScriptableObjectGeneratorWindow()
    {
        GetWindow<ScriptableObjectGenerator>();
    }

    void OnGUI()
    {
        // コンパイル中は無効化
        EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);

        var instance = ScriptableObjectGeneratorParameter.instance;

        instance.m_ClassName = EditorGUILayout.TextField("Name", instance.m_ClassName);
        instance.m_ScriptOutputDir = EditorGUILayout.TextField("Script Output Directory", instance.m_ScriptOutputDir);
        instance.m_InstanceOutputDir = EditorGUILayout.TextField("Instance Output Directory", instance.m_InstanceOutputDir);

        var ClassName = string.IsNullOrEmpty(instance.m_ClassName);

        // クラス名が未指定だったらボタン無効化
        EditorGUI.BeginDisabledGroup(ClassName);
        
        if (GUILayout.Button("Create"))
        {
            bool ret = CreateScriptableObject(instance.m_ClassName);
            
            // 正常に生成出来ればウィンドウを閉じる
            if (ret) Close();
        }

        EditorGUI.EndDisabledGroup();
		
        EditorGUI.EndDisabledGroup();
    }

    public bool CreateScriptableObject(string name)
    {
        var instance = ScriptableObjectGeneratorParameter.instance;
        
        ScriptableObject scriptableObject = null;
        bool isCreateInstance = false;

		// ディレクトリ指定がなければデフォルトのディレクトリに設定
		if (string.IsNullOrEmpty(instance.m_ScriptOutputDir))
            instance.m_ScriptOutputDir = DefaultScriptDir;
        if (string.IsNullOrEmpty(instance.m_InstanceOutputDir))
            instance.m_InstanceOutputDir = DefaultInstanceDir;

        CreateFolder(instance.m_ScriptOutputDir);
        CreateFolder(instance.m_InstanceOutputDir);

        instance.m_OutputScriptFilePath = instance.m_ScriptOutputDir + "/" + name + ".cs";

        // 同じ名前のクラスが存在するか
        var findScript = File.Exists(instance.m_OutputScriptFilePath);

        if (findScript == false)
        {
			// 存在しなければScriptableObjectのScript生成
			string script = ScriptableObjectDefines.TemplateScript.Replace("#name#", name);
            File.WriteAllText(instance.m_OutputScriptFilePath, script, Encoding.UTF8);
            AssetDatabase.Refresh();
            return false;
        }
        else
        {
            object o = Activator.CreateInstance(Type.GetType(name));
            scriptableObject = o as ScriptableObject;
            if (scriptableObject != null)
            {
                // インスタンス生成の確認ダイアログ表示
                isCreateInstance = EditorUtility.DisplayDialog
                (
                    "ScriptableObjectの生成確認",
                    "すでに" + name + ".cs" + "は存在しています。" + Environment.NewLine +
                    "インスタンスを生成しますか？",
                    "はい",
                    "いいえ"
                );

                // インスタンスを生成しないなら
                if (isCreateInstance == false)
                    return false;
            }
            else
            {
                // ダイアログ表示
                EditorUtility.DisplayDialog
                (
                    "ScriptableObjectの生成確認",
                    "すでに" + name + ".cs" + "は存在しています。",
                    "確認"
                );
            }
        }

        CreateScriptableObject(scriptableObject);
        
        return isCreateInstance;   
    }

    private static void CreateFolder(string path)
    {
        string target = "";
        char[] splitChars = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
        foreach (var dir in path.Split(splitChars))
        {
            // 一階層ごとにフォルダが存在するか確認
            var parent = target;
            target = Path.Combine(target, dir);
            if (AssetDatabase.IsValidFolder(target) == false) 
                AssetDatabase.CreateFolder(parent, dir);    // なければ作成
        }
    }

    [DidReloadScripts]
    static void OnComplied()
    {
        var instance = ScriptableObjectGeneratorParameter.instance;

        if (File.Exists(instance.m_OutputScriptFilePath) == false)
            return;

        // コードのコンパイルが完了したらScriptableObject生成

        object o = Activator.CreateInstance(Type.GetType(instance.m_ClassName));

        ScriptableObject scriptableObject = o as ScriptableObject;
        if (scriptableObject == null)
        {
            Debug.Log("error");
            return;
        }

        CreateScriptableObject(scriptableObject);
    }

    private static void CreateScriptableObject(ScriptableObject scriptableObj)
    {
        var instance = ScriptableObjectGeneratorParameter.instance;

        string assetPath = instance.m_InstanceOutputDir + "/" + instance.m_ClassName + ".asset";
		if (File.Exists(assetPath)) return; // すでに存在している
        string uniquePath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
        AssetDatabase.CreateAsset(scriptableObj, uniquePath);
        AssetDatabase.Refresh();
        Selection.activeObject = scriptableObj;
    }


}

#endif