using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class CheckpointTable : IBinarySerializable
	{
		public int numEntries;
		public int offset;
		public CheckpointEntry[] checkpointEntries;

		public void Serialize(FZWriter writer)
		{
			writer.Write(numEntries);
			writer.Write(offset);
			writer.WriteAtOffset(checkpointEntries, offset);
		}

		public static CheckpointTable Deserialize(FZReader reader)
		{
			var table = new CheckpointTable();

			table.numEntries = reader.ReadInt32();
			table.offset = reader.ReadInt32();
			table.checkpointEntries = reader.ReadArrayAtOffset(table.offset, table.numEntries, CheckpointEntry.Deserialize);

			return table;
		}
	}
}
