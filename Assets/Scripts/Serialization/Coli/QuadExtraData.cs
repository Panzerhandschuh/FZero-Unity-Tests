namespace FZeroGXEditor.Serialization
{
	public class QuadExtraData : IBinarySerializable
	{
		public int address;
		public int[] unknown1;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
		}

		public static QuadExtraData Deserialize(FZReader reader)
		{
			var data = new QuadExtraData();

			data.address = (int)reader.BaseStream.Position;
			data.unknown1 = reader.ReadInt32Array(6);

			return data;
		}
	}
}
