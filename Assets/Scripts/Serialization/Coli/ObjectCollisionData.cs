using System;
using System.IO;

namespace FZeroGXEditor.Serialization
{
	[Serializable]
	public class ObjectCollisionData : IBinarySerializable
	{
		public int address;
		public byte[] unknown1;
		public int startTriangleOffset;
		public int nextTriangleOffset;
		public Triangle[] triangles;
		public TriangleExtraData triangleExtraData;
		//public byte[] unknown2;
		public int startQuadOffset;
		public int nextQuadOffset;
		public Quad[] quads;
		public QuadExtraData quadExtraData;

		private const int triangleSize = 88;
		private const int triangleExtraDataSize = 28; // Extra data after triangle list (unknown data)
		private const int quadSize = 112;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
			writer.Write(startTriangleOffset);
			writer.Write(nextTriangleOffset);
			writer.WriteAtOffset(triangles, startTriangleOffset);
			writer.Write(startQuadOffset);
			writer.Write(nextQuadOffset);
			writer.WriteAtOffset(quads, startQuadOffset);
		}

		public static ObjectCollisionData Deserialize(FZReader reader)
		{
			var table = new ObjectCollisionData();

			table.address = (int)reader.BaseStream.Position;
			table.unknown1 = reader.ReadBytes(36);
			table.startTriangleOffset = reader.ReadInt32();
			table.nextTriangleOffset = reader.ReadInt32();

			var numTriangles = (table.nextTriangleOffset - table.startTriangleOffset - triangleExtraDataSize) / triangleSize;
			table.triangles = new Triangle[numTriangles];
			reader.BaseStream.Seek(table.startTriangleOffset, SeekOrigin.Begin);
			for (var i = 0; i < numTriangles; i++)
				table.triangles[i] = Triangle.Deserialize(reader);

			table.triangleExtraData = TriangleExtraData.Deserialize(reader);

			var quadOffset = 36 + 60 + 24; // 36 null bytes, 60 offset bytes for triangle mesh section, and 24 bytes of unknown data to get to the offset for the first quad mesh
			reader.BaseStream.Seek(table.address + quadOffset, SeekOrigin.Begin); // Go to start of extra mesh table
			table.startQuadOffset = BinarySerializer.ReadInt32(reader);
			table.nextQuadOffset = BinarySerializer.ReadInt32(reader);

			var numQuads = (table.nextQuadOffset - table.startQuadOffset - 24) / quadSize;
			table.quads = new Quad[numQuads];
			reader.BaseStream.Seek(table.startQuadOffset, SeekOrigin.Begin);
			for (var i = 0; i < numQuads; i++)
				table.quads[i] = Quad.Deserialize(reader);

			table.quadExtraData = QuadExtraData.Deserialize(reader);

			return table;
		}
	}
}
