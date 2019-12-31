using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class CheckpointTable : IBinarySerializable
	{
		public int numEntries;
		public int offset;
		public CheckpointEntry[] checkpointEntries;

		private const int checkpointEntrySize = 12;

		public void Serialize(FZWriter writer)
		{
			writer.Write(numEntries);
			writer.Write(offset);
			foreach (var entry in checkpointEntries)
				writer.Write(entry);
		}

		public static CheckpointTable Deserialize(FZReader reader)
		{
			var table = new CheckpointTable();

			table.numEntries = reader.ReadInt32();
			table.offset = reader.ReadInt32();

			table.checkpointEntries = new CheckpointEntry[table.numEntries];
			for (var i = 0; i < table.numEntries; i++)
			{
				reader.BaseStream.Seek(table.offset + (checkpointEntrySize * i), SeekOrigin.Begin); // Go to the checkpoint list
				table.checkpointEntries[i] = CheckpointEntry.Deserialize(reader);
			}

			return table;
		}
	}
}
