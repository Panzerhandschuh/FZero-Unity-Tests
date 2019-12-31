using System.IO;
using UnityEngine;

namespace FZeroGXEditor.Serialization
{
	public class Checkpoint : IBinarySerializable
	{
		public long address;
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

		public void Serialize(FZWriter writer)
		{
			writer.BaseStream.Seek(address, SeekOrigin.Begin);
			writer.Write(trackOffset1);
			writer.Write(trackOffset2);
			writer.Write(unknown3);
			writer.Write(startTangent);
			writer.Write(start);
			writer.Write(unknown4);
			writer.Write(endTangent);
			writer.Write(end);
			writer.Write(unknown5);
			writer.Write(unknown6);
			writer.Write(width);
			writer.Write(startConnected);
			writer.Write(endConnected);
			writer.Write(flag3);
			writer.Write(flag4);
		}

		public static Checkpoint Deserialize(FZReader reader)
		{
			var obj = new Checkpoint();

			obj.address = reader.BaseStream.Position;
			obj.trackOffset1 = reader.ReadSingle();
			obj.trackOffset2 = reader.ReadSingle();
			obj.unknown3 = reader.ReadSingle();
			obj.startTangent = reader.ReadVector();
			obj.start = reader.ReadVector();
			obj.unknown4 = reader.ReadSingle();
			obj.endTangent = reader.ReadVector();
			obj.end = reader.ReadVector();
			obj.unknown5 = reader.ReadSingle();
			obj.unknown6 = reader.ReadSingle();
			obj.width = reader.ReadSingle();
			obj.startConnected = reader.ReadByte();
			obj.endConnected = reader.ReadByte();
			obj.flag3 = reader.ReadByte();
			obj.flag4 = reader.ReadByte();

			return obj;
		}
	}
}
