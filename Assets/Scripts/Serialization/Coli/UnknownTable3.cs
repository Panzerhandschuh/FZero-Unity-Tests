using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class UnknownTable3 : IBinarySerializable
	{
		public int numEntries;
		public int offset;
		public UnknownEntry3[] unknownEntries;

		public void Serialize(FZWriter writer)
		{
			writer.Write(numEntries);
			writer.Write(offset);
			writer.WriteAtOffset(unknownEntries, offset);
		}

		public static UnknownTable3 Deserialize(FZReader reader)
		{
			var table = new UnknownTable3();

			table.numEntries = reader.ReadInt32();
			table.offset = reader.ReadInt32();
			table.unknownEntries = reader.ReadArrayAtOffset(table.offset, table.numEntries, UnknownEntry3.Deserialize);

			return table;
		}
	}
}
