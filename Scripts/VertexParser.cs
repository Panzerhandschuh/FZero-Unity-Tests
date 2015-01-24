using UnityEngine;
using System;
using System.IO;

public class VertexParser : MonoBehaviour
{
	const int HEADER_SIZE = 256;

	void Start()
	{
		string testDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FZeroTests";
		string quadFile = testDirectory + "\\COLI_COURSE03,lz";
		ReadTriangleCollisions(quadFile);

		//string triangleFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\" + "tris.txt";
		//ReadTriangleCollisions(triangleFile);
	}

	void ReadTriangleCollisions(string filePath)
	{
		using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
		{
			reader.BaseStream.Seek(HEADER_SIZE, SeekOrigin.Begin);
			const int testLength = 60;
			for (int i = 0; i < testLength; i++)
			{
				reader.BaseStream.Seek(16, SeekOrigin.Current); // Skip normal/unknown data

				Vector3 v1 = ReadVector(reader);
				CreateVertex(v1);

				Vector3 v2 = ReadVector(reader);
				CreateVertex(v2);
				Debug.DrawLine(v1, v2, Color.red, 999f);

				Vector3 v3 = ReadVector(reader);
				CreateVertex(v3);
				Debug.DrawLine(v2, v3, Color.red, 999f);
				Debug.DrawLine(v3, v1, Color.red, 999f);

				reader.BaseStream.Seek(36, SeekOrigin.Current); // Skip unknown data
			}
		}
	}

	void ReadQuadCollisions(string filePath)
	{
		using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
		{
			reader.BaseStream.Seek(HEADER_SIZE, SeekOrigin.Begin);
			const int testLength = 30;
			for (int i = 0; i < testLength; i++)
			{
				reader.BaseStream.Seek(16, SeekOrigin.Current); // Skip normal/unknown data

				Vector3 v1 = ReadVector(reader);
				CreateVertex(v1);

				Vector3 v2 = ReadVector(reader);
				CreateVertex(v2);
				Debug.DrawLine(v1, v2, Color.red, 999f);

				Vector3 v3 = ReadVector(reader);
				CreateVertex(v3);
				Debug.DrawLine(v2, v3, Color.red, 999f);

				Vector3 v4 = ReadVector(reader);
				CreateVertex(v4);
				Debug.DrawLine(v3, v4, Color.red, 999f);
				Debug.DrawLine(v4, v1, Color.red, 999f);

				reader.BaseStream.Seek(48, SeekOrigin.Current); // Skip unknown data
			}
		}
	}

	//void ReadQuintCollisions(string filePath)
	//{
	//	using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
	//	{
	//		reader.BaseStream.Seek(HEADER_SIZE, SeekOrigin.Begin);
	//		const int testLength = 10;
	//		for (int i = 0; i < testLength; i++)
	//		{
	//			reader.BaseStream.Seek(16, SeekOrigin.Current); // Skip normal/unknown data

	//			Vector3 v1 = ReadVector(reader);
	//			CreateVertex(v1);

	//			Vector3 v2 = ReadVector(reader);
	//			CreateVertex(v2);
	//			Debug.DrawLine(v1, v2, Color.red, 999f);

	//			Vector3 v3 = ReadVector(reader);
	//			CreateVertex(v3);
	//			Debug.DrawLine(v2, v3, Color.red, 999f);

	//			Vector3 v4 = ReadVector(reader);
	//			CreateVertex(v4);
	//			Debug.DrawLine(v3, v4, Color.red, 999f);

	//			Vector3 v5 = ReadVector(reader);
	//			CreateVertex(v5);
	//			Debug.DrawLine(v4, v5, Color.red, 999f);
	//			Debug.DrawLine(v5, v1, Color.red, 999f);

	//			reader.BaseStream.Seek(54, SeekOrigin.Current); // Skip unknown data
	//		}
	//	}
	//}

	void CreateVertex(Vector3 position)
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		obj.transform.position = position;
	}

	Vector3 ReadVector(BinaryReader reader)
	{
		return new Vector3(ReadSingle(reader), ReadSingle(reader), ReadSingle(reader));
	}

	float ReadSingle(BinaryReader reader)
	{
		byte[] bytes = reader.ReadBytes(4);
		Array.Reverse(bytes); // Swap endianness
		return BitConverter.ToSingle(bytes, 0);
	}
}
