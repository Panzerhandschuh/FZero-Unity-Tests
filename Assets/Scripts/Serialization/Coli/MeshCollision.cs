namespace FZeroGXEditor.Serialization
{
	public class MeshCollision : IBinarySerializable
	{
		public byte[] unknown1;
		public int numTriangles;
		public int numQuads;
		public int trianglesOffset;
		public int quadsOffset;

		public void Serialize(FZWriter writer)
		{
			throw new System.NotImplementedException();
		}

		public static MeshCollision Deserialize(FZReader reader)
		{
			var obj = new MeshCollision();

			obj.unknown1 = reader.ReadBytes(20);
			obj.numTriangles = BinarySerializer.ReadInt32(reader);
			obj.numQuads = BinarySerializer.ReadInt32(reader);
			obj.trianglesOffset = BinarySerializer.ReadInt32(reader);
			obj.quadsOffset = BinarySerializer.ReadInt32(reader);
			ReadTriangles(reader, numTriangles, trianglesOffset, Color.red);
			ReadQuads(reader, numQuads, quadOffset, Color.red);

			return obj;
		}
	}
}
