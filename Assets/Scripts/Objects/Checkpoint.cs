using FZeroGXEditor.Serialization;
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

			start.position = data.start;
			end.position = data.end;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(data.start, data.end);

			Gizmos.color = Color.white;
			Gizmos.DrawRay(data.start, data.startTangent * 2f);
			Gizmos.DrawRay(data.end, data.endTangent * 2f);
		}
	}
}
