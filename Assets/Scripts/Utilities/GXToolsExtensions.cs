using UnityEngine;

namespace FZeroGXEditor.Utilities
{
	public static class VectorUtil
	{
		public static Vector3 ToUnityVector(this FZeroGXTools.Serialization.Vector3 vector)
		{
			return new Vector3(vector.x, vector.y, vector.z);
		}
	}
}
