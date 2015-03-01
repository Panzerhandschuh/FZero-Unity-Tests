using UnityEngine;
using System;
using System.IO;

public class CollisionParser : MonoBehaviour
{
	public bool readMeshCollisions = true;
	public bool readObjects = true;
	public bool loadFromOutput = false; // Load modified course files in the output folder
	public int courseNumber = 3;

	const int TABLE_ENTRY_SIZE = 16;
	const int SPLINE_ENTRY_SIZE = 12;
	const int OBJECT_ENTRY_SIZE = 64;
	const int TRIANGLE_ENTRY_SIZE = 88;
	const int QUAD_ENTRY_SIZE = 112;

	private string filePath;


	void Start()
	{
		string folder = (loadFromOutput) ? "Output" : "Input";

		//Windows specific path
		if (Application.platform == RuntimePlatform.WindowsEditor) {
			filePath = "Assets\\" + folder + "\\COLI_COURSE" + courseNumber.ToString ("00") + ",lz";

		//Mac OSX specific path
		} else if (Application.platform == RuntimePlatform.OSXEditor) {
			filePath = "Assets/" + folder + "/COLI_COURSE" + courseNumber.ToString ("00") + ",lz";
		}

		using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
		{
			// Read spline data
			reader.BaseStream.Seek(8, SeekOrigin.Begin); // Seek to header info about spline data table
			int splineTableSize = BinarySerializer.ReadInt32(reader);
			int splineTableOffset = BinarySerializer.ReadInt32(reader);
			ReadSplineTable(reader, splineTableSize, splineTableOffset);

			// Read main mesh data
			reader.BaseStream.Seek(100, SeekOrigin.Begin); // Seek to header info about mesh offset table
			int tableSize = BinarySerializer.ReadInt32(reader);
			int tableOffset = BinarySerializer.ReadInt32(reader);
			ReadMeshTable(reader, tableSize, tableOffset);

			// Read extra mesh data (boost pads, heal regions, jump pads, misc collisions)
			reader.BaseStream.Seek(28, SeekOrigin.Begin); // Seek to header info about extra mesh offset table
			int extraTableOffset = BinarySerializer.ReadInt32(reader);
			ReadMeshExtraTable(reader, extraTableOffset);

			// Read unknown data (0x10)
			//reader.BaseStream.Seek(16, SeekOrigin.Begin); // Seek to header info about unknown data table
			//int unknownTableSize = ReadInt32(reader);
			//int unknownTableOffset = ReadInt32(reader);
			//ReadUnknownTable0x10(reader, unknownTableSize, unknownTableOffset);

			// Read object data (boost pad locations, mines)
			if (readObjects)
			{
				reader.BaseStream.Seek(72, SeekOrigin.Begin); // Seek to header info about number of objects
				int objectTableSize = BinarySerializer.ReadInt32(reader);
				reader.BaseStream.Seek(84, SeekOrigin.Begin); // Seek to header info about object table offset
				int objectTableOffset = BinarySerializer.ReadInt32(reader);
				ReadObjectTable(reader, objectTableSize, objectTableOffset);
			}

			// Read unknown data (0x84)
			//reader.BaseStream.Seek(132, SeekOrigin.Begin);
			//int unknownTableOffset = ReadInt32(reader);
			//ReadUnknownTable0x84(reader, unknownTableOffset);
		}
	}

	void ReadSplineTable(BinaryReader reader, int numEntries, int offset)
	{
		print("Number of Spline Entries: " + numEntries);
		print("Spline Table Offset: " + offset);

		for (int i = 0; i < numEntries; i++)
		{
			reader.BaseStream.Seek(offset + (SPLINE_ENTRY_SIZE * i), SeekOrigin.Begin); // Go to the spline list
			reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip unknown data

			int splineOffset = BinarySerializer.ReadInt32(reader);
			Spline spline = Spline.LoadSpline(reader, splineOffset);
		}
	}

