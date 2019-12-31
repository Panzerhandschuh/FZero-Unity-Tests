using UnityEngine;
using System.IO;

namespace FZeroGXEditor.Serialization
{
	// Object info for things like scenary, boost pad effects, mines, jump plates, etc
	public class FZObject : MonoBehaviour
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

		public static FZObject LoadObject(BinaryReader reader, int offset)
		{
			GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obj.transform.localScale = new Vector3(3f, 3f, 3f);
			obj.GetComponent<Renderer>().material.color = Color.red;
			obj.tag = "FZObject";
			FZObject fzObject = obj.AddComponent<FZObject>();

			reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to object info
			fzObject.address = (int)reader.BaseStream.Position;
			fzObject.unknown1 = BinarySerializer.ReadInt32(reader);
			fzObject.unknown2 = BinarySerializer.ReadInt32(reader);
			fzObject.unknown3 = BinarySerializer.ReadInt32(reader);

			fzObject.position = BinarySerializer.ReadVector(reader);
			fzObject.unknown4 = BinarySerializer.ReadInt32(reader);
			fzObject.unknown5 = BinarySerializer.ReadInt32(reader);
			fzObject.scale = BinarySerializer.ReadVector(reader);

			fzObject.unknown6 = BinarySerializer.ReadInt32(reader);
			fzObject.unknownOffset1 = BinarySerializer.ReadInt32(reader);
			fzObject.unknownOffset2 = BinarySerializer.ReadInt32(reader);

			fzObject.unknown7 = BinarySerializer.ReadInt32(reader);
			fzObject.orientationOffset = BinarySerializer.ReadInt32(reader);

			obj.transform.position = fzObject.position;
			if (fzObject.orientationOffset != 0)
				FZOrientation.LoadOrientation(obj, reader, fzObject.orientationOffset);

			return fzObject;
		}

		public static void WriteObject(BinaryWriter writer, FZObject obj)
		{
			writer.BaseStream.Seek(obj.address, SeekOrigin.Begin);
			BinarySerializer.Write(writer, obj.unknown1);
			BinarySerializer.Write(writer, obj.unknown2);
			BinarySerializer.Write(writer, obj.unknown3);

			BinarySerializer.Write(writer, obj.position);
			BinarySerializer.Write(writer, obj.unknown4);
			BinarySerializer.Write(writer, obj.unknown5);
			BinarySerializer.Write(writer, obj.scale);

			BinarySerializer.Write(writer, obj.unknown6);
			BinarySerializer.Write(writer, obj.unknownOffset1);
			BinarySerializer.Write(writer, obj.unknownOffset2);

			BinarySerializer.Write(writer, obj.unknown7);
			BinarySerializer.Write(writer, obj.orientationOffset);

			FZOrientation orientation = obj.GetComponent<FZOrientation>();
			if (orientation != null)
				FZOrientation.WriteOrientation(writer, orientation);
		}
	}

	// Orientation info associated with all FZObjects
	public class FZOrientation : MonoBehaviour
	{
		public int address;
		public Vector3 right; // Forward and right vectors might be switched (unconfirmed)
		public float positionX; // X position of the object/orientation vectors (seems redundant since the object already has this info)
		public Vector3 up;
		public float positionY; // Y position of the object/orientation vectors
		public Vector3 forward;
		public float positionZ; // Z position of the object/orientation vectors

		public static FZOrientation LoadOrientation(GameObject obj, BinaryReader reader, int offset)
		{
			FZOrientation fzOrientation = obj.AddComponent<FZOrientation>();

			reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to object orientation info
			fzOrientation.address = (int)reader.BaseStream.Position;
			fzOrientation.right = BinarySerializer.ReadVector(reader);
			fzOrientation.positionX = -BinarySerializer.ReadSingle(reader);

			fzOrientation.up = BinarySerializer.ReadVector(reader);
			fzOrientation.positionY = BinarySerializer.ReadSingle(reader);

			fzOrientation.forward = BinarySerializer.ReadVector(reader);
			fzOrientation.positionZ = BinarySerializer.ReadSingle(reader);

			obj.transform.rotation = Quaternion.LookRotation(fzOrientation.forward, fzOrientation.up);
			Debug.DrawRay(obj.transform.position, fzOrientation.right * 3f, Color.yellow, 999f);
			Debug.DrawRay(obj.transform.position, fzOrientation.up * 3f, Color.yellow, 999f);
			Debug.DrawRay(obj.transform.position, fzOrientation.forward * 3f, Color.yellow, 999f);

			return fzOrientation;
		}

		public static void WriteOrientation(BinaryWriter writer, FZOrientation orientation)
		{
			writer.BaseStream.Seek(orientation.address, SeekOrigin.Begin);
			BinarySerializer.Write(writer, orientation.right);
			BinarySerializer.Write(writer, -orientation.positionX);

			BinarySerializer.Write(writer, orientation.up);
			BinarySerializer.Write(writer, orientation.positionY);

			BinarySerializer.Write(writer, orientation.forward);
			BinarySerializer.Write(writer, orientation.positionZ);
		}
	}
}
