using UnityEngine;
using System;
using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class FZReader : BinaryReader
	{
		public FZReader(Stream input) : base(input)
		{
		}

		public Vector3 ReadVector()
		{
			return new Vector3(-ReadSingle(), ReadSingle(), ReadSingle());
		}

		public override int ReadInt32()
		{
			var bytes = ReadBytes(4);
			Array.Reverse(bytes); // Swap endianness
			return BitConverter.ToInt32(bytes, 0);
		}

		public override float ReadSingle()
		{
			var bytes = ReadBytes(4);
			Array.Reverse(bytes); // Swap endianness
			return BitConverter.ToSingle(bytes, 0);
		}
	}
}
