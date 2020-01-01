using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class MeshCollisionEntry : IBinarySerializable
	{
		public byte[] unknown1;
		public int offset;
		public MeshCollision meshCollision;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
			writer.Write(offset);
			if (offset != 0)
				writer.WriteAtOffset(meshCollision, offset);
		}

		public static MeshCollisionEntry Deserialize(FZReader reader)
		{
			var entry = new MeshCollisionEntry();

			entry.unknown1 = reader.ReadBytes(12);
			entry.offset = reader.ReadInt32();
			if (entry.offset != 0)
				entry.meshCollision = reader.ReadAtOffset(entry.offset, MeshCollision.Deserialize);

			return entry;
		}
	}
}