	// Read table containing triangle and quad meshes
	void ReadMeshTable(BinaryReader reader, int numEntries, int offset)
	{
		print("Number of Mesh Table Entries: " + numEntries);
		print("Mesh Table Offset: " + offset);

		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to the mesh collisions table

		int objectCount = 0;
		for (int i = 0; i < numEntries; i++) // Each table entry is 16 bytes
		{
			reader.BaseStream.Seek(12, SeekOrigin.Current); // Skip first 12 bytes of data on table entry (unknown data)

			int collisionInfoOffset = BinarySerializer.ReadInt32(reader);
			if (collisionInfoOffset != 0) // Ignore null offsets
			{
				ReadCollisionInfo(reader, collisionInfoOffset);
				objectCount++;
			}
		}

		print("Number of mesh objects found: " + objectCount);
	}

	void ReadCollisionInfo(BinaryReader reader, int offset)
	{
		print("Collision Info Offset: " + offset);

		// Store stream position
		long startOffset = reader.BaseStream.Position;

		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to the collision info entry
		// Collision info entries are 36 bytes long
		reader.BaseStream.Seek(20, SeekOrigin.Current); // Skip the first 20 bytes (unknown data)

		int numTriangles = BinarySerializer.ReadInt32(reader);
		int numQuads = BinarySerializer.ReadInt32(reader);
		int trianglesOffset = BinarySerializer.ReadInt32(reader);
		int quadOffset = BinarySerializer.ReadInt32(reader);
		ReadTriangles(reader, numTriangles, trianglesOffset, Color.red);
		ReadQuads(reader, numQuads, quadOffset, Color.red);

		// Restore stream position
		reader.BaseStream.Seek(startOffset, SeekOrigin.Begin);
	}

	// Read the mesh table that contains offsets to triangle and quad meshes (boost pads, heal/dirt regions, misc collisions)
	void ReadMeshExtraTable(BinaryReader reader, int offset)
	{
		print("Extra Mesh Table Offset: " + offset);

		// Read triangle meshes
		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to start of extra mesh table
		reader.BaseStream.Seek(36, SeekOrigin.Current); // Skip 36 null bytes
		int startTriangleOffset = BinarySerializer.ReadInt32(reader);
		int nextTriangleOffset = BinarySerializer.ReadInt32(reader);
		int numTriangles = (nextTriangleOffset - startTriangleOffset - 24) / TRIANGLE_ENTRY_SIZE; // Find the difference between the start triangle mesh offset and the next triangle mesh offset, subtract 24 bytes (unknown data), then divide by size of triangle entry
		ReadTriangles(reader, numTriangles, startTriangleOffset, new Color(1f, 0.5f, 0f));

		// Read quad meshes
		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to start of extra mesh table
		int quadOffset = 36 + 60 + 24; // 36 null bytes, 60 offset bytes for triangle mesh section, and 24 bytes of unknown data to get to the offset for the first quad mesh
		reader.BaseStream.Seek(quadOffset, SeekOrigin.Current); // Skip 36 null bytes
		int startQuadOffset = BinarySerializer.ReadInt32(reader);
		int nextQuadOffset = BinarySerializer.ReadInt32(reader);
		int numQuads = (nextQuadOffset - startQuadOffset - 24) / QUAD_ENTRY_SIZE;
		ReadQuads(reader, numQuads, startQuadOffset, new Color(1f, 0.25f, 0f));
	}

	void ReadUnknownTable0x10(BinaryReader reader, int numEntries, int offset)
	{
		print("Number of Unknown Table Entries: " + numEntries);
		print("Unknown Table Offset: " + offset);

		reader.BaseStream.Seek(offset, SeekOrigin.Begin);
		for (int i = 0; i < numEntries; i++)
		{
			ReadTriangle(reader);

			reader.BaseStream.Seek(8, SeekOrigin.Current);
		}
	}

	// Table for object positions (start line, mines, boosts)
	void ReadObjectTable(BinaryReader reader, int numEntries, int offset)
	{
		print("Number of Object Table Entries: " + numEntries);
		print("Object Table Offset: " + offset);

		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to start of object table

		for (int i = 0; i < numEntries; i++)
		{
			reader.BaseStream.Seek(12, SeekOrigin.Current); // Skip 12 unknown bytes

			Vector3 pos = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(pos, new Vector3(5f, 5f, 5f), Color.red);

			reader.BaseStream.Seek(24, SeekOrigin.Current); // Skip 24 unknown bytes
			int extraInfoOffset = BinarySerializer.ReadInt32(reader);
			if (extraInfoOffset != 0)
				ReadObjectExtraInfo(reader, extraInfoOffset);

			reader.BaseStream.Seek(8, SeekOrigin.Current); // Skip 8 unknown bytes
			int orientationOffset = BinarySerializer.ReadInt32(reader);
			ReadObjectOrientation(reader, pos, orientationOffset);
		}
	}

