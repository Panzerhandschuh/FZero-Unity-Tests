namespace FZeroGXEditor.CourseEditor
{
	public interface ILoader
	{
		void Load(Course course);
		void Unload();
		void Save();
	}
}
