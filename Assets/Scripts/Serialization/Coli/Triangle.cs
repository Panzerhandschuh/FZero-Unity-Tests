using UnityEngine;

namespace FZeroGXEditor.Serialization
{
	public class Triangle : IBinarySerializable
	{
		public int address;
		public int unknown1;
		public Vector3 normal;
		public Vector3 vertex1;
		public Vector3 vertex2;
		public Vector3 vertex3;
		public byte[] unknown2;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
			writer.Write(normal);
			writer.Write(vertex1);
			writer.Write(vertex2);
			writer.Write(vertex3);
			writer.Write(unknown2);
		}

		public static Triangle Deserialize(FZReader reader)
		{
			var obj = new Triangle();

			obj.address = (int)reader.BaseStream.Position;
			obj.unknown1 = reader.ReadInt32();
			obj.normal = reader.ReadVector3();
			obj.vertex1 = reader.ReadVector3();
			obj.vertex2 = reader.ReadVector3();
			obj.vertex3 = reader.ReadVector3();
			obj.unknown2 = reader.ReadBytes(36);

			return obj;
		}
	}
}
