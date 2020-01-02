using FZeroGXEditor.Serialization;
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
				DrawTriangle(triangle);

			foreach (var quad in data.quads)
				DrawQuad(quad);
		}

		private void DrawTriangle(Triangle triangle)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(triangle.vertex1, triangle.vertex2);
			Gizmos.DrawLine(triangle.vertex2, triangle.vertex3);
			Gizmos.DrawLine(triangle.vertex3, triangle.vertex1);

			var normalPos = (triangle.vertex1 + triangle.vertex2 + triangle.vertex3) / 3;
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(normalPos, triangle.normal);
		}

		private void DrawQuad(Quad quad)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(quad.vertex1, quad.vertex2);
			Gizmos.DrawLine(quad.vertex2, quad.vertex3);
			Gizmos.DrawLine(quad.vertex3, quad.vertex4);
			Gizmos.DrawLine(quad.vertex4, quad.vertex1);

			var normalPos = (quad.vertex1 + quad.vertex2 + quad.vertex3 + quad.vertex4) / 4;
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(normalPos, quad.normal);
		}
	}
}
