using UnityEngine;
using System.IO;

public class Spline : MonoBehaviour
{
	public int address;
	public int unknown1;
	public int unknown2;
	public int unknown3;
	public Vector3 startTangent;
	public Vector3 start;
	public int unknown4;
	public Vector3 endTangent;
	public Vector3 end;
	public int unknown5;
	public int unknown6;
	public float width;
	public int type;

	public static Spline LoadSpline(BinaryReader reader, int offset)
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//GameObject end = GameObject.CreatePrimitive(PrimitiveType.Cube);
		obj.tag = "Spline";
		Spline spline = obj.AddComponent<Spline>();

		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to spline info
		spline.address = (int)reader.BaseStream.Position;
		spline.unknown1 = BinarySerializer.ReadInt32(reader);
		spline.unknown2 = BinarySerializer.ReadInt32(reader);

		spline.unknown3 = BinarySerializer.ReadInt32(reader);
		spline.startTangent = BinarySerializer.ReadVector(reader);
		spline.start = BinarySerializer.ReadVector(reader);

		spline.unknown4 = BinarySerializer.ReadInt32(reader);
		spline.endTangent = BinarySerializer.ReadVector(reader);
		spline.end = BinarySerializer.ReadVector(reader);

		spline.unknown5 = BinarySerializer.ReadInt32(reader);
		spline.unknown6 = BinarySerializer.ReadInt32(reader);
		spline.width = BinarySerializer.ReadSingle(reader);
		spline.type = BinarySerializer.ReadInt32(reader);

		obj.transform.position = spline.start;
		//end.transform.position = spline.start1;
		Debug.DrawLine(spline.start, spline.end, Color.green, 999f);
		Debug.DrawRay(spline.start, spline.startTangent, Color.white, 999f);
		Debug.DrawRay(spline.end, spline.endTangent, Color.white, 999f);

		return spline;
	}

	public static void WriteSpline(BinaryWriter writer, Spline spline)
	{
		writer.BaseStream.Seek(spline.address, SeekOrigin.Begin);
		BinarySerializer.Write(writer, spline.unknown1);
		BinarySerializer.Write(writer, spline.unknown2);

		BinarySerializer.Write(writer, spline.unknown3);
		BinarySerializer.Write(writer, spline.startTangent);
		BinarySerializer.Write(writer, spline.start);

		BinarySerializer.Write(writer, spline.unknown4);
		BinarySerializer.Write(writer, spline.endTangent);
		BinarySerializer.Write(writer, spline.end);

		BinarySerializer.Write(writer, spline.unknown5);
		BinarySerializer.Write(writer, spline.unknown6);
		BinarySerializer.Write(writer, spline.width);
		BinarySerializer.Write(writer, spline.type);
	}
}
