using FZeroGXEditor.Config;

namespace FZeroGXEditor.CourseEditor
{
	public interface ILoader
	{
		void Load(EditorConfig config);
		void Unload();
		void Save();
	}
}