	// Unknown info about the object
	void ReadObjectExtraInfo(BinaryReader reader, int offset)
	{
		//print("Object Extra Info Offset: " + offset);

		// Store stream position
		long startOffset = reader.BaseStream.Position;

		reader.BaseStream.Seek(offset, SeekOrigin.Begin);
		reader.BaseStream.Seek(28, SeekOrigin.Current); // Skip 28 unknown/null bytes

		const int numAddresses = 33; // Number of 8 byte length + address pairs to check
		for (int i = 0; i < numAddresses; i++)
		{
			int extraSize = BinarySerializer.ReadInt32(reader);
			int extraOffset = BinarySerializer.ReadInt32(reader);
			if (extraOffset != 0)
				ReadObjectExtraItems(reader, extraSize, extraOffset);
		}

		// Restore stream position
		reader.BaseStream.Seek(startOffset, SeekOrigin.Begin);
	}

	// A list of unknown items related to the object info
	void ReadObjectExtraItems(BinaryReader reader, int numEntries, int offset)
	{
		//print(numEntries + " " + offset);

		// Store stream position
		long startOffset = reader.BaseStream.Position;

		reader.BaseStream.Seek(offset, SeekOrigin.Begin);
		for (int i = 0; i < numEntries; i++)
		{
			reader.BaseStream.Seek(8, SeekOrigin.Current); // Skip 8 unknown bytes

			Vector3 v = BinarySerializer.ReadVector(reader);
			//CreateVertex(v, new Vector3(5f, 5f, 5f), Color.green);
		}

		// Restore stream position
		reader.BaseStream.Seek(startOffset, SeekOrigin.Begin);
	}

	// The rotation of the objects?
	void ReadObjectOrientation(BinaryReader reader, Vector3 position, int offset)
	{
		// Store stream position
		long startOffset = reader.BaseStream.Position;

		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to object orientation data

		Vector3 v1 = BinarySerializer.ReadVector(reader).normalized;
		Debug.DrawRay(position, v1 * 5f, Color.yellow, 999f);

		reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip 4 unknown bytes

		Vector3 v2 = BinarySerializer.ReadVector(reader).normalized;
		Debug.DrawRay(position, v2 * 5f, Color.yellow, 999f);

		reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip 4 unknown bytes

		Vector3 v3 = BinarySerializer.ReadVector(reader).normalized;
		Debug.DrawRay(position, v3 * 5f, Color.yellow, 999f);

		//reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip 4 unknown bytes

		// Restore stream position
		reader.BaseStream.Seek(startOffset, SeekOrigin.Begin);
	}

	void ReadUnknownTable0x84(BinaryReader reader, int offset)
	{
		print("Unknown Table Offset: " + offset);

		const int numEntries = 36 / 3;
		reader.BaseStream.Seek(offset, SeekOrigin.Begin);
		for (int i = 0; i < numEntries; i++)
		{
			ReadTriangle(reader);
		}
	}

