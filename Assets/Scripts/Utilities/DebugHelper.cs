using UnityEngine;

namespace FZeroGXEditor.Utilities
{
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
			var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obj.transform.position = position;
			return obj;
		}

		public static GameObject CreateVertex(Vector3 position, Vector3 scale, Color color)
		{
			var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obj.transform.position = position;
			obj.transform.localScale = scale;
			obj.GetComponent<Renderer>().material.color = color;
			obj.name = "Test";
			return obj;
		}

		public static void DrawText(string text, Vector3 position)
		{
			var obj = (GameObject)GameObject.Instantiate(instance.debugText);
			obj.transform.position = position;
			obj.GetComponent<TextMesh>().text = text;
		}
	}
}
