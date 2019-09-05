using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// システム全体でのenum,定数等定義用クラス
public static class SystemDefines
{
	[System.Serializable]
	public enum AxisType
	{
		KeyorMouseButton = 0,
		MouseMovement,
		JoystickAxis
	}

	[System.Serializable]
	public class InputAxis
	{
		public string Name;
		public string DescriptiveName;
		public string DescriptiveNegativeName;
		public string NegativeButton;
		public string PositiveButton;
		public string AltNegativeButton;
		public string AltPositiveButton;
		public float Gravity;
		public float Dead;
		public float Sensitivity;
		public bool Snap;
		public bool Invert;
		public AxisType Type;
		public int Axis;
		public int JoyNum;

		public static bool operator ==(InputAxis l,InputAxis r)
		{
			if (l.Name != r.Name) return false;
			if (l.DescriptiveName != r.DescriptiveName) return false;
			if (l.DescriptiveNegativeName != r.DescriptiveNegativeName) return false;
			if (l.NegativeButton != r.NegativeButton) return false;
			if (l.PositiveButton != r.PositiveButton) return false;
			if (l.AltNegativeButton != r.AltNegativeButton) return false;
			if (l.AltPositiveButton != r.AltPositiveButton) return false;
			if (l.Gravity != r.Gravity) return false;
			if (l.Dead != r.Dead) return false;
			if (l.Sensitivity != r.Sensitivity) return false;
			if (l.Snap != r.Snap) return false;
			if (l.Invert != r.Invert) return false;
			if (l.Type != r.Type) return false;
			if (l.Axis != r.Axis) return false;
			if (l.JoyNum != r.JoyNum) return false;
			
			return true;
		}

		public static bool operator !=(InputAxis l,InputAxis r)
		{
			return !(l == r);
		}
	}
}
