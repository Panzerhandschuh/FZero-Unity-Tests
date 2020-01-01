namespace FZeroGXEditor.Serialization
{
	public class UnknownTable1 : IBinarySerializable
	{
		public int numEntries;
		public int offset;
		public UnknownEntry1[] unknownEntries;

		public void Serialize(FZWriter writer)
		{
			writer.Write(numEntries);
			writer.Write(offset);
			writer.WriteAtOffset(unknownEntries, offset);
		}

		public static UnknownTable1 Deserialize(FZReader reader)
		{
			var table = new UnknownTable1();

			table.numEntries = reader.ReadInt32();
			table.offset = reader.ReadInt32();
			table.unknownEntries = reader.ReadArrayAtOffset(table.offset, table.numEntries, UnknownEntry1.Deserialize);

			return table;
		}
	}
}
