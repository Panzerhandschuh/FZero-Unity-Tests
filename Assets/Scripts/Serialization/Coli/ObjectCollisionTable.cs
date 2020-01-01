namespace FZeroGXEditor.Serialization
{
	public class ObjectCollisionTable : IBinarySerializable
	{
		public int numEntries;
		public int offset;
		public ObjectCollision[] objectCollisions;

		public void Serialize(FZWriter writer)
		{
			writer.Write(numEntries);
			writer.Write(offset);
			writer.WriteAtOffset(objectCollisions, offset);
		}

		public static ObjectCollisionTable Deserialize(FZReader reader)
		{
			var table = new ObjectCollisionTable();

			table.numEntries = reader.ReadInt32();
			table.offset = reader.ReadInt32();
			table.objectCollisions = reader.ReadArrayAtOffset(table.offset, table.numEntries, ObjectCollision.Deserialize);

			return table;
		}
	}
}
