using CryptoTools.Common.FileSystems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace CryptoTools.Common.UnitTests.FileSystems
{
	[TestClass]
	public class CreateAutoDeleteFilesTests
	{
		[TestMethod]
		public void CreateAutoDeleteDirectory_BasicUsage()
		{
			FileManager fileMan = new FileManager();
			const string SubDirectory = "TestFile";

			// Create Directory
			using (CreateAutoDeleteDirectory directory = new CreateAutoDeleteDirectory(SubDirectory))
			{
				Assert.IsTrue(fileMan.DirectoryExists(SubDirectory));
			}
			// Make sure it has been deleted
			Assert.IsTrue(!fileMan.DirectoryExists(SubDirectory));

			// Create Multiple Directories
			List<DirectoryInfo> directories = new List<DirectoryInfo>()
			{
				new DirectoryInfo("dir1"),
				new DirectoryInfo("dir2"),
				new DirectoryInfo("dir3"),
				new DirectoryInfo("dir4"),
				new DirectoryInfo("dir5"),
				new DirectoryInfo("dir6"),
				new DirectoryInfo("dir7"),
				new DirectoryInfo("dir8"),
				new DirectoryInfo("dir91"),
				new DirectoryInfo("dir10")
			};

			using (CreateAutoDeleteDirectory directory = new CreateAutoDeleteDirectory(directories))
			{
				Assert.IsTrue(fileMan.AllDirectoriesExists(directories));
			}
			// Make sure it has been deleted
			Assert.IsTrue(!fileMan.DirectoryExists(SubDirectory));
		}




		[TestMethod]
		public void CreateAutoDeleteFiles_BasicUsage()
		{
			FileManager fileMan = new FileManager();
			ByteGenerator byteGenerator = new ByteGenerator();
			IList<FileInfo> files = new List<FileInfo>();
			const int FileCount = 20;
			const string SubDirectory = "TestFile";

			// Define a bunch of Files
			for (int i = 0; i < FileCount; i++)
			{
				FileInfo fileInfo = new FileInfo(Path.Combine(SubDirectory, $"test{i}.dat"));
				//File.WriteAllBytes(fileInfo.FullName, byteGenerator.GenerateBytes(100));
				files.Add(fileInfo);
			}

			// 1). Create Files
			using (CreateAutoDeleteDirectory directory = new CreateAutoDeleteDirectory(SubDirectory))
			{
				using (CreateAutoDeleteFiles file = new CreateAutoDeleteFiles(files))
				{
					// Make sure they exists
					Assert.IsTrue(fileMan.AllFilesExists(files));
				}
			}
			// Make sure they dont exist and they have been deleted
			Assert.IsTrue(fileMan.AllFilesDoNotExist(files));

			// 2.) Create Files With Content
			using (CreateAutoDeleteDirectory directory = new CreateAutoDeleteDirectory(SubDirectory))
			{
				using (CreateAutoDeleteFiles file = new CreateAutoDeleteFiles(files, true, 200))
				{
					// Make sure they exists
					Assert.IsTrue(fileMan.AllFilesExists(files));

					// Check to see if the files are there with the same size
					foreach (FileInfo info in files)
					{
						Assert.IsTrue(info.Length == 200);
					}
				}
			}
			// Make sure they dont exist and they have been deleted
			Assert.IsTrue(fileMan.AllFilesDoNotExist(files));

		}



	}
}
