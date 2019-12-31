using System;
using System.IO;
using UnityEngine;

namespace FZeroGXEditor.Serialization
{
	public class FZWriter : BinaryWriter
	{
		public void Write(Vector3 value)
		{
			Write(-value.x);
			Write(value.y);
			Write(value.z);
		}

		public override void Write(int value)
		{
			var bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			Write(bytes);
		}

		public override void Write(float value)
		{
			var bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			Write(bytes);
		}

		public void Write(IBinarySerializable serializable)
		{
			serializable.Serialize(this);
		}
	}
}
