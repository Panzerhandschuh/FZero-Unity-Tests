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

			GUILayout.BeginHorizontal();

			var courseLoader = (CourseLoader)target;
			if (GUILayout.Button("Load Course"))
				courseLoader.LoadCourse();
			else if (GUILayout.Button("Save Course"))
				courseLoader.SaveCourse();

			GUILayout.EndHorizontal();
		}
	}
}
