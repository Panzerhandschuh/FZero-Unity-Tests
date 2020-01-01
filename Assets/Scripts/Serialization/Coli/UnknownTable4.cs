using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class UnknownTable4 : IBinarySerializable
	{
		public int numEntries;
		public int offset;
		public UnknownEntry4[] unknownEntries;

		public void Serialize(FZWriter writer)
		{
			writer.Write(numEntries);
			writer.Write(offset);
			writer.WriteAtOffset(unknownEntries, offset);
		}

		public static UnknownTable4 Deserialize(FZReader reader)
		{
			var table = new UnknownTable4();

			table.numEntries = reader.ReadInt32();
			table.offset = reader.ReadInt32();
			table.unknownEntries = reader.ReadArrayAtOffset(table.offset, table.numEntries, UnknownEntry4.Deserialize);

			return table;
		}
	}
}
