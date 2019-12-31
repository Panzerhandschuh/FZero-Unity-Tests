using System.IO;
using UnityEngine;

namespace FZeroGXEditor.Config
{
	public static class ConfigLoader
	{
		private const string configPath = @"Assets\EditorConfig.json";

		public static EditorConfig LoadConfig()
		{
			var json = File.ReadAllText(configPath);
			return JsonUtility.FromJson<EditorConfig>(json);
		}
	}
}
