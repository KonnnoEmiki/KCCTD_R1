using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 数学便利クラス
public static class MathUtil
{
	public static float GetTargetAngle(Transform trans,Vector3 targetPos)
	{
		Vector3 forword = trans.forward;
		Vector3 targetDir = trans.position - targetPos;
		return Vector3.Angle(forword, targetDir);
	}

	public static void Billboard(Transform trans,Vector3 camPos,bool rotateX = true, bool rotateY = true)
	{
		if (!rotateX) camPos.y = trans.position.y;
		if (!rotateY) camPos.x = trans.position.x;

		trans.LookAt(camPos);
	}

	public static void BillboardMainCam(Transform trans, bool rotateX = true, bool rotateY = true)
	{
		Billboard(trans, Camera.main.transform.position, rotateX, rotateY);
	}
	
	public static Vector3 RandomRange(float min,float max)
	{
		float x = Random.Range(min,max);
		float y = Random.Range(min,max);
		float z = Random.Range(min,max);
		return new Vector3(x, y, z);
	}
}