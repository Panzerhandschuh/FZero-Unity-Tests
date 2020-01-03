using FZeroGXTools.Serialization;
using UnityEngine;

namespace FZeroGXEditor.Utilities
{
	public static class GizmoUtil
	{
		public static void DrawTriangle(Triangle triangle, Color triangleColor, Color normalColor)
		{
			Gizmos.color = triangleColor;
			Gizmos.DrawLine(triangle.vertex1.ToUnityVector(), triangle.vertex2.ToUnityVector());
			Gizmos.DrawLine(triangle.vertex2.ToUnityVector(), triangle.vertex3.ToUnityVector());
			Gizmos.DrawLine(triangle.vertex3.ToUnityVector(), triangle.vertex1.ToUnityVector());

			var normalPos = (triangle.vertex1.ToUnityVector() + triangle.vertex2.ToUnityVector() +
				triangle.vertex3.ToUnityVector()) / 3;
			Gizmos.color = normalColor;
			Gizmos.DrawRay(normalPos, triangle.normal.ToUnityVector());
		}

		public static void DrawQuad(Quad quad, Color quadColor, Color normalColor)
		{
			Gizmos.color = quadColor;
			Gizmos.DrawLine(quad.vertex1.ToUnityVector(), quad.vertex2.ToUnityVector());
			Gizmos.DrawLine(quad.vertex2.ToUnityVector(), quad.vertex3.ToUnityVector());
			Gizmos.DrawLine(quad.vertex3.ToUnityVector(), quad.vertex4.ToUnityVector());
			Gizmos.DrawLine(quad.vertex4.ToUnityVector(), quad.vertex1.ToUnityVector());

			var normalPos = (quad.vertex1.ToUnityVector() + quad.vertex2.ToUnityVector() +
				quad.vertex3.ToUnityVector() + quad.vertex4.ToUnityVector()) / 4;
			Gizmos.color = normalColor;
			Gizmos.DrawRay(normalPos, quad.normal.ToUnityVector());
		}
	}
}
