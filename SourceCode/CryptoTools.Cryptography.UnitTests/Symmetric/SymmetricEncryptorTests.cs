using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoTools.Cryptography.Symmetric;
using CryptoTools.Common.FileSystems;
using System.Linq;
using System.Security.Cryptography;
using CryptoTools.Cryptography.Utils;
using System.Diagnostics;
using CryptoTools.Cryptography.Exceptions;
using CryptoTools.Cryptography.Hashing;
using CryptoTools.Common.Utils;

namespace CryptoTools.Cryptography.UnitTests.Symmetric
{
	[TestClass]
	public class SymmetricEncryptorTests
	{
		[TestMethod]
		public void SymmetricEncryptor_BasicUsage()
		{
			// Create the Credentials
			CryptoCredentials credentials = new CryptoCredentials
			{
				Passphrase = new CryptoString("My Passphrase"),
			};

			// Create some Buffers
			byte[] buffer = new ByteGenerator().GenerateBytes(2000);
			byte[] decryptedBuffer;
			byte[] encryptedBuffer;

			using (SymmetricEncryptor encryptor = new SymmetricEncryptor(credentials))
			{
				//Encrypt the buffer
				encryptedBuffer = encryptor.EncryptBytes(buffer);

				// Decrypt
				decryptedBuffer = encryptor.DecryptBytes(encryptedBuffer);
			} 

			// Assert - Check to make sure the bytes are all the same
			Assert.IsTrue(buffer.SequenceEqual(decryptedBuffer));
		}
		[TestMethod]
		public void SymmetricEncryptor_BasicTest()
		{
			CryptoCredentials credentials = new CryptoCredentials
			{
				Passphrase = new CryptoString("My Passphrase"),
				Pin = 2222
			};

			// Arrange
			int blockSize = 1000;
			byte[] buffer = new ByteGenerator().GenerateBytes(blockSize);

			byte[] decryptedBuffer;
			byte[] encryptedBuffer;

			using (SymmetricEncryptor encryptor = new SymmetricEncryptor(credentials))
			{
				//Encrypt
				encryptedBuffer = encryptor.EncryptBytes(buffer);

				// Decrypt
				decryptedBuffer = encryptor.DecryptBytes(encryptedBuffer);
			} // IDispose - Closes and clears the keys in memory

			// Assert - Check to make sure the bytes are all the same
			Assert.IsTrue(buffer.SequenceEqual(decryptedBuffer));
		}

		/// <summary>
		/// TEst to show that the Symmetric Algorithm produces different output with every encryption.
		/// </summary>
		[TestMethod]
		public void SymmetricEncryptor_ComparingOutPredictability()
		{
			CryptoCredentials credentials = new CryptoCredentials
			{
				Passphrase = new CryptoString("My Passphrase"),
				Pin = 2222
			};

			byte[] buffer = new byte[] { 0x1, 0x0 };

			using (SymmetricEncryptor encryptor = new SymmetricEncryptor(credentials))
			{
				for (int i = 0; i < 20; i++)
				{
					byte[] encrypted = encryptor.EncryptBytes(buffer);
					byte[] decrypted = encryptor.DecryptBytes(encrypted);
					Debug.WriteLine($"encrypted: {StringUtils.BytesToHexString(encrypted)} -> Decrypted: {StringUtils.BytesToHexString(decrypted)}");

				}
			} // IDispose - Closes and clears the keys in memory
			
		}

		[TestMethod]
		public void SymmetricEncryptor_HeavyUsage()
		{

			CryptoCredentials credentials = new CryptoCredentials
			{
				Passphrase = new CryptoString("My Passphrase"),
				Pin = 2222
			};
			CryptoRandomizer random = new CryptoRandomizer();
			const int Iterations = 10;
			const int MaxMemoryBlock = 100000;


			for (int i = 0; i < Iterations; i++)
			{
				int blockSize = random.Next(MaxMemoryBlock);
				byte[] buffer = new ByteGenerator().GenerateBytes(blockSize);
				string key = CryptoString.GenerateRandomText(1000);

				byte[] decryptedBuffer;
				byte[] encryptedBuffer;

				using (SymmetricEncryptor encryptor = new SymmetricEncryptor(credentials))
				{
					//Encrypt
					encryptedBuffer = encryptor.EncryptBytes(buffer);

					// Decrypt
					decryptedBuffer = encryptor.DecryptBytes(encryptedBuffer);
				} // IDispose - Closes and clears the keys in memory

				// Assert - Check to make sure the bytes are all the same
				Assert.IsTrue(buffer.SequenceEqual(decryptedBuffer));
			}			
		}



		[TestMethod]
		public void BasicUsage_MultiThreadHeavyUsuage()
		{
			// TODO
			
			//// Create a thread callback
			
			//// Act - Encrypt
			
			//// Launce all the threads

			
			//// Wait for all threads to finish
			
			///Wait
			
		}


		[TestMethod]
		public void SymmetricEncryptor_HandleExceptions()
		{
			CryptoCredentials credentials = new CryptoCredentials
			{
				Passphrase = new CryptoString("My Passphrase"),
				Pin = 2222
			};
			CryptoCredentials credentialsBadCredentials = new CryptoCredentials
			{
				Passphrase = new CryptoString("My BAD Passphrase"),
				Pin = 2222
			};

			byte[] buffer = new ByteGenerator().GenerateBytes(100);		
			byte[] decryptedBuffer;
			byte[] encryptedBuffer;

			using (SymmetricEncryptor encryptor = new SymmetricEncryptor(credentials))
			{
				////////////////////////////////////////
				//Successful En/Decryption
				////////////////////////////////////////
				encryptor.Credentials = credentials;
				encryptedBuffer = encryptor.EncryptBytes(buffer);
				decryptedBuffer = encryptor.DecryptBytes(encryptedBuffer);
				Assert.IsTrue(buffer.SequenceEqual(decryptedBuffer));


				////////////////////////////////////////
				// BAD Credentials will throw exception
				////////////////////////////////////////
				try
				{
					encryptor.Credentials = credentials;
					encryptedBuffer = encryptor.EncryptBytes(buffer);
					encryptor.Credentials = credentialsBadCredentials;
					decryptedBuffer = encryptor.DecryptBytes(encryptedBuffer);
				}
				catch (CryptoDecryptionException exception)
				{
					Assert.IsTrue(exception is CryptoDecryptionException);

				}
				catch (Exception exception)
				{
					Debug.WriteLine(exception.Message);
					Assert.Fail("Test was not expecting an Exception");
				}

				////////////////////////////////////////
				// NULL Credentials not set will throw exception
				////////////////////////////////////////
				try
				{
					encryptor.Credentials = null;
					encryptedBuffer = encryptor.EncryptBytes(buffer);
					encryptor.Credentials = credentialsBadCredentials;
					decryptedBuffer = encryptor.DecryptBytes(encryptedBuffer);
				}
				catch (CryptoCredentialsNullException exception)
				{
					Assert.IsTrue(exception is CryptoCredentialsNullException);

				}
				catch (Exception exception)
				{
					Debug.WriteLine(exception.Message);
					Assert.Fail("Test was not expecting an Exception");
				}				
			}

		}		
	}
}
