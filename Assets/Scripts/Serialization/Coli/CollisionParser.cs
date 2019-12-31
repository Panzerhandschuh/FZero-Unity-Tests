using UnityEngine;
using System;
using System.IO;
using FZeroGXEditor.Utilities;

namespace FZeroGXEditor.Serialization
{
	public class CollisionParser : MonoBehaviour
	{
		public int courseNumber = 3;
		public bool readMeshCollisions = true;
		public bool readObjectCollisions = true;
		public bool readObjects = true;
		public bool loadFromOutput = false; // Load modified course files in the output folder

		public bool displayVertices = true;

		const int TABLE_ENTRY_SIZE = 16;
		const int SPLINE_ENTRY_SIZE = 12;
		const int OBJECT_ENTRY_SIZE = 64;
		const int TRIANGLE_ENTRY_SIZE = 88;
		const int QUAD_ENTRY_SIZE = 112;

		void Start()
		{
			var folder = (loadFromOutput) ? "Output" : "Input";
			var filePath = "Assets\\" + folder + "\\COLI_COURSE" + courseNumber.ToString("00") + ",lz";
			using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
			{
				// Read spline data
				reader.BaseStream.Seek(8, SeekOrigin.Begin); // Seek to header info about spline data table
				var splineTableSize = BinarySerializer.ReadInt32(reader);
				var splineTableOffset = BinarySerializer.ReadInt32(reader);
				ReadSplineTable(reader, splineTableSize, splineTableOffset);

				// Read main mesh data
				if (readMeshCollisions)
				{
					reader.BaseStream.Seek(100, SeekOrigin.Begin); // Seek to header info about mesh offset table
					var tableSize = BinarySerializer.ReadInt32(reader);
					var tableOffset = BinarySerializer.ReadInt32(reader);
					ReadMeshCollisionTable(reader, tableSize, tableOffset);
				}

				// Read object collision data (boost pads, heal regions, jump pads, misc collisions)
				if (readObjectCollisions)
				{
					reader.BaseStream.Seek(28, SeekOrigin.Begin); // Seek to header info about extra mesh offset table
					var extraTableOffset = BinarySerializer.ReadInt32(reader);
					ReadObjectCollisionTable(reader, extraTableOffset);
				}

				// Read unknown data (0x10)
				//reader.BaseStream.Seek(16, SeekOrigin.Begin); // Seek to header info about unknown data table
				//int unknownTableSize = ReadInt32(reader);
				//int unknownTableOffset = ReadInt32(reader);
				//ReadUnknownTable0x10(reader, unknownTableSize, unknownTableOffset);

				// Read object data (boost pad locations, mines)
				if (readObjects)
				{
					reader.BaseStream.Seek(72, SeekOrigin.Begin); // Seek to header info about number of objects
					var objectTableSize = BinarySerializer.ReadInt32(reader);
					reader.BaseStream.Seek(84, SeekOrigin.Begin); // Seek to header info about object table offset
					var objectTableOffset = BinarySerializer.ReadInt32(reader);
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

			for (var i = 0; i < numEntries; i++)
			{
				reader.BaseStream.Seek(offset + (SPLINE_ENTRY_SIZE * i), SeekOrigin.Begin); // Go to the spline list
				reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip unknown data

				var splineOffset = BinarySerializer.ReadInt32(reader);
				var spline = CheckpointOld.LoadSpline(reader, splineOffset);
			}
		}

		// Read table containing triangle and quad meshes
		void ReadMeshCollisionTable(BinaryReader reader, int numEntries, int offset)
		{
			print("Number of Mesh Table Entries: " + numEntries);
			print("Mesh Table Offset: " + offset);

			reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to the mesh collisions table

			var objectCount = 0;
			for (var i = 0; i < numEntries; i++) // Each table entry is 16 bytes
			{
				reader.BaseStream.Seek(12, SeekOrigin.Current); // Skip first 12 bytes of data on table entry (unknown data)

				var collisionInfoOffset = BinarySerializer.ReadInt32(reader);
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
			var startOffset = reader.BaseStream.Position;

			reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to the collision info entry
															  // Collision info entries are 36 bytes long
			reader.BaseStream.Seek(20, SeekOrigin.Current); // Skip the first 20 bytes (unknown data)

			var numTriangles = BinarySerializer.ReadInt32(reader);
			var numQuads = BinarySerializer.ReadInt32(reader);
			var trianglesOffset = BinarySerializer.ReadInt32(reader);
			var quadOffset = BinarySerializer.ReadInt32(reader);
			ReadTriangles(reader, numTriangles, trianglesOffset, Color.red);
			ReadQuads(reader, numQuads, quadOffset, Color.red);

			// Restore stream position
			reader.BaseStream.Seek(startOffset, SeekOrigin.Begin);
		}

		// Read the object collision table that contains offsets to triangle and quad meshes (boost pads, heal/dirt regions, misc collisions)
		void ReadObjectCollisionTable(BinaryReader reader, int offset)
		{
			print("Object Collision Table Offset: " + offset);

			// Read triangle meshes
			reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to start of extra mesh table
			reader.BaseStream.Seek(36, SeekOrigin.Current); // Skip 36 null bytes
			var startTriangleOffset = BinarySerializer.ReadInt32(reader);
			var nextTriangleOffset = BinarySerializer.ReadInt32(reader);
			var numTriangles = (nextTriangleOffset - startTriangleOffset - 24) / TRIANGLE_ENTRY_SIZE; // Find the difference between the start triangle mesh offset and the next triangle mesh offset, subtract 24 bytes (unknown data), then divide by size of triangle entry
			ReadTriangles(reader, numTriangles, startTriangleOffset, new Color(1f, 0.5f, 0f));

			// Read quad meshes
			reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to start of extra mesh table
			var quadOffset = 36 + 60 + 24; // 36 null bytes, 60 offset bytes for triangle mesh section, and 24 bytes of unknown data to get to the offset for the first quad mesh
			reader.BaseStream.Seek(quadOffset, SeekOrigin.Current); // Skip 36 null bytes
			var startQuadOffset = BinarySerializer.ReadInt32(reader);
			var nextQuadOffset = BinarySerializer.ReadInt32(reader);
			var numQuads = (nextQuadOffset - startQuadOffset - 24) / QUAD_ENTRY_SIZE;
			ReadQuads(reader, numQuads, startQuadOffset, new Color(1f, 0.25f, 0f));
		}

		void ReadUnknownTable0x10(BinaryReader reader, int numEntries, int offset)
		{
			print("Number of Unknown Table Entries: " + numEntries);
			print("Unknown Table Offset: " + offset);

			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			for (var i = 0; i < numEntries; i++)
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

			for (var i = 0; i < numEntries; i++)
			{
				var objectOffset = offset + (OBJECT_ENTRY_SIZE * i);
				var fzObject = FZObjectOld.LoadObject(reader, objectOffset);
			}
		}

		// Unknown info about the object (first entry is in course03 0x0000EB8C)
		void ReadObjectExtraInfo(BinaryReader reader, int offset)
		{
			//print("Object Extra Info Offset: " + offset);

			// Store stream position
			var startOffset = reader.BaseStream.Position;

			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			reader.BaseStream.Seek(28, SeekOrigin.Current); // Skip 28 unknown/null bytes

			const int numAddresses = 33; // Number of 8 byte length + address pairs to check
			for (var i = 0; i < numAddresses; i++)
			{
				var extraSize = BinarySerializer.ReadInt32(reader);
				var extraOffset = BinarySerializer.ReadInt32(reader);
				if (extraOffset != 0)
					ReadObjectExtraItems(reader, extraSize, extraOffset);
			}

			// Restore stream position
			reader.BaseStream.Seek(startOffset, SeekOrigin.Begin);
		}

		// A list of unknown items related to the extra object info
		void ReadObjectExtraItems(BinaryReader reader, int numEntries, int offset)
		{
			//print(numEntries + " " + offset);

			// Store stream position
			var startOffset = reader.BaseStream.Position;

			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			for (var i = 0; i < numEntries; i++)
			{
				reader.BaseStream.Seek(8, SeekOrigin.Current); // Skip 8 unknown bytes

				var v = BinarySerializer.ReadVector(reader);
				//CreateVertex(v, new Vector3(5f, 5f, 5f), Color.green);
			}

			// Restore stream position
			reader.BaseStream.Seek(startOffset, SeekOrigin.Begin);
		}

		void ReadUnknownTable0x84(BinaryReader reader, int offset)
		{
			print("Unknown Table Offset: " + offset);

			const int numEntries = 36 / 3;
			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			for (var i = 0; i < numEntries; i++)
			{
				ReadTriangle(reader);
			}
		}

		public void SaveChanges()
		{
			var filePath = "Assets\\Output\\COLI_COURSE" + courseNumber.ToString("00") + ",lz";
			if (!File.Exists(filePath))
			{
				var inputFilePath = "Assets\\Input\\COLI_COURSE" + courseNumber.ToString("00") + ",lz";
				File.Copy(inputFilePath, filePath);
			}

			using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Open)))
			{
				// Save objects (untested)
				var objs = GameObject.FindGameObjectsWithTag("FZObject");
				for (var i = 0; i < objs.Length; i++)
				{
					var obj = objs[i].GetComponent<FZObjectOld>();
					FZObjectOld.WriteObject(writer, obj);
				}

				// Save splines
				var splines = GameObject.FindGameObjectsWithTag("FZSpline");
				for (var i = 0; i < splines.Length; i++)
				{
					var spline = splines[i].GetComponent<CheckpointOld>();

					// Checkpoint position?
					//spline.start += new Vector3(200f, 200f, 200f);
					//spline.end += new Vector3(200f, 200f, 200f);

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

					CheckpointOld.WriteSpline(writer, spline);
				}
			}

			print("Changes have been saved");
		}

