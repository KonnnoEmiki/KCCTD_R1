using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// メッシュ拡大クラス
[RequireComponent(typeof(MeshFilter))]
public class MeshScaling : MonoBehaviour
{
	[SerializeField]
	private float m_Scale = 1;

#if UNITY_EDITOR
	[SerializeField]
	private bool m_DebugDrawNormal = true;
#endif

	private MeshFilter m_Filter = null;

	void Awake()
    {
		m_Filter = GetComponent<MeshFilter>();
		
		if (m_Filter == null) return;
		var mesh = m_Filter.mesh;
		var vertices = mesh.vertices;
		for (int i = 0; i < vertices.Length; i++)
			vertices[i] = vertices[i] * m_Scale;
		mesh.vertices = vertices;
		mesh.RecalculateBounds();

		var collider = GetComponent<MeshCollider>();
		if (collider != null)
			collider.sharedMesh = mesh;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (m_Filter == null)
			m_Filter = GetComponent<MeshFilter>();

		Mesh mesh = null;
		Vector3 scale = Vector3.one;
		if (EditorApplication.isPlaying == false)
		{
			scale *= m_Scale;
			mesh = m_Filter.sharedMesh;
		}
		else
			mesh = m_Filter.mesh;

		if (mesh == null) return;

		Gizmos.color = Color.green;
		Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation, scale);

		if (m_DebugDrawNormal == false) return;

		// 法線描画
		var normals = mesh.normals;
		var vertices = mesh.vertices;
		for (int i = 0; i < normals.Length; i++)
		{
			Vector3 pos = vertices[i];
			pos.x *= scale.x;
			pos.y *= scale.y;
			pos.z *= scale.z;
			pos += transform.position;
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(pos, Vector3.one * 0.1f);
			Gizmos.DrawLine(pos, pos + normals[i]);
		}
	}
#endif

}
