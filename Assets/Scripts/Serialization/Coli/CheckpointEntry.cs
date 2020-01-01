namespace FZeroGXEditor.Serialization
{
	public class CheckpointEntry : IBinarySerializable
	{
		public int address;
		public int unknown1;
		public int offset;
		public Checkpoint checkpoint;
		public int unknownOffset;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
			writer.Write(offset);
			writer.WriteAtOffset(checkpoint, offset);
			writer.Write(unknownOffset);
		}

		public static CheckpointEntry Deserialize(FZReader reader)
		{
			var entry = new CheckpointEntry();

			entry.address = (int)reader.BaseStream.Position;
			entry.unknown1 = reader.ReadInt32();
			entry.offset = reader.ReadInt32();
			entry.checkpoint = reader.ReadAtOffset(entry.offset, Checkpoint.Deserialize);
			entry.unknownOffset = reader.ReadInt32();

			return entry;
		}
	}
}
