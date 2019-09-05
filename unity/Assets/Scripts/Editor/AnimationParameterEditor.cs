#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AnimationParameter))]
public class AnimationParameterEditor : PropertyDrawer
{
	SerializedProperty m_Index;
	SerializedProperty m_Type;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		m_Index = property.FindPropertyRelative("m_Index");
		m_Type = property.FindPropertyRelative("m_Type");

		// propertyから属性を付加した変数を持つObjectを、
		// asによるキャストだとnullが返るので強制的にMonoBehaviourにキャストし、
		// Animatorコンポーネントを取得する
		var target = (MonoBehaviour)property.serializedObject.targetObject;
		var animator = target.GetComponent<Animator>();

		if (animator == null)
		{
			// Animatorコンポーネントが無い旨をラベル表示
			EditorGUI.LabelField(position,label,"Animator Component is not found");
			m_Index.intValue = -1;
			m_Type.enumValueIndex = 0;
			return;
		}

		List<string> parameterNames = new List<string>();
		for (int i = 0; i < animator.parameters.Length; ++i)
		{
			var param = animator.parameters[i];
			parameterNames.Add(param.name);
		}

		int selectIndex = parameterNames.FindIndex((e) => Animator.StringToHash(e) == m_Index.intValue);
		// Popupでステート選択
		selectIndex = EditorGUI.Popup(position, property.name,selectIndex, parameterNames.ToArray());

		if (selectIndex < 0 || selectIndex > animator.parameters.Length)
		{
			m_Index.intValue = -1;
			m_Type.enumValueIndex = 0;
			return;
		}

		m_Index.intValue = Animator.StringToHash(animator.parameters[selectIndex].name);
		var selectParam = animator.parameters[selectIndex];
		switch (selectParam.type)
		{
			case AnimatorControllerParameterType.Float:
				m_Type.enumValueIndex = (int)AnimationParameter.Type.Float;
			break;
			case AnimatorControllerParameterType.Int:
				m_Type.enumValueIndex = (int)AnimationParameter.Type.Int;
			break;
			case AnimatorControllerParameterType.Bool:
				m_Type.enumValueIndex = (int)AnimationParameter.Type.Bool;
			break;
			case AnimatorControllerParameterType.Trigger:
				m_Type.enumValueIndex = (int)AnimationParameter.Type.Trigger;
			break;

		}
	}

}
#endif