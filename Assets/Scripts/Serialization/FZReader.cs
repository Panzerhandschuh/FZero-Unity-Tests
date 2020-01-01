using UnityEngine;
using System;
using System.IO;
using System.Text;

namespace FZeroGXEditor.Serialization
{
	public class FZReader : BinaryReader
	{
		public FZReader(Stream input) : base(input)
		{
		}

		public FZReader(Stream input, Encoding encoding) : base(input, encoding)
		{
		}

		public FZReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
		{
		}

		public Vector3 ReadVector3()
		{
			return new Vector3(-ReadSingle(), ReadSingle(), ReadSingle());
		}

		public int[] ReadInt32Array(int count)
		{
			var array = new int[count];
			
			for (var i = 0; i < count; i++)
				array[i] = ReadInt32();

			return array;
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

		public T ReadAtOffset<T>(int offset, Func<FZReader, T> readFunc) where T : IBinarySerializable
		{
			var returnAddress = BaseStream.Position;
			BaseStream.Seek(offset, SeekOrigin.Begin);

			var obj = readFunc(this);

			BaseStream.Seek(returnAddress, SeekOrigin.Begin);

			return obj;
		}

		public T[] ReadArrayAtOffset<T>(int offset, int count, Func<FZReader, T> readFunc) where T : IBinarySerializable
		{
			var returnAddress = BaseStream.Position;
			BaseStream.Seek(offset, SeekOrigin.Begin);

			var array = new T[count];
			for (var i = 0; i < count; i++)
				array[i] = readFunc(this);

			BaseStream.Seek(returnAddress, SeekOrigin.Begin);

			return array;
		}
	}
}
