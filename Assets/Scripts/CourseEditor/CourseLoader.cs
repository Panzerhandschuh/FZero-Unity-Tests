using System;
using FZeroGXEditor.Config;
using FZeroGXEditor.Objects;
using FZeroGXEditor.Serialization;
using FZeroGXEditor.Utilities;
using UnityEngine;

namespace FZeroGXEditor.CourseEditor
{
	public class CourseLoader : MonoBehaviour
	{
		public EditorConfig config;

		private void Awake()
		{
			config = ConfigLoader.LoadConfig();
		}

		public void LoadCourse()
		{
			ConfigLoader.SaveConfig(config);

			UnloadCourse();

			var coliFile = LoadColiFile();
			LoadObjects(coliFile);
		}

		public void UnloadCourse()
		{
			transform.DestroyChildren();
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
			LoadObjectCollisions(coliFile.objectCollisionTable);
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

		private void LoadObjectCollisions(ObjectCollisionTable objectCollisionTable)
		{
			foreach (var entry in objectCollisionTable.objectCollisions)
			{
				var obj = CreateObject(typeof(ObjectCollision));
				obj.GetComponent<ObjectCollision>().Init(entry);
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
			obj.transform.parent = transform;

			return obj;
		}

		private string GetCoursePath()
		{
			var courseNumberStr = ((int)config.course).ToString("D2");
			return $@"{config.gxRootInputDir}\stage\COLI_COURSE{courseNumberStr}.lz";
		}
	}
}
