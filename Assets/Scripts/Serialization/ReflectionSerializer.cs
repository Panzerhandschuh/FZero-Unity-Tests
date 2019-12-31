using System;
using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class ReflectionSerializer
	{
		public void Serialize(BinaryReader reader, Type type, object instance)
		{
			var fields = type.GetFields();
			foreach (var field in fields)
			{
				field.GetValue(instance);
			}
		}
	}
}