	public void SaveChanges()
	{
		string filePath = "Assets\\Output\\COLI_COURSE" + courseNumber.ToString("00") + ",lz";
		if (!File.Exists(filePath))
		{
			string inputFilePath = "Assets\\Input\\COLI_COURSE" + courseNumber.ToString("00") + ",lz";
			File.Copy(inputFilePath, filePath);
		}

		using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Open)))
		{
			GameObject[] splines = GameObject.FindGameObjectsWithTag("Spline");
			for (int i = 0; i < splines.Length; i++)
			{
				Spline spline = splines[i].GetComponent<Spline>();

				// Track offset?
				//spline.trackOffset1 += 50f;
				//spline.trackOffset2 += 50f;

				// Make tracks entirely straight?
				//spline.trackOffset1 = 0f;
				//spline.trackOffset1 = 0f;

				// Rotates track?
				//spline.startTangent = Quaternion.Euler(20f, 0f, 0f) * spline.startTangent;
				//spline.endTangent = Quaternion.Euler(20f, 0f, 0f) * spline.endTangent;

				// Connect gaps (seems to only affect checkpoint system loading the player right on the edge of gaps)
				//spline.startConnected = 1;
				//spline.endConnected = 1;

				Spline.WriteSpline(writer, spline);
			}
		}

		print("Changes have been saved");
	}

	void ReadTriangles(BinaryReader reader, int numTriangles, int offset, Color color)
	{
		print("Number of Triangles: " + numTriangles);
		print("Triangles Offset: " + offset);

		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to the start of the triangles list
		for (int i = 0; i < numTriangles; i++) // Each triangle entry is 88 bytes
		{
			reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip unknown data (might be related to checkpoint ids)

			// Read normal
			Vector3 n = BinarySerializer.ReadVector(reader);

			// Read the edges of the triangle
			Vector3 v1 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v1);

			Vector3 v2 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v2);
			Debug.DrawLine(v1, v2, color, 999f);

			Vector3 v3 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v3);
			Debug.DrawLine(v2, v3, color, 999f);
			Debug.DrawLine(v3, v1, color, 999f);

			// Display normal
			Vector3 nPos = (v1 + v2 + v3) / 3f;
			Debug.DrawRay(nPos, n * 5f, Color.blue, 999f);

			reader.BaseStream.Seek(36, SeekOrigin.Current); // Skip unknown data
		}
	}

	// For testing purposes
	void ReadTriangle(BinaryReader reader)
	{
		Vector3 v1 = BinarySerializer.ReadVector(reader);
		DebugHelper.CreateVertex(v1);

		Vector3 v2 = BinarySerializer.ReadVector(reader);
		DebugHelper.CreateVertex(v2);
		Debug.DrawLine(v1, v2, Color.red, 999f);

		Vector3 v3 = BinarySerializer.ReadVector(reader);
		DebugHelper.CreateVertex(v3);
		Debug.DrawLine(v2, v3, Color.red, 999f);
		Debug.DrawLine(v3, v1, Color.red, 999f);
	}

	void ReadQuads(BinaryReader reader, int numQuads, int offset, Color color)
	{
		print("Number of Quads: " + numQuads);
		print("Quads Offset: " + offset);

		reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to the start of the quads list
		for (int i = 0; i < numQuads; i++) // Each quad entry is 112 bytes long
		{
			reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip unknown data (might be related to checkpoint ids)

			// Read normal
			Vector3 n = BinarySerializer.ReadVector(reader);

			// Read the edges of the quad
			Vector3 v1 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v1);

			Vector3 v2 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v2);
			Debug.DrawLine(v1, v2, color, 999f);

			Vector3 v3 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v3);
			Debug.DrawLine(v2, v3, color, 999f);

			Vector3 v4 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v4);
			Debug.DrawLine(v3, v4, color, 999f);
			Debug.DrawLine(v4, v1, color, 999f);

			// Display normal
			Vector3 nPos = (v1 + v2 + v3 + v4) / 4f;
			Debug.DrawRay(nPos, n * 5f, Color.blue, 999f);

			reader.BaseStream.Seek(48, SeekOrigin.Current); // Skip unknown data
		}
	}

	// For testing purposes
	void ReadQuad(BinaryReader reader)
	{
		Vector3 v1 = BinarySerializer.ReadVector(reader);
		DebugHelper.CreateVertex(v1);

		Vector3 v2 = BinarySerializer.ReadVector(reader);
		DebugHelper.CreateVertex(v2);
		Debug.DrawLine(v1, v2, Color.red, 999f);

		Vector3 v3 = BinarySerializer.ReadVector(reader);
		DebugHelper.CreateVertex(v3);
		Debug.DrawLine(v2, v3, Color.red, 999f);

		Vector3 v4 = BinarySerializer.ReadVector(reader);
		DebugHelper.CreateVertex(v4);
		Debug.DrawLine(v3, v4, Color.red, 999f);
		Debug.DrawLine(v4, v1, Color.red, 999f);
	}
}
