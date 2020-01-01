namespace FZeroGXEditor.Serialization
{
	public class ColiFile : IBinarySerializable
	{
		public byte[] header; // 8 bytes
		public CheckpointTable checkpointTable;
		public UnknownTable1 unknownTable1;
		public ObjectCollisionTable objectCollisionTable;
		public int[] unknown1;
		public ObjectTable objectTable;
		public int unknown2;
		public int[] unknown3;
		public MeshCollisionTable meshCollisionTable;
		public UnknownTable2 unknownTable2;
		public int[] unknown4;
		public int unknownOffset1;
		public int[] unknown5;
		public int unknownOffset2; // Points to end of checkpoint data
		public UnknownTable3 unknownTable3;
		public UnknownTable4 unknownTable4;
		public int[] unknown6;

		public void Serialize(FZWriter writer)
		{
			writer.Write(header);
			throw new System.NotImplementedException();
		}

		public static ColiFile Deserialize(FZReader reader)
		{
			var file = new ColiFile();

			file.header = reader.ReadBytes(8);
			file.checkpointTable = CheckpointTable.Deserialize(reader);
			file.unknownTable1 = UnknownTable1.Deserialize(reader);
			file.objectCollisionTable = ObjectCollisionTable.Deserialize(reader);
			file.unknown1 = reader.ReadInt32Array(12);
			file.objectTable = ObjectTable.Deserialize(reader);
			file.unknown2 = reader.ReadInt32();
			file.unknown3 = reader.ReadInt32Array(2);
			file.meshCollisionTable = MeshCollisionTable.Deserialize(reader);
			file.unknownTable2 = UnknownTable2.Deserialize(reader);
			file.unknown4 = reader.ReadInt32Array(4);
			file.unknownOffset1 = reader.ReadInt32();
			file.unknown5 = reader.ReadInt32Array(2);
			file.unknownOffset2 = reader.ReadInt32();
			file.unknownTable3 = UnknownTable3.Deserialize(reader);
			file.unknownTable4 = UnknownTable4.Deserialize(reader);
			file.unknown6 = reader.ReadInt32Array(23);

			return file;
		}
	}
}
