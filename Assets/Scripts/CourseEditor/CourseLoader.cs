using FZeroGXEditor.Config;
using FZeroGXEditor.Serialization;
using System.IO;
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

			var coliFile = LoadColiFile();
		}

		private ColiFile LoadColiFile()
		{
			var coursePath = GetCoursePath();

			//using (var stream = File.OpenRead(@"D:\Users\Tyler\Documents\Tools\F-Zero GX\gxpand\output\COLI_COURSE03,lz"))
			using (var loader = new GXPandLoader(coursePath, config.gxRootOutputDir))
			using (var reader = new FZReader(loader.GetStream()))
			{
				return ColiFile.Deserialize(reader);
			}
		}

		private string GetCoursePath()
		{
			var courseNumberStr = ((int)config.course).ToString("D2");
			return $@"{config.gxRootInputDir}\stage\COLI_COURSE{courseNumberStr}.lz";
		}
	}
}
