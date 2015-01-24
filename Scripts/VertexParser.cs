using UnityEngine;
using System.IO;

public class VertexParser : MonoBehaviour
{
	const int TRI_CHUNK_SIZE = 22;
	const int QUAD_CHUNK_SIZE = 28;

	void Start()
	{
		string quadFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\" + "quads.txt";
		ReadQuadCollisions(quadFile);

		//string triangleFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\" + "tris.txt";
		//ReadTriangleCollisions(triangleFile);
	}

	void ReadTriangleCollisions(string filePath)
	{
		int numLines = GetNumLines(filePath);
		int numChunks = numLines / TRI_CHUNK_SIZE;
		print("Num Chunks: " + numChunks);
		using (StreamReader reader = new StreamReader(File.Open(filePath, FileMode.Open)))
		{
			// Parse all chunks
			for (int i = 0; i < numChunks; i++)
			{
				SkipLines(reader, 4); // Skip 4 lines (unknown and normal)

				Vector3 v1 = ReadVector(reader);
				CreateVertex(v1);

				Vector3 v2 = ReadVector(reader);
				CreateVertex(v2);
				Debug.DrawLine(v1, v2, Color.red, 999f);

				Vector3 v3 = ReadVector(reader);
				CreateVertex(v3);
				Debug.DrawLine(v2, v3, Color.red, 999f);
				Debug.DrawLine(v3, v1, Color.red, 999f);

				SkipLines(reader, 9); // Skip 8 lines (unknown content)
			}
		}
	}

	void ReadQuadCollisions(string filePath)
	{
		int numLines = GetNumLines(filePath);
		int numChunks = numLines / QUAD_CHUNK_SIZE;
		print("Num Chunks: " + numChunks);
		using (StreamReader reader = new StreamReader(File.Open(filePath, FileMode.Open)))
		{
			// Parse all chunks
			for (int i = 0; i < numChunks; i++)
			{
				SkipLines(reader, 4); // Skip 4 lines (unknown and normal)

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

				SkipLines(reader, 12); // Skip 12 lines (unknown content)
			}
		}
	}

	void CreateVertex(Vector3 position)
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		obj.transform.position = position;
	}

	// Returns the number of lines in a text file
	int GetNumLines(string filePath)
	{
		var numLines = 0;
		using (var reader = File.OpenText(filePath))
		{
			while (reader.ReadLine() != null)
			{
				numLines++;
			}
		}

		return numLines;
	}

	Vector3 ReadVector(StreamReader reader)
	{
		return new Vector3(float.Parse(reader.ReadLine()), float.Parse(reader.ReadLine()), float.Parse(reader.ReadLine()));
	}

	void SkipLines(StreamReader reader, int numLines)
	{
		for (int j = 0; j < numLines; j++)
			reader.ReadLine();
	}
}
