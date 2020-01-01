namespace FZeroGXEditor.Serialization
{
	public class UnknownEntry1 : IBinarySerializable
	{
		public int address;
		public byte[] unknown1;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
		}

		public static UnknownEntry1 Deserialize(FZReader reader)
		{
			var obj = new UnknownEntry1();

			obj.address = (int)reader.BaseStream.Position;
			obj.unknown1 = reader.ReadBytes(20);

			return obj;
		}
	}
}
