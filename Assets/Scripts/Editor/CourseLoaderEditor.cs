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

			if (GUILayout.Button("Load Course"))
			{
				var courseLoader = (CourseLoader)target;
				courseLoader.LoadCourse();
			}
		}
	}
}
