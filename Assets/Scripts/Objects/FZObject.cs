using FZeroGXEditor.Utilities;
using FZeroGXTools.Serialization;
using UnityEngine;

namespace FZeroGXEditor.Objects
{
	public class FZObject : MonoBehaviour
	{
		public FZObjectData data;

		public void Init(FZObjectData data)
		{
			this.data = data;

			transform.position = data.position.ToUnityVector();
			if (data.orientation != null)
				InitOrientation(data.orientation);
		}

		private void InitOrientation(FZOrientation orientation)
		{
			transform.rotation = Quaternion.LookRotation(orientation.forward.ToUnityVector(), orientation.up.ToUnityVector());
		}

		private void OnDrawGizmos()
		{
			var orientation = data.orientation;
			if (orientation != null)
				DrawOrientation(orientation);
		}

		private void DrawOrientation(FZOrientation orientation)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, orientation.right.ToUnityVector() * 3f);
			Gizmos.color = Color.green;
			Gizmos.DrawRay(transform.position, orientation.up.ToUnityVector() * 3f);
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(transform.position, orientation.forward.ToUnityVector() * 3f);
		}
	}
}
