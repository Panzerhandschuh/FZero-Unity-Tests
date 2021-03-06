﻿using System;
using FZeroGXEditor.Objects;
using FZeroGXTools.Serialization;
using FZeroGXEditor.Utilities;
using UnityEngine;
using System.IO;

namespace FZeroGXEditor.CourseEditor
{
	public class CollisionLoader : MonoBehaviour, ILoader
	{
		private Course course;
		private ColiFile coliFile;

		public void Load(Course course)
		{
			this.course = course;

			Unload();

			coliFile = LoadColiFile();
			LoadCollisionData(coliFile);
		}

		public void Unload()
		{
			transform.DestroyChildren();
		}

		private ColiFile LoadColiFile()
		{
			var coursePath = GetCoursePath();

			using (var loader = new LZUnpacker(coursePath))
			using (var reader = new FZReader(loader.GetStream()))
			{
				return ColiFile.Deserialize(reader);
			}
		}

		private void LoadCollisionData(ColiFile coliFile)
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

		public void Save()
		{
			var coursePath = GetCoursePath();
			using (var stream = File.OpenWrite(coursePath))
			using (var writer = new FZWriter(stream))
			{
				writer.Write(coliFile);
			}
			
			LZPacker.Pack(coursePath);
		}

		private string GetCoursePath()
		{
			var courseNumberStr = ((int)course).ToString("D2");
			return $@"{EditorConstants.gameDataDir}\stage\COLI_COURSE{courseNumberStr}.lz";
		}
	}
}
