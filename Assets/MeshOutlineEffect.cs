using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeshEffect : MonoBehaviour {
	protected MeshFilter[] childFilters;
	protected MeshRenderer[] childRenderers;

	private void Start() {
		childRenderers = GetComponentsInChildren<MeshRenderer>();
		childFilters = new MeshFilter[childRenderers.Length];
		for (int mr = 0; mr < childRenderers.Length; mr++) {
			childFilters[mr] = childRenderers[mr].GetComponent<MeshFilter>();
		}
	}

	protected void DrawAllMeshes(Material material, Vector3 worldPositionOffset) {
		for (int mr = 0; mr < childRenderers.Length; mr++) {
			Graphics.DrawMesh(childFilters[mr].sharedMesh, Matrix4x4.Translate(worldPositionOffset) * childFilters[mr].transform.localToWorldMatrix, material, gameObject.layer);
		}
	}

}

[ExecuteAlways]
public class MeshOutlineEffect : MeshEffect {
	[SerializeField] Material m_OutlineMaterial;
	[SerializeField] float offsetMultiplier = 1;
	[SerializeField] Vector3[] offsets;

	private void LateUpdate() {
		for (int of = 0; of < offsets.Length; of++) {
			DrawAllMeshes(m_OutlineMaterial, offsets[of] * offsetMultiplier);
		}
	}
}
