﻿using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class MeshCollision : IBinarySerializable
	{
		public byte[] unknown1;
		public int numTriangles;
		public int numQuads;
		public int trianglesOffset;
		public int quadsOffset;
		public Triangle[] triangles;
		public Quad[] quads;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
			writer.Write(numTriangles);
			writer.Write(numQuads);
			writer.Write(trianglesOffset);
			writer.Write(quadsOffset);
			writer.WriteAtOffset(triangles, trianglesOffset);
			writer.WriteAtOffset(quads, quadsOffset);
		}

		public static MeshCollision Deserialize(FZReader reader)
		{
			var obj = new MeshCollision();

			obj.unknown1 = reader.ReadBytes(20);
			obj.numTriangles = BinarySerializer.ReadInt32(reader);
			obj.numQuads = BinarySerializer.ReadInt32(reader);
			obj.trianglesOffset = BinarySerializer.ReadInt32(reader);
			obj.quadsOffset = BinarySerializer.ReadInt32(reader);
			obj.triangles = reader.ReadArrayAtOffset(obj.trianglesOffset, obj.numTriangles, Triangle.Deserialize);
			obj.quads = reader.ReadArrayAtOffset(obj.quadsOffset, obj.numQuads, Quad.Deserialize);

			return obj;
		}
	}
}