using System;
using System.Collections.Generic;
using UnityEngine;

namespace FZeroGXEditor.Objects
{
	[ExecuteInEditMode]
	public class ObjectLoader : MonoBehaviour
	{
		public static ObjectLoader instance;

		public GameObject checkpointPrefab;
		public GameObject fzObjectPrefab;

		private Dictionary<Type, GameObject> objectMap;

		private void Awake()
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
