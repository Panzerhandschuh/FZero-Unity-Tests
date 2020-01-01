using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class UnknownEntry3 : IBinarySerializable
	{
		public int address;
		public int unknown1;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
		}

		public static UnknownEntry3 Deserialize(FZReader reader)
		{
			var obj = new UnknownEntry3();

			obj.address = (int)reader.BaseStream.Position;
			obj.unknown1 = reader.ReadInt32();

			return obj;
		}
	}
}
