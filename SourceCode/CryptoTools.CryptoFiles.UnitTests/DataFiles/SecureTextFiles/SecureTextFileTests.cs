using CryptoTools.Common.FileSystems;
using CryptoTools.Common.Utils;
using CryptoTools.CryptoFiles.DataFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CryptoTools.CryptoFiles.UnitTests.DataFiles.SecureTextFiles
{
	[TestClass]
	public class SecureTextFileTests
	{
		[TestMethod]
		public void SecureTextFile_BasicUsage()
		{
			/////////////////////////////////////////////////////////////////////
			// Arrange
			/////////////////////////////////////////////////////////////////////
			string fileName = "SecureTextFile.txt";
			SecureTextFile file = new SecureTextFile(fileName);
			string text = "TEST" + Environment.NewLine + "AGAIN" + Environment.NewLine + Environment.NewLine; //new TextGenerator() { ApproxLineLength = 25 }.GenerateText(20000);
																											  //string text = new TextGenerator() { ApproxLineLength = 25 }.GenerateText(20000);


			/////////////////////////////////////////////////////////////////////
			// Act
			/////////////////////////////////////////////////////////////////////

			// Write & Read ALL Text
			string textIn = text;
			file.WriteAllText(textIn);
			File.WriteAllText("filetest.txt", textIn);
			string textResult = file.ReadAllText();
			string fileTextResult = File.ReadAllText("filetest.txt");


			// Write & Read Text Lines
			IList<string> lines = StringUtils.StringToStringArray(text).ToList();
			file.WriteAllLines(lines);
			File.WriteAllLines("filetest2.txt", lines);
			IList<string> readlines = file.ReadAllLines();
			IList<string> readlines2 = File.ReadAllLines("filetest2.txt");


			/////////////////////////////////////////////////////////////////////
			// Assert
			/////////////////////////////////////////////////////////////////////
			Assert.IsTrue(lines.SequenceEqual(readlines));
			Assert.IsTrue(textIn.Equals(textResult));

		}

		[TestMethod]
		public void SecureTextFile_ManyIterations_StressTest()
		{
			/////////////////////////////////////////////////////////////////////
			// Arrange
			/////////////////////////////////////////////////////////////////////
			string fileName = "SecureTextFile.dat";
			int iterations = 100;
			FileManager fileMan = new FileManager();
			string text = new TextGenerator() { ApproxLineLength = 25 }.GenerateText(100);

			/////////////////////////////////////////////////////////////////////
			// Act
			/////////////////////////////////////////////////////////////////////
			for (int i = 0; i < iterations; i++)
			{
				SecureTextFile file = new SecureTextFile(fileName);

				// Write & Read ALL Text
				string textIn = text;
				file.WriteAllText(textIn);
				string textResult = file.ReadAllText();

				// Write & Read Text Lines
				IList<string> lines = StringUtils.StringToStringArray(text).ToList();
				file.WriteAllLines(lines);
				IList<string> readlines = file.ReadAllLines();

				/////////////////////////////////////////////////////////////////////
				// Assert
				/////////////////////////////////////////////////////////////////////
				Assert.IsTrue(lines.SequenceEqual(readlines));
				Assert.IsTrue(textIn.Equals(textResult));

				fileMan.DeleteFile(fileName);
			}
		}

		[TestMethod]
		public void SecureTextFile_LargeFileSize_StressTest()
		{
			/////////////////////////////////////////////////////////////////////
			// Arrange
			/////////////////////////////////////////////////////////////////////
			string fileName = "SecureTextFile.dat";
			int iterations = 3;
			int fileSize = 10000;
			FileManager fileMan = new FileManager();
			string text = new TextGenerator() { ApproxLineLength = 25 }.GenerateText(fileSize);

			/////////////////////////////////////////////////////////////////////
			// Act
			/////////////////////////////////////////////////////////////////////
			for (int i = 0; i < iterations; i++)
			{
				SecureTextFile file = new SecureTextFile(fileName);

				// Write & Read ALL Text
				string textIn = text;
				file.WriteAllText(textIn);
				string textResult = file.ReadAllText();

				// Write & Read Text Lines
				IList<string> lines = StringUtils.StringToStringArray(text).ToList();
				file.WriteAllLines(lines);
				IList<string> readlines = file.ReadAllLines();

				/////////////////////////////////////////////////////////////////////
				// Assert
				/////////////////////////////////////////////////////////////////////
				Assert.IsTrue(lines.SequenceEqual(readlines));
				Assert.IsTrue(textIn.Equals(textResult));

				fileMan.DeleteFile(fileName);
			}
		}


		[TestMethod]
		public void SecureTextFile_WriterReader_BasicUsage()
		{
			#region TODO
			//string[] lines = { "First line", "Second line", "Third line" };

			//// Create File
			//string fileName = "SecureTextFile.txt";
			//SecureTextFile file = new SecureTextFile(fileName);


			//// Write Text

			//// Ex. 1
			//file.WriteAllLines(lines);

			//// Ex. 2
			//file.WriteAllText(text);

			//// Ex. 3 Writes Lines
			//using (SecureTextFileWriter writer = new SecureTextFileWriter(fileName))
			//{
			//	foreach (string line in lines)
			//	{
			//		writer.Write("Word ");
			//		writer.WriteLine(line);
			//	}
			//}

			//// Ex. 3 Append Writes Lines
			//using (SecureTextFileWriter writer = new SecureTextFileWriter(fileName))
			//{
			//	foreach (string line in lines)
			//	{
			//		writer.AppendLine(line);
			//	}
			//}

			//// 4. Read Text
			//string readText = file.ReadAllLines();

			//// 5. Read All Tet
			//string readText = file.ReadAllText();

			//// Ex. 4 Read Lines
			//using (SecureTextFileReader reader = new SecureTextFileReader(fileName))
			//{
			//	foreach (string line in lines)
			//	{
			//		string line = reader.ReadLine();
			//	}
			#endregion
		}
	}
}
