using UnityEngine;
using System.IO;

public class Spline : MonoBehaviour
{
	public string address;
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
		Spline spline = obj.AddComponent<Spline>();

		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to spline info
		spline.address = reader.BaseStream.Position.ToString("X");
		spline.unknown1 = CollisionParser.ReadInt32(reader);
		spline.unknown2 = CollisionParser.ReadInt32(reader);

		spline.unknown3 = CollisionParser.ReadInt32(reader);
		spline.startTangent = CollisionParser.ReadVector(reader);
		spline.start = CollisionParser.ReadVector(reader);

		spline.unknown4 = CollisionParser.ReadInt32(reader);
		spline.endTangent = CollisionParser.ReadVector(reader);
		spline.end = CollisionParser.ReadVector(reader);

		spline.unknown5 = CollisionParser.ReadInt32(reader);
		spline.unknown6 = CollisionParser.ReadInt32(reader);
		spline.width = CollisionParser.ReadSingle(reader);
		spline.type = CollisionParser.ReadInt32(reader);

		obj.transform.position = spline.start;
		//end.transform.position = spline.start1;
		Debug.DrawLine(spline.start, spline.end, Color.green, 999f);
		Debug.DrawRay(spline.start, spline.startTangent, Color.white, 999f);
		Debug.DrawRay(spline.end, spline.endTangent, Color.white, 999f);

		return spline;
	}
}
