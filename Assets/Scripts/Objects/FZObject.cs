using FZeroGXEditor.Serialization;
using UnityEngine;

namespace FZeroGXEditor.Objects
{
	public class FZObject : MonoBehaviour
	{
		public FZObjectData data;

		public void Init(FZObjectData data)
		{
			this.data = data;
		}
	}
}
