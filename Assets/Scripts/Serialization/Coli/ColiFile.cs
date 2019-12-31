namespace FZeroGXEditor.Serialization
{
	public class ColiFile : IBinarySerializable
	{
		public byte[] header; // 8 bytes

		public void Serialize(FZWriter writer)
		{
			writer.Write(header);
		}

		public static ColiFile Deserialize(FZReader reader)
		{
			var file = new ColiFile();

			file.header = reader.ReadBytes(8);

			return file;
		}
	}
}
