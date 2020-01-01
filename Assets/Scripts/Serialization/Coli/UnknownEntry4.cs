using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class UnknownEntry4 : IBinarySerializable
	{
		public int address;
		public int unknown1;

		public void Serialize(FZWriter writer)
		{
			writer.Write(unknown1);
		}

		public static UnknownEntry4 Deserialize(FZReader reader)
		{
			var obj = new UnknownEntry4();

			obj.address = (int)reader.BaseStream.Position;
			obj.unknown1 = reader.ReadInt32();

			return obj;
		}
	}
}
