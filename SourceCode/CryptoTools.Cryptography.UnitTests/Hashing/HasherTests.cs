using CryptoTools.Common.FileSystems;
using CryptoTools.Cryptography.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace CryptoTools.Cryptography.UnitTests.Hashing
{
	[TestClass]
	public class HasherTests
	{

		[TestMethod]
		public void HasherBasicUsage()
		{
			// Arrange
			string message = "Hash Test";
			string hashSha256Expected = "a091c02972cf73649dcb7ef912ef9ee40071ced5b36cfccb0960e8fa2db3cb5a";

			// Act
			Hasher hasher = new Hasher();
			string hash = hasher.Hash(message);

			// Assert
			Assert.IsTrue(hash.Equals(hashSha256Expected));
		}

		[TestMethod]
		public void HasherBasicUsage2()
		{
			/*
			// Arrange
			string message = "Hash Test";
			string fileName = "testFile.bin";
			string directoryRoot = "TestFiles";
			string directoryRoot2 = "TestFiles2";


			//// Create Files and Directories
			List<DirectoryInfo> directories = new List<DirectoryInfo>()
			{
				new DirectoryInfo("TestFiles1"),
				new DirectoryInfo("TestFiles2")
			};
			List<FileInfo> files = new List<FileInfo>()
			{
				new FileInfo("TestFiles1"),
				new FileInfo("TestFiles2")
			};

			using (CreateAutoDeleteDirectory directory = new CreateAutoDeleteDirectory(directories))
			{
				using (CreateAutoDeleteFiles autoDeleteFiles = new CreateAutoDeleteFiles(files))
				{





				}
			}	

			// Create the default Hasher SHA256 & example of user defined algorithm 
			// ( You can use all the .Net Cryptography Hashers or write your own custom hashers)
			Hasher sha256 = new Hasher();
			Hasher sha512 = new Hasher(SHA512.Create());

			// Create Hashes that return a hexidecimal string fingerprint (Ex. a091c02972cf73649dcb7ef912ef9ee40071ced5b36cfccb0960e8fa2db3cb5a)
			string hashString = sha256.Hash(message);
			string hashBytes = sha256.Hash(new byte[] { 1, 2, 3 });
			string hashStream = sha256.Hash(File.OpenRead(fileName));
			string hashFile = sha256.HashFile(message);
			string hashMulti = sha256.MultiHash(message, 1000); // 1000 iterations

			// Create Hashes that return a byte array of the hash signature
			byte[] hashedBytes = sha256.HashToBytes(new byte[] { 1, 2, 3 });
			byte[] hashedFileStream = sha256.HashToBytes(File.OpenRead(fileName));
			byte[] hashedString = sha256.HashToBytes(message);
			
			// Takes approx 500ms ideal for adding complexity
			string hashSecure = sha256.SecureHash(message,500); 

			// Hashes every file in a directory and return a single hash
			string hashFullDirectory = sha256.HashDirectoryTree(directoryRoot);

			// Compares 2 diretories for Equality based on all file contents in directory
			bool match = sha256.CompareDirectoryHashSignatures(directoryRoot, directoryRoot2);
			*/

		}

		[TestMethod]
		public void HasherSecureHashUsage()
		{
			// Arrange
			string message = "Hash Test";
			int executionMillisecondsApprox = 100;
			string firstHash = "a091c02972cf73649dcb7ef912ef9ee40071ced5b36cfccb0960e8fa2db3cb5a";
			string hashExpected = firstHash;

			// Act
			Stopwatch sw = Stopwatch.StartNew();
			Hasher hasher = new Hasher();
			string hash = hasher.SecureHash(message, executionMillisecondsApprox);
			sw.Stop();
			Debug.WriteLine($"Approx execution time in milliseconds = {sw.ElapsedMilliseconds}");


			// Assert
			// No way to really test this without rewriting the Secure Algorithm
		}

		[TestMethod]
		public void HasherMultiHashUsage()
		{
			// Arrange
			string message = "Hash Test";
			int iterations = 20000;
			string firstHash = "a091c02972cf73649dcb7ef912ef9ee40071ced5b36cfccb0960e8fa2db3cb5a";
			string hashExpected = firstHash;
			for (int i = 0; i < iterations - 1; i++) hashExpected = new Hasher().Hash(hashExpected);

			// Act
			Hasher hasher = new Hasher();
			string hash = hasher.MultiHash(message, iterations);

			// Assert
			Assert.IsTrue(hash.Equals(hashExpected));
		}


		[TestMethod]
		public void HasherCreateHashesUsingVariousAlgorithms()
		{
			// Online Hash Generator could be useful here
			// https://www.browserling.com/tools/all-hashes			

			string message = "Hash Test";
			string hashSha1Expected = "b42616bc4884db35e6f24a5803b3c65333fd4bd5";
			string hashSha256Expected = "a091c02972cf73649dcb7ef912ef9ee40071ced5b36cfccb0960e8fa2db3cb5a";
			string hashSha512Expected = "9f90094c4c2e35be90956a1f27a423a273800d8ae8568171ef7b6ec8c1ea59c464d4381e0d6e189c8887c21858da1b8a49aa742e06426be0ab24232ee0b7f463";
			//string hashRipeMd160Expected = "6f49ea6fa331cdfbbc189faf7cba02813da48b4a";
			string hash = null;

			//  Sha1 Test
			Hasher sha1 = new Hasher(SHA1.Create());
			hash = sha1.Hash(message);
			Assert.IsTrue(hash.Equals(hashSha1Expected));

			//  Sha256 Test
			Hasher sha256 = new Hasher(SHA256.Create());
			hash = sha256.Hash(message);
			Assert.IsTrue(hash.Equals(hashSha256Expected));

			//  Sha512 Test
			Hasher sha512 = new Hasher(SHA512.Create());
			hash = sha512.Hash(message);
			Assert.IsTrue(hash.Equals(hashSha512Expected));

			// REMOVED to make .net core 2.0 compliant
			//  Ripe Md 160 Test
			//Hasher ripeMd160 = new Hasher(RIPEMD160.Create());
			//hash = ripeMd160.Hash(message);
			//Assert.IsTrue(hash.Equals(hashRipeMd160Expected));		
		}


		[TestMethod]
		public void HasherSetApplicationGlobalOptionsOnStartUp()
		{
			const string SaltApplicationGlobal = "AppSalt";

			// Set Gobal Options
			GlobalCryptographyOptions.HasherOptions.Salt = SaltApplicationGlobal;
			GlobalCryptographyOptions.HasherOptions.HashFingerprintLowercase = true;

			// Create Instance based on Application Global options
			Hasher hasher1 = new Hasher();
			Assert.IsTrue(hasher1.Options.Salt == SaltApplicationGlobal);
		}

		[TestMethod]
		public void HasherSetInstanceOptions()
		{
			const string SaltInstance = "InstanceSalt";

			// Create Instance based on Instance options
			Hasher hasher2 = new Hasher(new HasherOptions
			{
				Salt = SaltInstance

			});
			Assert.IsTrue(hasher2.Options.Salt == SaltInstance);

		}

		[TestMethod]
		public void HasherCalculateFileHash()
		{
			FileManager fileManager = new FileManager();
			Hasher hasher = new Hasher();
			int fileSize = 10000000;
			string fileName = "TestFile.dat";

			try
			{
				// Create a test file
				FileGenerator generator = new FileGenerator();
				generator.GenerateRandomBinaryFile(fileName, fileSize);

				Assert.IsTrue(fileManager.FileExists(fileName));
				Assert.IsTrue(fileManager.GetFileInfo(fileName).Length == fileSize);

				string hash = hasher.HashFile(fileName);

				byte[] fileBytes = File.ReadAllBytes(fileName);
				string hashBytes = hasher.Hash(fileBytes);

				Assert.IsTrue(hash.Equals(hashBytes));

			}
			finally
			{
				fileManager.DeleteFile(fileName);
			}
		}
	}
}
