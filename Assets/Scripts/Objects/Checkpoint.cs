using FZeroGXEditor.Utilities;
using FZeroGXTools.Serialization;
using UnityEngine;

namespace FZeroGXEditor.Objects
{
	public class Checkpoint : MonoBehaviour
	{
		public Transform start;
		public Transform end;
		public CheckpointData data;

		public void Init(CheckpointData data)
		{
			this.data = data;

			start.position = data.start.ToUnityVector();
			end.position = data.end.ToUnityVector();
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(data.start.ToUnityVector(), data.end.ToUnityVector());

			Gizmos.color = Color.white;
			Gizmos.DrawRay(data.start.ToUnityVector(), data.startTangent.ToUnityVector() * 2f);
			Gizmos.DrawRay(data.end.ToUnityVector(), data.endTangent.ToUnityVector() * 2f);
		}
	}
}
