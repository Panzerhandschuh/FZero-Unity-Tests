using FZeroGXEditor.Serialization;
using UnityEngine;

namespace FZeroGXEditor.Utilities
{
	public static class GizmoUtil
	{
		public static void DrawTriangle(Triangle triangle, Color triangleColor, Color normalColor)
		{
			Gizmos.color = triangleColor;
			Gizmos.DrawLine(triangle.vertex1, triangle.vertex2);
			Gizmos.DrawLine(triangle.vertex2, triangle.vertex3);
			Gizmos.DrawLine(triangle.vertex3, triangle.vertex1);

			var normalPos = (triangle.vertex1 + triangle.vertex2 + triangle.vertex3) / 3;
			Gizmos.color = normalColor;
			Gizmos.DrawRay(normalPos, triangle.normal);
		}

		public static void DrawQuad(Quad quad, Color quadColor, Color normalColor)
		{
			Gizmos.color = quadColor;
			Gizmos.DrawLine(quad.vertex1, quad.vertex2);
			Gizmos.DrawLine(quad.vertex2, quad.vertex3);
			Gizmos.DrawLine(quad.vertex3, quad.vertex4);
			Gizmos.DrawLine(quad.vertex4, quad.vertex1);

			var normalPos = (quad.vertex1 + quad.vertex2 + quad.vertex3 + quad.vertex4) / 4;
			Gizmos.color = normalColor;
			Gizmos.DrawRay(normalPos, quad.normal);
		}
	}
}
