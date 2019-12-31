using UnityEngine;

namespace FZeroGXEditor.Serialization
{
	// Orientation info associated with all FZObjects
	public class FZOrientation : IBinarySerializable
	{
		public int address;
		public Vector3 right; // Forward and right vectors might be switched (unconfirmed)
		public float positionX; // X position of the object/orientation vectors (seems redundant since the object already has this info)
		public Vector3 up;
		public float positionY; // Y position of the object/orientation vectors
		public Vector3 forward;
		public float positionZ; // Z position of the object/orientation vectors

		public void Serialize(FZWriter writer)
		{
			BinarySerializer.Write(writer, right);
			BinarySerializer.Write(writer, -positionX);
			BinarySerializer.Write(writer, up);
			BinarySerializer.Write(writer, positionY);
			BinarySerializer.Write(writer, forward);
			BinarySerializer.Write(writer, positionZ);
		}

		public static FZOrientation Deserialize(FZReader reader)
		{
			var obj = new FZOrientation();

			obj.address = (int)reader.BaseStream.Position;
			obj.right = BinarySerializer.ReadVector(reader);
			obj.positionX = -BinarySerializer.ReadSingle(reader);
			obj.up = BinarySerializer.ReadVector(reader);
			obj.positionY = BinarySerializer.ReadSingle(reader);
			obj.forward = BinarySerializer.ReadVector(reader);
			obj.positionZ = BinarySerializer.ReadSingle(reader);

			return obj;
		}
	}
}
