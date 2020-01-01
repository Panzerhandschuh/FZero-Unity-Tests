namespace FZeroGXEditor.Serialization
{
	public class TriangleExtraData : IBinarySerializable
	{
		public int address;
		public int[] unknown1;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
		}

		public static TriangleExtraData Deserialize(FZReader reader)
		{
			var data = new TriangleExtraData();

			data.address = (int)reader.BaseStream.Position;
			data.unknown1 = reader.ReadInt32Array(7);

			return data;
		}
	}
}
