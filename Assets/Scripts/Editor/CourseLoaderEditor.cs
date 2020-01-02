using FZeroGXEditor.CourseEditor;
using UnityEditor;
using UnityEngine;

namespace FZeroGXEditor.EditorScripts
{
	[CustomEditor(typeof(CourseLoader))]
	public class CourseLoaderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var courseLoader = (CourseLoader)target;
			if (GUILayout.Button("Load Course"))
				courseLoader.LoadCourse();
			else if (GUILayout.Button("Unload Course"))
				courseLoader.UnloadCourse();
		}
	}
}
