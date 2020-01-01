using System;
using System.Diagnostics;
using System.IO;

namespace FZeroGXEditor.Serialization
{
	public class GXPandLoader : IDisposable
	{
		private string gxRootOutputDir;
		private string unpackedFile;
		private Stream stream;

		private const string gxpandPath = @"Assets\Tools\gxpand.exe";

		/// <param name="inputFile">Path of input COLI_COURSE##.lz file</param>
		/// <param name="gxRootOutputDir">Root output directory</param>
		public GXPandLoader(string inputFile, string gxRootOutputDir)
		{
			this.gxRootOutputDir = gxRootOutputDir;

			var args = GetGXPandArgs(inputFile);
			var process = Process.Start(gxpandPath, args);
			process.WaitForExit();

			unpackedFile = unpackedFile.Replace('.', ',');
			stream = File.Open(unpackedFile, FileMode.Open);
		}

		private string GetGXPandArgs(string inputFile)
		{
			var inputFileName = Path.GetFileName(inputFile);
			unpackedFile = Path.Combine(Path.GetTempPath(), inputFileName);

			return $"unpack \"{inputFile}\" \"{unpackedFile}\"";
		}

		public Stream GetStream()
		{
			return stream;
		}

		public void Dispose()
		{
			MoveUnpackedFileToOutput();
			stream.Close();
		}

		private void MoveUnpackedFileToOutput()
		{
			var destinationDir = Path.Combine(gxRootOutputDir, "stage");
			Directory.CreateDirectory(destinationDir);

			// TODO: Re-pack file
			var unpackedFileName = Path.GetFileName(unpackedFile).Replace(',', '.');
			var destinationFile = Path.Combine(destinationDir, unpackedFileName);
			File.Move(unpackedFile, destinationFile);
		}
	}
}
