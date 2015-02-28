using UnityEngine;
using System.IO;

public class Spline : MonoBehaviour
{
	public int address;
	public float trackOffset1;
	public float trackOffset2;
	public float unknown3;
	public Vector3 startTangent;
	public Vector3 start;
	public float unknown4;
	public Vector3 endTangent;
	public Vector3 end;
	public float unknown5;
	public float unknown6;
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
		spline.type = BinarySerializer.ReadInt32(reader);

		obj.transform.position = (spline.start + spline.end) / 2f;
		Debug.DrawLine(obj.transform.position, spline.start, Color.green, 999f);
		Debug.DrawLine(obj.transform.position, spline.end, Color.green, 999f);
		Debug.DrawRay(obj.transform.position, spline.startTangent, Color.white, 999f);
		Debug.DrawRay(obj.transform.position, spline.endTangent, Color.white, 999f);

		return spline;
	}

	public static void WriteSpline(BinaryWriter writer, Spline spline)
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
		BinarySerializer.Write(writer, spline.type);
	}
}
