using CryptoTools.Common.FileSystems;
using CryptoTools.Cryptography.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;

namespace CryptoTools.Cryptography.UnitTests.Utils
{
	[TestClass]
	public class CryptoBlobTests
	{
		[TestMethod]
		public void CryptoBlob_BasicUsage()
		{
			try
			{
				byte[] bytes = new ByteGenerator().GenerateBytes(10);

				// Create Credentials
				CryptoCredentials credentials = new CryptoCredentials
				{
					Passphrase = new CryptoString("My Passphrase"),
					Pin = 2222
				};

				// Create the Blob object and assign Encrypt some Bytes
				CryptoBlob blob = new CryptoBlob(credentials, bytes);

				// Retrieve the Decrypted Bytes
				byte[] decryptedBytes = blob.Decrypt();

				// Get the Encrypted Data - Perhaps you want to store it in a Database for example
				byte[] encryptedBytes = blob.GetEncryptedBytes();

				// Set the Encrypted Bytes - Perhaps you pulled them from a Database or in another file
				blob.SetEncryptedBytes(encryptedBytes);

				// Validates the Checksum of the blob and throws an exception if the Blob fails the integrity check
				blob.ValidateChecksum();

			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				throw;
			}
		}

		[TestMethod]
		public void CryptoBlob_AdvancedUsage()
		{
			try
			{
				//byte[] bytes = new ByteGenerator().GenerateBytes(10);
				byte[] bytes = new byte[] { 0x00, 0x00 };

				// Create Credentials
				CryptoCredentials credentials = new CryptoCredentials
				{
					Passphrase = new CryptoString("My Passphrase"),
					Pin = 2222
				};

				CryptoBlob blob = new CryptoBlob(credentials, bytes);
				byte[] encryptedBytes = blob.GetEncryptedBytes();
				blob.SetEncryptedBytes(encryptedBytes);
				byte[] decryptedBytes = blob.Decrypt(true);


				CryptoBlob blob2 = new CryptoBlob(credentials, decryptedBytes);
				byte[] encryptedBytes2 = blob2.GetEncryptedBytes();
				byte[] decryptedBytes2 = blob.Decrypt(true);

				bool diff = encryptedBytes.SequenceEqual(encryptedBytes2);


				//byte[] decryptedBytes = blob.Decrypt();
				//byte[] encryptedBytes = blob.GetEncryptedBytes();
				//blob.SetEncryptedBytes(encryptedBytes);

				// Validates the Checksum of the blob and throws an exception if the Blob fails the integrity check
				blob.ValidateChecksum();

			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				throw;
			}
		}

		[TestMethod]
		public void CryptoBlob_StressTest()
		{


			int iterations = 200;
			int blocksize = 100;

			byte[] bytes = new ByteGenerator().GenerateBytes(blocksize);


			for (int i = 0; i < iterations; i++)
			{
				try
				{
					// Create Credentials
					CryptoCredentials credentials = new CryptoCredentials
					{
						Passphrase = new CryptoString("My Passphrase"),
						Pin = 2222
					};

					// Create the Blob object and assign Encrypt some Bytes
					CryptoBlob blob = new CryptoBlob(credentials, bytes);

					// Retrieve the Decrypted Bytes
					byte[] decryptedBytes = blob.Decrypt();

					// Get the Encrypted Data - Perhaps you want to store it in a Database for example
					byte[] encryptedBytes = blob.GetEncryptedBytes();

					// Set the Encrypted Bytes - Perhaps you pulled them from a Database or in another file
					blob.SetEncryptedBytes(encryptedBytes);

					// Validates the Checksum of the blob and throws an exception if the Blob fails the integrity check
					blob.ValidateChecksum();

					Assert.IsTrue(decryptedBytes.SequenceEqual(bytes));

				}
				catch (Exception e)
				{
					Debug.WriteLine(e.Message);
					throw;
				}
			}

		}
	}
}