		void ReadTriangles(BinaryReader reader, int numTriangles, int offset, Color color)
		{
			print("Number of Triangles: " + numTriangles);
			print("Triangles Offset: " + offset);

			reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to the start of the triangles list
			for (var i = 0; i < numTriangles; i++) // Each triangle entry is 88 bytes
			{
				reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip unknown data (might be related to checkpoint ids)

				// Read normal
				var n = BinarySerializer.ReadVector(reader);

				// Read the edges of the triangle
				var v1 = BinarySerializer.ReadVector(reader);
				DebugHelper.CreateVertex(v1);

				var v2 = BinarySerializer.ReadVector(reader);
				DebugHelper.CreateVertex(v2);
				Debug.DrawLine(v1, v2, color, 999f);

				var v3 = BinarySerializer.ReadVector(reader);
				DebugHelper.CreateVertex(v3);
				Debug.DrawLine(v2, v3, color, 999f);
				Debug.DrawLine(v3, v1, color, 999f);

				// Display normal
				var nPos = (v1 + v2 + v3) / 3f;
				Debug.DrawRay(nPos, n * 5f, Color.blue, 999f);

				reader.BaseStream.Seek(36, SeekOrigin.Current); // Skip unknown data
			}
		}

		// For testing purposes
		void ReadTriangle(BinaryReader reader)
		{
			var v1 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v1);

