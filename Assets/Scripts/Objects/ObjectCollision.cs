using FZeroGXTools.Serialization;
using FZeroGXEditor.Utilities;
using UnityEngine;

namespace FZeroGXEditor.Objects
{
	public class ObjectCollision : MonoBehaviour
	{
		public ObjectCollisionData data;

		public void Init(ObjectCollisionData data)
		{
			this.data = data;
		}

		private void OnDrawGizmos()
		{
			foreach (var triangle in data.triangles)
				GizmoUtil.DrawTriangle(triangle, new Color(1f, 0.5f, 0f), Color.blue);

			foreach (var quad in data.quads)
				GizmoUtil.DrawQuad(quad, new Color(1f, 0.25f, 0f), Color.blue);
		}
	}
}
