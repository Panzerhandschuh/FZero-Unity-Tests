using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class UnknownTable2 : IBinarySerializable
	{
		public int numEntries;
		public int offset;
		public UnknownEntry2[] unknownEntries;

		public void Serialize(FZWriter writer)
		{
			writer.Write(numEntries);
			writer.Write(offset);
			writer.WriteAtOffset(unknownEntries, offset);
		}

		public static UnknownTable2 Deserialize(FZReader reader)
		{
			var table = new UnknownTable2();

			table.numEntries = reader.ReadInt32();
			table.offset = reader.ReadInt32();
			table.unknownEntries = reader.ReadArrayAtOffset(table.offset, table.numEntries, UnknownEntry2.Deserialize);

			return table;
		}
	}
}
