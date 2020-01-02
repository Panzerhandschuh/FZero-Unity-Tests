using System;
using System.Collections.Generic;
using UnityEngine;

namespace FZeroGXEditor.Objects
{
	[ExecuteInEditMode]
	public class ObjectLoader : MonoBehaviour
	{
		private static ObjectLoader instance;
		public static ObjectLoader Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<ObjectLoader>();
					instance.Init();
				}

				return instance;
			}
		}

		public GameObject checkpointPrefab;
		public GameObject fzObjectPrefab;

		private Dictionary<Type, GameObject> objectMap;

		private void Init()
		{
			instance = this;

			objectMap = new Dictionary<Type, GameObject>();
			objectMap.Add(typeof(Checkpoint), checkpointPrefab);
			objectMap.Add(typeof(FZObject), fzObjectPrefab);
		}

		public GameObject CreateObject(Type type)
		{
			var prefab = objectMap[type];
			return Instantiate(prefab);
		}
	}
}
