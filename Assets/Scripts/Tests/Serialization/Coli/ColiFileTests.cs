using FZeroGXEditor.Serialization;
using NUnit.Framework;
using System.IO;

namespace TurboForce.Tests.Serialization
{
	public class ColiFileTests// : FileStructureTest
	{
		private const string course01 = @"D:\Users\Tyler\Documents\FZeroTests\Input\COLI_COURSE03,lz";

		[Test]
		public void CanDeserializeColiFile()
		{
			using (var stream = File.Open(course01, FileMode.Open))
			using (var reader = new FZReader(stream))
			{
				var file = ColiFile.Deserialize(reader);
			}
		}
	}
}
