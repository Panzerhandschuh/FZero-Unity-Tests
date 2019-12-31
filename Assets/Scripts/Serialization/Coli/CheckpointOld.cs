using UnityEngine;
using System.IO;

namespace FZeroGXEditor.Serialization
{
	// Information about track splines
	// Editing some of these properties will change the in game collisions
	public class CheckpointOld : MonoBehaviour
	{
		public int address;
		public float trackOffset1; // Moves the track collisions forward/back?
		public float trackOffset2; // Moves the track collisions forward/back?
		public float unknown3; // This is related to collisions
		public Vector3 startTangent; // Rotating the tangent affects the collisions
		public Vector3 start; // Changing this does not change collisions, but it affects what parts of the track kill you (maybe related to activating checkpoints)
		public float unknown4; // This is related to collisions
		public Vector3 endTangent; // Rotating the tangent affects the collisions
		public Vector3 end; // Changing this does not change collisions, but it affects what parts of the track kill you (maybe related to activating checkpoints)
		public float unknown5; // Messes with CPU AI, but does not affect collisions
		public float unknown6; // Messes with CPU AI, but does not affect collisions
		public float width; // Seems to be related to track width, but changing this doesn't seem to do anything
		public byte startConnected; // Flag denoting whether or not the current spline has a connection with the previous spline (0 when there is no previous connection such as right after a dive)
		public byte endConnected; // Flag denoting whether or not the current spline has a connection with the next spline (0 when there is no next connection such as right before a dive)
		public byte flag3; // Always seems to be 0
		public byte flag4; // Always seems to be 0

		public static CheckpointOld LoadSpline(BinaryReader reader, int offset)
		{
			var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obj.transform.localScale = new Vector3(2f, 2f, 2f);
			obj.GetComponent<Renderer>().material.color = Color.yellow;
			obj.tag = "FZSpline";
			var spline = obj.AddComponent<CheckpointOld>();

			reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to spline info
			spline.address = (int)reader.BaseStream.Position;
			spline.trackOffset1 = BinarySerializer.ReadSingle(reader);
			spline.trackOffset2 = BinarySerializer.ReadSingle(reader);

			spline.unknown3 = BinarySerializer.ReadSingle(reader);
			spline.startTangent = BinarySerializer.ReadVector(reader);
			spline.start = BinarySerializer.ReadVector(reader);

			spline.unknown4 = BinarySerializer.ReadSingle(reader);
			spline.endTangent = BinarySerializer.ReadVector(reader);
			spline.end = BinarySerializer.ReadVector(reader);

			spline.unknown5 = BinarySerializer.ReadSingle(reader);
			spline.unknown6 = BinarySerializer.ReadSingle(reader);
			spline.width = BinarySerializer.ReadSingle(reader);
			spline.startConnected = reader.ReadByte();
			spline.endConnected = reader.ReadByte();
			spline.flag3 = reader.ReadByte();
			spline.flag4 = reader.ReadByte();

			if (spline.flag4 == 1)
				print(spline.address);
			obj.transform.position = (spline.start + spline.end) / 2f;
			Debug.DrawLine(obj.transform.position, spline.start, Color.green, 999f);
			Debug.DrawLine(obj.transform.position, spline.end, Color.green, 999f);
			Debug.DrawRay(obj.transform.position, spline.startTangent * 2f, Color.white, 999f);
			Debug.DrawRay(obj.transform.position, spline.endTangent * 2f, Color.white, 999f);

			return spline;
		}

		public static void WriteSpline(BinaryWriter writer, CheckpointOld spline)
		{
			writer.BaseStream.Seek(spline.address, SeekOrigin.Begin);
			BinarySerializer.Write(writer, spline.trackOffset1);
			BinarySerializer.Write(writer, spline.trackOffset2);

			BinarySerializer.Write(writer, spline.unknown3);
			BinarySerializer.Write(writer, spline.startTangent);
			BinarySerializer.Write(writer, spline.start);

			BinarySerializer.Write(writer, spline.unknown4);
			BinarySerializer.Write(writer, spline.endTangent);
			BinarySerializer.Write(writer, spline.end);

			BinarySerializer.Write(writer, spline.unknown5);
			BinarySerializer.Write(writer, spline.unknown6);
			BinarySerializer.Write(writer, spline.width);
			writer.Write(spline.startConnected);
			writer.Write(spline.endConnected);
			writer.Write(spline.flag3);
			writer.Write(spline.flag4);
		}
	}
}
