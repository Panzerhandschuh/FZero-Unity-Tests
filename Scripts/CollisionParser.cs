using UnityEngine;
using System;
using System.IO;

public class CollisionParser : MonoBehaviour
{
	const int TABLE_ENTRY_SIZE = 16;

	void Start()
	{
		string testDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FZeroTests";
		string filePath = testDirectory + "\\COLI_COURSE03,lz";
		using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
		{
			reader.BaseStream.Seek(100, SeekOrigin.Begin); // Seek to header info about offsets table
			int tableSize = ReadInt32(reader);
			int tableOffset = ReadInt32(reader);
			ReadMeshTable(reader, tableSize, tableOffset);
		}
	}

	// Read table containing triangle and quad meshes
	void ReadMeshTable(BinaryReader reader, int numEntries, int offset)
	{
		print("Number of Table Entries: " + numEntries);
		print("Table Offset: " + offset);

		int validObjectCount = 0;
		for (int i = 0; i < numEntries; i++) // Each table entry is 16 bytes
		{
			reader.BaseStream.Seek(offset + (TABLE_ENTRY_SIZE * i), SeekOrigin.Begin); // Go to the current table entry
			reader.BaseStream.Seek(12, SeekOrigin.Current); // Skip first 12 bytes of data on table entry (unknown data)

			int collisionInfoOffset = ReadInt32(reader);
			if (collisionInfoOffset != 0) // Ignore null offsets
			{
				ReadCollisionInfo(reader, collisionInfoOffset);
				validObjectCount++;
			}
		}

		print("Number of valid objects found: " + validObjectCount);
	}

	void ReadCollisionInfo(BinaryReader reader, int offset)
	{
		print("Collision Info Offset: " + offset);

		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to the collision info entry
		// Collision info entries are 36 bytes long
		reader.BaseStream.Seek(20, SeekOrigin.Current); // Skip the first 20 bytes (unknown data)

		int numTriangles = ReadInt32(reader);
		int numQuads = ReadInt32(reader);
		int trianglesOffset = ReadInt32(reader);
		int quadOffset = ReadInt32(reader);
		ReadTriangles(reader, numTriangles, trianglesOffset);
		ReadQuads(reader, numQuads, quadOffset);
	}

	void ReadTriangles(BinaryReader reader, int numTriangles, int offset)
	{
		print("Number of Triangles: " + numTriangles);
		print("Triangles Offset: " + offset);

		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to the start of the triangles list
		for (int i = 0; i < numTriangles; i++) // Each triangle entry is 88 bytes
		{
			reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip unknown data

			// Read normal
			Vector3 n = ReadVector(reader);

			// Read the edges of the triangle
			Vector3 v1 = ReadVector(reader);
			CreateVertex(v1);

			Vector3 v2 = ReadVector(reader);
			CreateVertex(v2);
			Debug.DrawLine(v1, v2, Color.red, 999f);

			Vector3 v3 = ReadVector(reader);
			CreateVertex(v3);
			Debug.DrawLine(v2, v3, Color.red, 999f);
			Debug.DrawLine(v3, v1, Color.red, 999f);

			// Display normal
			Vector3 nPos = (v1 + v2 + v3) / 3f;
			Debug.DrawRay(nPos, n, Color.blue, 999f);

			reader.BaseStream.Seek(36, SeekOrigin.Current); // Skip unknown data
		}
	}

	void ReadQuads(BinaryReader reader, int numQuads, int offset)
	{
		print("Number of Quads: " + numQuads);
		print("Quads Offset: " + offset);

		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to the start of the quads list
		for (int i = 0; i < numQuads; i++) // Each quad entry is 112 bytes long
		{
			reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip normal/unknown data

			// Read normal
			Vector3 n = ReadVector(reader);

			// Read the edges of the quad
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

			// Display normal
			Vector3 nPos = (v1 + v2 + v3 + v4) / 4f;
			Debug.DrawRay(nPos, n, Color.blue, 999f);

			reader.BaseStream.Seek(48, SeekOrigin.Current); // Skip unknown data
		}
	}

	// Used for visuallizing vertices
	void CreateVertex(Vector3 position)
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		obj.transform.position = position;
	}

	Vector3 ReadVector(BinaryReader reader)
	{
		return new Vector3(ReadSingle(reader), ReadSingle(reader), ReadSingle(reader));
	}

	int ReadInt32(BinaryReader reader)
	{
		byte[] bytes = reader.ReadBytes(4);
		Array.Reverse(bytes); // Swap endianness
		return BitConverter.ToInt32(bytes, 0);
	}

	float ReadSingle(BinaryReader reader)
	{
		byte[] bytes = reader.ReadBytes(4);
		Array.Reverse(bytes); // Swap endianness
		return BitConverter.ToSingle(bytes, 0);
	}
}
