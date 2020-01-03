using UnityEngine;

namespace FZeroGXEditor.CourseEditor
{
	public class CourseLoader : MonoBehaviour
	{
		public CollisionLoader collisionLoader;
		public Course course;

		public void Load()
		{
			collisionLoader.Load(course);
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
