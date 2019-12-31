using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class CheckpointEntry : IBinarySerializable
	{
		public int unknown1;
		public int offset;
		public Checkpoint checkpoint;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
			writer.Write(offset);
			writer.Write(checkpoint);
		}

		public static CheckpointEntry Deserialize(FZReader reader)
		{
			var entry = new CheckpointEntry();

			entry.unknown1 = reader.ReadInt32();
			entry.offset = reader.ReadInt32();

			reader.BaseStream.Seek(entry.offset, SeekOrigin.Begin);
			entry.checkpoint = Checkpoint.Deserialize(reader);

			return entry;
		}
	}
}
