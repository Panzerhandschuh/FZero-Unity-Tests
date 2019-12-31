using UnityEngine;
using System;
using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class BinarySerializer
	{
		public static Vector3 ReadVector(BinaryReader reader)
		{
			return new Vector3(-ReadSingle(reader), ReadSingle(reader), ReadSingle(reader));
		}

		public static int ReadInt32(BinaryReader reader)
		{
			var bytes = reader.ReadBytes(4);
			Array.Reverse(bytes); // Swap endianness
			return BitConverter.ToInt32(bytes, 0);
		}

		public static float ReadSingle(BinaryReader reader)
		{
			var bytes = reader.ReadBytes(4);
			Array.Reverse(bytes); // Swap endianness
			return BitConverter.ToSingle(bytes, 0);
		}

		public static void Write(BinaryWriter writer, Vector3 value)
		{
			Write(writer, -value.x);
			Write(writer, value.y);
			Write(writer, value.z);
		}

		public static void Write(BinaryWriter writer, int value)
		{
			var bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			writer.Write(bytes);
		}

		public static void Write(BinaryWriter writer, float value)
		{
			var bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			writer.Write(bytes);
		}
	}
}