			var v2 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v2);
			Debug.DrawLine(v1, v2, Color.red, 999f);

			var v3 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v3);
			Debug.DrawLine(v2, v3, Color.red, 999f);
			Debug.DrawLine(v3, v1, Color.red, 999f);
		}

		void ReadQuads(BinaryReader reader, int numQuads, int offset, Color color)
		{
			print("Number of Quads: " + numQuads);
			print("Quads Offset: " + offset);

			reader.BaseStream.Seek(offset, SeekOrigin.Begin); // Go to the start of the quads list
			for (var i = 0; i < numQuads; i++) // Each quad entry is 112 bytes long
			{
				reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip unknown data (might be related to checkpoint ids)

				// Read normal
				var n = BinarySerializer.ReadVector(reader);

				// Read the edges of the quad
				var v1 = BinarySerializer.ReadVector(reader);
				DebugHelper.CreateVertex(v1);

				var v2 = BinarySerializer.ReadVector(reader);
				DebugHelper.CreateVertex(v2);
				Debug.DrawLine(v1, v2, color, 999f);

				var v3 = BinarySerializer.ReadVector(reader);
				DebugHelper.CreateVertex(v3);
				Debug.DrawLine(v2, v3, color, 999f);

				var v4 = BinarySerializer.ReadVector(reader);
				DebugHelper.CreateVertex(v4);
				Debug.DrawLine(v3, v4, color, 999f);
				Debug.DrawLine(v4, v1, color, 999f);

				// Display normal
				var nPos = (v1 + v2 + v3 + v4) / 4f;
				Debug.DrawRay(nPos, n * 5f, Color.blue, 999f);

				reader.BaseStream.Seek(48, SeekOrigin.Current); // Skip unknown data
			}
		}

		// For testing purposes
		void ReadQuad(BinaryReader reader)
		{
			var v1 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v1);

			var v2 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v2);
			Debug.DrawLine(v1, v2, Color.red, 999f);

			var v3 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v3);
			Debug.DrawLine(v2, v3, Color.red, 999f);

			var v4 = BinarySerializer.ReadVector(reader);
			DebugHelper.CreateVertex(v4);
			Debug.DrawLine(v3, v4, Color.red, 999f);
			Debug.DrawLine(v4, v1, Color.red, 999f);
		}
	}
}
