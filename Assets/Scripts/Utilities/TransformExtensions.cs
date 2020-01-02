using UnityEngine;

namespace FZeroGXEditor.Utilities
{
	public static class TransformExtensions
	{
		public static void DestroyChildren(this Transform transform)
		{
			for (var i = transform.childCount - 1; i >= 0; i--) // Must do a reverse loop to properly delete children
			{
				var child = transform.GetChild(i);
				Object.DestroyImmediate(child.gameObject);
			}
		}
	}
}
