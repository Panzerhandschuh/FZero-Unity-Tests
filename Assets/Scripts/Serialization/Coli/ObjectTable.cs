namespace FZeroGXEditor.Serialization
{
	public class ObjectTable : IBinarySerializable
	{
		public int numEntries;
		public int offset;
		public FZObject[] objects;

		public void Serialize(FZWriter writer)
		{
			writer.Write(numEntries);
			writer.Write(offset);
			writer.WriteAtOffset(objects, offset);
		}

		public static ObjectTable Deserialize(FZReader reader)
		{
			var table = new ObjectTable();

			table.numEntries = reader.ReadInt32();
			table.offset = reader.ReadInt32();
			table.objects = reader.ReadArrayAtOffset(table.offset, table.numEntries, FZObject.Deserialize);

			return table;
		}
	}
}
