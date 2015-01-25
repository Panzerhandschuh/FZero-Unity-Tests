﻿using UnityEngine;
using System;
using System.IO;

public class CollisionParser : MonoBehaviour
{
	const int TABLE_ENTRY_SIZE = 16;

	// TODO: Axis conversion?
	void Start()
	{
		string testDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\FZeroTests";
		string filePath = testDirectory + "\\COLI_COURSE01,lz";
		using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
		{
			reader.BaseStream.Seek(100, SeekOrigin.Begin); // Seek to header info about mesh offset table
			int tableSize = ReadInt32(reader);
			int tableOffset = ReadInt32(reader);
			ReadMeshTable(reader, tableSize, tableOffset);

			reader.BaseStream.Seek(28, SeekOrigin.Begin); // Seek to header info about extra mesh offset table
			int extraTableOffset = ReadInt32(reader);
			ReadMeshExtraTable(reader, extraTableOffset);
		}
	}

	// Read table containing triangle and quad meshes
	void ReadMeshTable(BinaryReader reader, int numEntries, int offset)
	{
		print("Number of Mesh Table Entries: " + numEntries);
		print("Mesh Table Offset: " + offset);

		int objectCount = 0;
		for (int i = 0; i < numEntries; i++) // Each table entry is 16 bytes
		{
			reader.BaseStream.Seek(offset + (TABLE_ENTRY_SIZE * i), SeekOrigin.Begin); // Go to the current table entry
			reader.BaseStream.Seek(12, SeekOrigin.Current); // Skip first 12 bytes of data on table entry (unknown data)

			int collisionInfoOffset = ReadInt32(reader);
			if (collisionInfoOffset != 0) // Ignore null offsets
			{
				ReadCollisionInfo(reader, collisionInfoOffset);
				objectCount++;
			}
		}

		print("Number of mesh objects found: " + objectCount);
	}

	// Read the other table that contains offsets to triangle and quad meshes
	void ReadMeshExtraTable(BinaryReader reader, int offset)
	{
		print("Extra Mesh Table Offset: " + offset);

		// Read triangle meshes
		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to start of extra mesh table
		reader.BaseStream.Seek(36, SeekOrigin.Current); // Skip 36 null bytes
		int startTriangleOffset = ReadInt32(reader);
		int nextTriangleOffset = ReadInt32(reader);
		int numTriangles = (nextTriangleOffset - startTriangleOffset - 28) / 88; // Remove 28 extra unknown bytes between both triangle mesh offsets
		ReadTriangles(reader, numTriangles, startTriangleOffset);

		// Read quad meshes
		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to start of extra mesh table
		int quadOffset = 36 + 60 + 24; // 36 null bytes, 60 offset bytes for triangle mesh section, and 24 bytes of unknown data to get to the offset for the first quad mesh
		reader.BaseStream.Seek(quadOffset, SeekOrigin.Current); // Skip 36 null bytes
		int startQuadOffset = ReadInt32(reader);
		int nextQuadOffset = ReadInt32(reader);
		int numQuads = (nextQuadOffset - startQuadOffset - 28) / 112;
		ReadQuads(reader, numQuads, startQuadOffset);
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
			reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip unknown data

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
