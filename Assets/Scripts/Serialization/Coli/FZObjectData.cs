using System;
using System.IO;
using UnityEngine;

namespace FZeroGXEditor.Serialization
{
	[Serializable]
	public class FZObjectData : IBinarySerializable
	{
		public int address;
		public int unknown1;
		public int unknown2;
		public int unknown3;
		public Vector3 position;
		public int unknown4; // Maybe an offset? Unlikely a float
		public int unknown5; // Maybe an offset? Unlikely a float
		public Vector3 scale; // Unconfirmed if this is scale
		public int unknown6; // Always 0?
		public int unknownOffset1; // Offset to unknown data (usually 0)
		public int unknownOffset2; // Offset to unknown data (usually not 0)
		public int unknown7; // Always 0?
		public int orientationOffset; // Offset to orientation info
		public FZOrientation orientation;

		public void Serialize(FZWriter writer)
		{
			writer.BaseStream.Seek(address, SeekOrigin.Begin);
			writer.Write(unknown1);
			writer.Write(unknown2);
			writer.Write(unknown3);
			writer.Write(position);
			writer.Write(unknown4);
			writer.Write(unknown5);
			writer.Write(scale);
			writer.Write(unknown6);
			writer.Write(unknownOffset1);
			writer.Write(unknownOffset2);
			writer.Write(unknown7);
			writer.Write(orientationOffset);
			if (orientationOffset != 0)
				writer.WriteAtOffset(orientation, orientationOffset);
		}

		public static FZObjectData Deserialize(FZReader reader)
		{
			var obj = new FZObjectData();

			obj.address = (int)reader.BaseStream.Position;
			obj.unknown1 = reader.ReadInt32();
			obj.unknown2 = reader.ReadInt32();
			obj.unknown3 = reader.ReadInt32();
			obj.position = reader.ReadVector3();
			obj.unknown4 = reader.ReadInt32();
			obj.unknown5 = reader.ReadInt32();
			obj.scale = reader.ReadVector3();
			obj.unknown6 = reader.ReadInt32();
			obj.unknownOffset1 = reader.ReadInt32();
			obj.unknownOffset2 = reader.ReadInt32();
			obj.unknown7 = reader.ReadInt32();
			obj.orientationOffset = reader.ReadInt32();
			if (obj.orientationOffset != 0)
				obj.orientation = reader.ReadAtOffset(obj.orientationOffset, FZOrientation.Deserialize);

			return obj;
		}
	}
}
