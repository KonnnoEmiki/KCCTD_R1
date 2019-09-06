using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// メッシュの面反転クラス
[RequireComponent(typeof(MeshFilter))]
public class InvertMesh : MonoBehaviour
{
    void Awake()
    {
		Invert();
		var meshCollider = gameObject.GetComponent<MeshCollider>();
		if (meshCollider == null)
			gameObject.AddComponent<MeshCollider>();
		else
			meshCollider.sharedMesh = GetComponent<MeshFilter>()?.mesh;
			
    }

	void Invert()
	{
		var filter = GetComponent<MeshFilter>();
		Mesh mesh = filter.mesh;
		mesh.triangles = mesh.triangles.Reverse().ToArray();
		var normals = mesh.normals;
		for(int i = 0; i < normals.Length; i++)
			normals[i] = normals[i] * -1;
		mesh.normals = normals;
		filter.mesh = mesh;
	}

}
