using FZeroGXEditor.Config;
using UnityEngine;

namespace FZeroGXEditor.CourseEditor
{
	public class CourseLoader : MonoBehaviour
	{
		public CollisionLoader collisionLoader;
		public EditorConfig config;

		private void Awake()
		{
			config = ConfigLoader.LoadConfig();
		}

		public void Load()
		{
			ConfigLoader.SaveConfig(config);

			collisionLoader.Load(config);
		}

		public void Unload()
		{
			collisionLoader.Unload();
		}

		public void Save()
		{
			collisionLoader.Save();
		}
	}
}
