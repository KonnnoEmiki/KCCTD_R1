using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アニメーションパラメータ設定用
[System.Serializable]
public class AnimationParameter
{
	public enum Type
	{
		None = 0,
		Float,
		Int,
		Bool,
		Trigger
	}

	public int m_Index;
	public Type m_Type;
}
