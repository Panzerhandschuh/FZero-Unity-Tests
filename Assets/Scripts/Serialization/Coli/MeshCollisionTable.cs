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
			foreach (var entry in meshCollisionEntries)
				writer.Write(entry);
		}

		public static MeshCollisionTable Deserialize(FZReader reader)
		{
			var table = new MeshCollisionTable();

			table.numEntries = reader.ReadInt32();
			table.offset = reader.ReadInt32();

			table.meshCollisionEntries = new MeshCollisionEntry[table.numEntries];
			reader.BaseStream.Seek(table.offset, SeekOrigin.Begin); // Go to the mesh collisions table
			for (var i = 0; i < table.numEntries; i++) // Each table entry is 16 bytes
				table.meshCollisionEntries[i] = MeshCollisionEntry.Deserialize(reader);

			return table;
		}
	}
}
