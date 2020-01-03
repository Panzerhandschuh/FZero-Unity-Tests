using FZeroGXTools.Serialization;
using FZeroGXEditor.Utilities;
using UnityEngine;

namespace FZeroGXEditor.Objects
{
	public class MeshCollision : MonoBehaviour
	{
		public MeshCollisionData data;

		public void Init(MeshCollisionData data)
		{
			this.data = data;
		}

		private void OnDrawGizmos()
		{
			foreach (var triangle in data.triangles)
				GizmoUtil.DrawTriangle(triangle, Color.red, Color.blue);

			foreach (var quad in data.quads)
				GizmoUtil.DrawQuad(quad, Color.red, Color.blue);
		}
	}
}
