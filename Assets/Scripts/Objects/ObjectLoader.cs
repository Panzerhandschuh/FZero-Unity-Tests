using System;
using System.Collections.Generic;
using UnityEngine;

namespace FZeroGXEditor.Objects
{
	public class ObjectLoader : MonoBehaviour
	{
		public GameObject checkpointPrefab;
		public GameObject fzObjectPrefab;

		private Dictionary<Type, GameObject> objectMap;

		private void Awake()
		{
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
