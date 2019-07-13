using CryptoTools.Common.FileSystems;
using CryptoTools.CryptoFiles.DataFiles;
using CryptoTools.CryptoFiles.Exceptions;
using CryptoTools.Cryptography.Exceptions;
using CryptoTools.Cryptography.Hashing;
using CryptoTools.Cryptography.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CryptoTools.CryptoFiles.UnitTests.DataFiles
{
	[TestClass]
	public class CryptoDataFileTests
	{
		[TestMethod]
		public void TestMethod1()
		{
		}

		[TestMethod]
		public void CryptoDataFile_BasicTest_EncryptedFile()
		{
			FileManager fileOps = new FileManager();
			Hasher hasher = new Hasher();

			// Create Credentials
			CryptoCredentials credentials = new CryptoCredentials { Passphrase = new CryptoString("My Passphrase") };
			CryptoCredentials wrongCredentials = new CryptoCredentials { Passphrase = new CryptoString("My WRONG Passphrase") };

			CryptoDataFile file1 = null;
			CryptoDataFile file2 = null;
			string dataFileName1 = "EncryptedTestDataFile1.dat";
			string dataFileName2 = "EncryptedTestDataFile2.dat";
			//byte[] testBytes = new ByteGenerator().GenerateBytes(1000);
			byte[] testBytes = new byte[] { 0x11, 0x22 };
			List<FileInfo> fileList = new List<FileInfo> { new FileInfo(dataFileName1), new FileInfo(dataFileName2) };

			fileOps.DeleteFile(dataFileName1);

			using (AutoDeleteFiles deleteFiles = new AutoDeleteFiles(fileList))
			{
				////////////////////////////////////////////////////////////
				// Write
				////////////////////////////////////////////////////////////
				try
				{
					// Write Content to data file
					file1 = new CryptoDataFile(dataFileName1);
					file1.EncryptFile = true; // This is the default
					file1.Credentials = credentials;
					file1.Content = testBytes;
					file1.Save();

				}
				catch (Exception ex)
				{
					Assert.Fail(ex.Message);
				}

				////////////////////////////////////////////////////////////
				// Read / Load
				////////////////////////////////////////////////////////////
				try
				{
					// Read & Load File
					file2 = new CryptoDataFile(dataFileName1);
					file2.Credentials = credentials;
					file2.EncryptFile = true;
					file2.Load();

					// Compare the files they should be exactly the same
					Assert.IsTrue(file1.Content.SequenceEqual(file2.Content));

				}
				catch (Exception ex)
				{
					// Delete Files
					Assert.Fail(ex.Message);
				}
				finally
				{
				}

				// THIS NEEDS REWORKING!
				////////////////////////////////////////////////////////////
				// Read / Load Wrong Password
				////////////////////////////////////////////////////////////
				try
				{
					// Read & Load File
					file1 = new CryptoDataFile(dataFileName1);
					file1.Credentials = wrongCredentials;
					file1.EncryptFile = true;
					file1.Load();

					Assert.Fail("Load should fail with wrong password");
				}
				catch (Exception ex)
				{
					Assert.IsTrue(true, ex.Message);
				}
			}
		}

		[TestMethod]
		public void CryptoDataFile_SaveLoadFromBytesTest()
		{
			string dataFileName = "testFile.bin";
			string fileContent = "My Test Content";

			// Save some data
			CryptoDataFile dataFile = new CryptoDataFile(dataFileName);
			byte[] content1 = Encoding.UTF8.GetBytes(fileContent);
			dataFile.Content = content1;
			dataFile.Save();
			byte[] fileBytes1 = dataFile.SaveToBytes();

			// Now load the data and compare
			CryptoDataFile dataLoader = new CryptoDataFile(dataFileName);
			dataLoader.Load();
			dataLoader.LoadFromBytes(fileBytes1);
			byte[] content2 = dataLoader.Content;


			Assert.IsTrue(content1.SequenceEqual(content2));
			Assert.IsTrue(dataFile.Content.SequenceEqual(dataLoader.Content));
		}

		[TestMethod]
		public void CryptoDataFile_EncryptedEntireFileTest()
		{
			string passphraseString = "My Passphrase";
			CryptoString passphrase = new CryptoString(CryptoString.StringToSecureString(passphraseString));

			CryptoCredentials credentials = new CryptoCredentials
			{
				Passphrase = passphrase,
			};

			string dataFileName = "EncryptedFile.bin";
			string fileContent = "My Test Content";


			using (AutoDeleteFiles autoDeleteFile = new AutoDeleteFiles(dataFileName))
			{
				// Save some data
				CryptoDataFile dataFile = new CryptoDataFile(dataFileName);
				byte[] content1 = Encoding.UTF8.GetBytes(fileContent);
				dataFile.Credentials = credentials;
				dataFile.EncryptFile = true;
				dataFile.Content = content1;
				dataFile.Save();
				byte[] fileBytes1 = dataFile.SaveToBytes();

				// Now load the data and compare
				CryptoDataFile dataLoader = new CryptoDataFile(dataFileName);
				dataLoader.Credentials = credentials;
				dataLoader.EncryptFile = true;
				dataLoader.Load();
				dataLoader.LoadFromBytes(fileBytes1);
				byte[] content2 = dataLoader.Content;


				Assert.IsTrue(content1.SequenceEqual(content2));
				Assert.IsTrue(dataFile.Content.SequenceEqual(dataLoader.Content));
			}
		}

		[TestMethod]
		public void CryptoDataFile_EncryptedEntireFileInvalidCredentialTest()
		{
			CryptoCredentials credentials = new CryptoCredentials
			{
				Passphrase = new CryptoString("My Passphrase"),
				Pin = 2222
			};
			CryptoCredentials credentialsWrongPassphrase = new CryptoCredentials
			{
				Passphrase = new CryptoString("My Wrong Passphrase"),
				Pin = 2222
			};
			CryptoCredentials credentialsWrongPin = new CryptoCredentials
			{
				Passphrase = new CryptoString("My Passphrase"),
				Pin = 1234
			};

			string dataFileName = "EncryptedFile.bin";
			string fileContent = "My Test Content";
			byte[] fileContentBytes = Encoding.UTF8.GetBytes(fileContent);


			using (AutoDeleteFiles autoDeleteFile = new AutoDeleteFiles(dataFileName))
			{
				// Save some data
				CryptoDataFile dataFile = new CryptoDataFile(dataFileName);
				dataFile.Credentials = credentials;
				dataFile.EncryptFile = true;
				dataFile.Content = fileContentBytes;
				dataFile.Save();

				try
				{
					// TEST WRONG PASSPHRASE
					CryptoDataFile dataFile2 = new CryptoDataFile(dataFileName);
					dataFile2.Credentials = credentialsWrongPassphrase;
					dataFile2.EncryptFile = true;
					dataFile2.Load();
				}
				catch (CryptoDecryptionException exception)
				{
					Assert.IsTrue(exception is CryptoDecryptionException);

				}
				catch (CryptoFileInvalidFormatException)
				{
					Assert.Fail("Test was not expecting an Exception");

				}
				catch (Exception)
				{
					Assert.Fail("Test was not expecting an Exception");
				}






				//Assert.IsTrue(dataFile.Content.SequenceEqual(dataFile2.Content));

			}
		}


		[TestMethod]
		public void CryptoDataFile_CustomOptions()
		{
			string dataFileName = "EncryptedFileCustomOptions.bin";
			string fileContent = "My Test Content";


			using (AutoDeleteFiles autoDeleteFile = new AutoDeleteFiles(dataFileName))
			{
				// Save some data
				CryptoDataFile dataFile = new CryptoDataFile(dataFileName);
				byte[] content1 = Encoding.UTF8.GetBytes(fileContent);
				dataFile.Content = content1;
				dataFile.Save();

				// Now load the data and compare
				CryptoDataFile dataLoader = new CryptoDataFile(dataFileName);
				dataLoader.Load();
				byte[] content2 = dataLoader.Content;


				Assert.IsTrue(content1.SequenceEqual(content2));
				Assert.IsTrue(dataFile.Content.SequenceEqual(dataLoader.Content));
			}
		}

		[TestMethod]
		public void CryptoDataFile_VersionNumbersTest()
		{
			FileManager fileOps = new FileManager();
			Hasher hasher = new Hasher();

			// Create Credentials
			CryptoCredentials credentials = new CryptoCredentials { Passphrase = new CryptoString("My Passphrase") };

			CryptoDataFile file1 = null;
			CryptoDataFile file2 = null;
			string dataFileName1 = "VersionDataFileTest.dat";
			byte[] testBytes = new byte[] { 0x11, 0x22 };

			fileOps.DeleteFile(dataFileName1);

			using (AutoDeleteFiles deleteFiles = new AutoDeleteFiles(dataFileName1))
			{
				////////////////////////////////////////////////////////////
				// Write
				////////////////////////////////////////////////////////////
				try
				{
					// Write Content to data file
					file1 = new CryptoDataFile(dataFileName1);

					file1.EncryptFile = true;
					file1.Credentials = credentials;
					file1.Content = testBytes;
					file1.Save();

				}
				catch (Exception ex)
				{
					Assert.Fail(ex.Message);
				}

				////////////////////////////////////////////////////////////
				// Read / Load
				////////////////////////////////////////////////////////////
				try
				{
					// Read & Load File
					file2 = new CryptoDataFile(dataFileName1);
					file2.Credentials = credentials;
					file2.EncryptFile = true;
					file2.Load();

					// Compare the files they should be exactly the same
					Assert.IsTrue(file1.Content.SequenceEqual(file2.Content));

				}
				catch (Exception ex)
				{
					// Delete Files
					Assert.Fail(ex.Message);
				}
				finally
				{
				}
			}
		}
	}
}

