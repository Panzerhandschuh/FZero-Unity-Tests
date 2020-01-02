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
			LoadMeshCollisions(coliFile.meshCollisionTable);
		}

		private void LoadCheckpoints(CheckpointTable checkpointTable)
		{
			foreach (var entry in checkpointTable.checkpointEntries)
			{
				var obj = CreateObject(typeof(Checkpoint));
				obj.GetComponent<Checkpoint>().Init(entry.checkpoint);
			}
		}

		private void LoadFZObjects(ObjectTable objectTable)
		{
			foreach (var entry in objectTable.objects)
			{
				var obj = CreateObject(typeof(FZObject));
				obj.GetComponent<FZObject>().Init(entry);
			}
		}

		private void LoadMeshCollisions(MeshCollisionTable meshCollisionTable)
		{
			foreach (var entry in meshCollisionTable.meshCollisionEntries)
			{
				if (entry.meshCollision != null)
				{
					var obj = CreateObject(typeof(MeshCollision));
					obj.GetComponent<MeshCollision>().Init(entry.meshCollision);
				}
			}
		}

		private GameObject CreateObject(Type type)
		{
			var obj = ObjectLoader.Instance.CreateObject(type);
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
