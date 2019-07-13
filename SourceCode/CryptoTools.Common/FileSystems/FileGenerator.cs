using CryptoTools.Common.Utils;
using System.Collections.Generic;
using System.IO;

namespace CryptoTools.Common.FileSystems
{
	/// <summary>
	/// Generates Random Files. This is useful for creating dummy data and files for obsfucation or various unit testing scenarios.
	/// </summary>
	public class FileGenerator
	{
		public IList<string> GenerateRandomFiles(string directoryRoot = "TestData", int subFolderCount = 2, int filesInFolderCount = 2)
		{
			List<string> files = new List<string>();

			string fileNamePrefix = "file";
			string fileExt = "txt";

			for (int i = 0; i < subFolderCount; i++)
			{

				string fullFolder = Path.Combine(directoryRoot, $"folder{i}");
				Directory.CreateDirectory(fullFolder);

				for (int j = 0; j < filesInFolderCount; j++)
				{
					string fileName = $"{fileNamePrefix}{j}.{fileExt}";
					string fullFileName = Path.Combine(fullFolder, fileName);
					string text = GuidUtils.NewGuid().ToString();
					File.AppendAllText(fullFileName, text);

					files.Add(fullFileName);
				}
			}

			return files;

		}

		public void GenerateRandomBinaryFile(string fileName, int size)
		{
			// Generate a Blob of Data
			ByteGenerator blobGenerator = new ByteGenerator();
			byte[] blob = blobGenerator.GenerateBytes(size);


			FileManager fileManager = new FileManager();
			fileManager.CreateWriteBytes(fileName, blob);


		}
	}
}
