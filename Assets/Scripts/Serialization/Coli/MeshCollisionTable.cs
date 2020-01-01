using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class MeshCollisionTable : IBinarySerializable
	{
		public int numEntries;
		public int offset;
		public MeshCollisionEntry[] meshCollisionEntries;

		public void Serialize(FZWriter writer)
		{
			writer.Write(numEntries);
			writer.Write(offset);
			writer.WriteAtOffset(meshCollisionEntries, offset);
		}

		public static MeshCollisionTable Deserialize(FZReader reader)
		{
			var table = new MeshCollisionTable();

			table.numEntries = reader.ReadInt32();
			table.offset = reader.ReadInt32();
			table.meshCollisionEntries = reader.ReadArrayAtOffset(table.offset, table.numEntries, MeshCollisionEntry.Deserialize);

			return table;
		}
	}
}
