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
			
		}

		public static MeshCollisionEntry Deserialize(FZReader reader)
		{
			var entry = new MeshCollisionEntry();

			entry.unknown1 = reader.ReadBytes(12);
			entry.offset = reader.ReadInt32();

			if (entry.offset != 0)
			{
				reader.BaseStream.Seek(entry.offset, SeekOrigin.Begin);
				entry.meshCollision = MeshCollision.Deserialize(reader);
			}

			return entry;
		}
	}
}
