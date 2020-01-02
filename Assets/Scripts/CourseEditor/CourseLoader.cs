using System;
using FZeroGXEditor.Config;
using FZeroGXEditor.Objects;
using FZeroGXEditor.Serialization;
using UnityEngine;

namespace FZeroGXEditor.CourseEditor
{
	public class CourseLoader : MonoBehaviour
	{
		public EditorConfig config;

		private GameObject courseParent;

		private void Awake()
		{
			config = ConfigLoader.LoadConfig();
		}

		public void LoadCourse()
		{
			ConfigLoader.SaveConfig(config);

			UnloadCourse();
			CreateCourseParent();

			var coliFile = LoadColiFile();
			LoadObjects(coliFile);
		}

		private void CreateCourseParent()
		{
			courseParent = new GameObject();
			courseParent.name = "Course";
		}

		public void UnloadCourse()
		{
			if (courseParent != null)
				DestroyImmediate(courseParent);
		}

		private ColiFile LoadColiFile()
		{
			var coursePath = GetCoursePath();

			using (var loader = new GXPandLoader(coursePath, config.gxRootOutputDir))
			using (var reader = new FZReader(loader.GetStream()))
			{
				return ColiFile.Deserialize(reader);
			}
		}

		private void LoadObjects(ColiFile coliFile)
		{
			LoadCheckpoints(coliFile.checkpointTable);
			LoadFZObjects(coliFile.objectTable);
		}

		private void LoadCheckpoints(CheckpointTable checkpointTable)
		{
			foreach (var checkpointEntry in checkpointTable.checkpointEntries)
			{
				var obj = CreateObject(typeof(Checkpoint));
				obj.GetComponent<Checkpoint>().Init(checkpointEntry.checkpoint);
			}
		}

		private void LoadFZObjects(ObjectTable objectTable)
		{
			foreach (var objectData in objectTable.objects)
			{
				var obj = CreateObject(typeof(FZObject));
				obj.GetComponent<FZObject>().Init(objectData);
			}
		}

		private GameObject CreateObject(Type type)
		{
			var obj = ObjectLoader.instance.CreateObject(type);
			obj.transform.parent = courseParent.transform;

			return obj;
		}

		private string GetCoursePath()
		{
			var courseNumberStr = ((int)config.course).ToString("D2");
			return $@"{config.gxRootInputDir}\stage\COLI_COURSE{courseNumberStr}.lz";
		}
	}
}
