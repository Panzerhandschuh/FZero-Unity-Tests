using System;
using UnityEngine;

namespace FZeroGXEditor.Serialization
{
	[Serializable]
	public class Quad : IBinarySerializable
	{
		public int address;
		public int unknown1;
		public Vector3 normal;
		public Vector3 vertex1;
		public Vector3 vertex2;
		public Vector3 vertex3;
		public Vector3 vertex4;
		public byte[] unknown2;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
			writer.Write(normal);
			writer.Write(vertex1);
			writer.Write(vertex2);
			writer.Write(vertex3);
			writer.Write(vertex4);
			writer.Write(unknown2);
		}

		public static Quad Deserialize(FZReader reader)
		{
			var obj = new Quad();

			obj.address = (int)reader.BaseStream.Position;
			obj.unknown1 = reader.ReadInt32();
			obj.normal = reader.ReadVector3();
			obj.vertex1 = reader.ReadVector3();
			obj.vertex2 = reader.ReadVector3();
			obj.vertex3 = reader.ReadVector3();
			obj.vertex4 = reader.ReadVector3();
			obj.unknown2 = reader.ReadBytes(48);

			return obj;
		}
	}
}
