using UnityEngine;
using System.Collections;

public class DebugHelper : MonoBehaviour
{
	static DebugHelper instance;
	public GameObject debugText;

	void Start()
	{
		instance = this;
	}

	// Used for visualizing vertices
	public static GameObject CreateVertex(Vector3 position)
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		obj.transform.position = position;
		obj.transform.localScale = new Vector3(2f, 2f, 2f);
		return obj;
	}

	public static GameObject CreateVertex(Vector3 position, Vector3 scale, Color color)
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		obj.transform.position = position;
		obj.transform.localScale = scale;
		obj.renderer.material.color = color;
		obj.name = "Test";
		return obj;
	}

	public static void DrawText(string text, Vector3 position)
	{
		GameObject obj = (GameObject)GameObject.Instantiate(instance.debugText);
		obj.transform.position = position;
		obj.GetComponent<TextMesh>().text = text;
	}
}
