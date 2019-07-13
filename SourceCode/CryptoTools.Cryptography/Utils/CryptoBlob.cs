using CryptoTools.Cryptography.Exceptions;
using CryptoTools.Cryptography.Hashing;
using CryptoTools.Cryptography.Symmetric;
using System;
using System.Linq;

namespace CryptoTools.Cryptography.Utils
{



	public class CryptoBlob
	{
		//	public class BlobData
		//	{
		//		public byte[] Data { get; set; }
		//		public byte[] Hash { get; set; } = new byte[32];
		//		public int HashSize { get { return 32;  } }
		//	}

		public CryptoCredentials Credentials { get; set; }
		private byte[] _encryptedBytes;
		private Hasher _hasher = new Hasher();
		private readonly int HashSize;


/* Unmerged change from project 'CryptoTools.Cryptography (net461)'
Before:
		private SymmetricEncryptor _encryptor;
		
		public CryptoBlob(CryptoCredentials credentials, byte[] decryptedBytes = null)
After:
		private SymmetricEncryptor _encryptor;

		public CryptoBlob(CryptoCredentials credentials, byte[] decryptedBytes = null)
*/
		private SymmetricEncryptor _encryptor;

		public CryptoBlob(CryptoCredentials credentials, byte[] decryptedBytes = null)
		{
			Credentials = credentials;
			_encryptor = new SymmetricEncryptor(Credentials);
			if (decryptedBytes != null) Encrypt(decryptedBytes);
			HashSize = Hasher.CalculateHashBytesLength(_hasher.GetAlgorithm());
		}


		#region Public Methods
		/// <summary>
		/// Encrypts the bytes to the Blob and adds a checksum hash to ensure data integrity
		/// </summary>
		/// <param name="data"></param>
		public void Encrypt(byte[] data)
		{
			// Create Unencrypted Block
			byte[] checksumDecrypted = _hasher.HashToBytes(data);
			byte[] fullDecryptedBytes = data.Concat(checksumDecrypted).ToArray();

			// Create Encrypted Block
			byte[] encryptedBytes = _encryptor.EncryptBytes(fullDecryptedBytes);
			byte[] checksumEncrypted = _hasher.HashToBytes(encryptedBytes);
			byte[] fullEncryptedBytes = encryptedBytes.ToList().Concat(checksumEncrypted).ToArray();

			_encryptedBytes = fullEncryptedBytes;

		}

		/// <summary>
		/// Decrypts the bytes of the Blob and validates the checksum hash to ensure data integrity
		/// </summary>
		/// <returns></returns>
		public byte[] Decrypt(bool validateChecksum = true)
		{
			// Validate Encrypted (if flag is set)
			if (validateChecksum)
				ValidateChecksum(_encryptedBytes);

			// Encrypted Block
			byte[] checksumEncrypted = _encryptedBytes.Skip(_encryptedBytes.Length - HashSize).ToArray();
			byte[] encryptedBytes = _encryptedBytes.Take(_encryptedBytes.Length - HashSize).ToArray();

			// Decrypted Block
			byte[] fullDecryptedData = _encryptor.DecryptBytes(encryptedBytes);
			byte[] checksumDecrypted = fullDecryptedData.Skip(fullDecryptedData.Length - HashSize).ToArray();

			// Validate Decrypted (if flag is set)
			if (validateChecksum)
				ValidateChecksum(fullDecryptedData);

			// Strip the Checksum and you have your original data
			byte[] decryptedBytes = fullDecryptedData.Take(fullDecryptedData.Length - HashSize).ToArray();

			return decryptedBytes;
		}

		/// <summary>
		/// Directly Sets the Encrypted Bytes to the Blob. This can be useful when you are reading the encrypted data from other containers such as a file for example.
		/// </summary>
		/// <param name="data"></param>
		public void SetEncryptedBytes(byte[] encryptedBytes)
		{
			_encryptedBytes = encryptedBytes;
		}

		/// <summary>
		/// Directly Gets the Encrypted Bytes of the Blob. This can be useful when you are saving the encrypted data in other containers such as a file for example.
		/// </summary>
		/// <returns></returns>
		public byte[] GetEncryptedBytes()
		{
			return _encryptedBytes;
		}

		public void Clear()
		{
			if (_encryptedBytes != null)
			{
				Array.Clear(_encryptedBytes, 0, _encryptedBytes.Length);
			}
		}

		/// <summary>
		/// Validates the Checksum Hash of the Blob and throws an exception if the Blob Content Hash does not match the checksum
		/// </summary>
		public void ValidateChecksum()
		{
			// this will fully validate and throw exception if failure
			byte[] b = Decrypt(true);
		}
		#endregion


		#region Private Methods
		/// <summary>
		/// Validates the Checksum Hash of the Blob and throws an exception if the Blob Content Hash does not match the checksum
		/// </summary>
		private void ValidateChecksum(byte[] bytes)
		{
			byte[] checksum = bytes.Skip(bytes.Length - HashSize).ToArray();
			byte[] encryptedBytes = bytes.Take(bytes.Length - HashSize).ToArray();

			byte[] actualChecksum = _hasher.HashToBytes(encryptedBytes);

			if (!actualChecksum.SequenceEqual(checksum))
			{
				throw new CryptoBlobChecksumFailedException("CryptoBlob Failed Checksum. This could indicate data has been compromised or has been corrupted.");
			}
		}

		private CryptoCredentials GetCredentials()
		{
			if (Credentials == null)
			{
				throw new CryptoCredentialsNullException(this.GetType());
			}
			return Credentials;
		}
		#endregion

		/// <summary>
		/// Verifies that the data in the blob is in the expected format. Ensures that the data is valid by checking the hashes of the data. Ensure the file has not been tampered with 
		/// or corrupted. This method will throw an exception with useful debug messages.
		/// </summary>
		//public void Verify()		{

		//	// Check Formats and Markers
		//	if (!_isLoaded)
		//	{
		//		Load();
		//	}

		//	// Check Format Markers
		//	if (!Header.FileFormat.SequenceEqual(FileFormat))
		//	{
		//		throw new CryptoFileFailedVerificationException(FullFileName, $"FileFormat failed verification. Expected:{FileFormat} Actual:{Header.FileFormat}");
		//	}
		//	if (!Header.ContentFormat.SequenceEqual(ContentFormat))
		//	{
		//		throw new CryptoFileFailedVerificationException(FullFileName, $"ContentFormat failed verification. Expected:{ContentFormat} Actual:{Header.ContentFormat}");
		//	}
		//	if (!Footer.EndFileFormat.SequenceEqual(EndFileFormat))
		//	{
		//		throw new CryptoFileFailedVerificationException(FullFileName, $"EndFileFormat failed verification. Expected:{EndFileFormat} Actual:{Footer.EndFileFormat}");
		//	}

		//	//  Check Content Size
		//	if (Content.Length != Header.ContentSize)
		//	{
		//		throw new CryptoFileFailedVerificationException(FullFileName, $"ContentSize failed verification. Expected(Header.ContentSize):{Header.ContentSize} Actual(Content.Length):{Content.Length}");
		//	}

		//	//  Check Content Hash
		//	byte[] actualHash = _hasher.HashToBytes(Content);
		//	if (!Header.ContentHash.SequenceEqual(actualHash))
		//	{
		//		throw new CryptoFileFailedVerificationException(FullFileName, $"ContentHash failed verification. Expected(Header.ContentHash):{Header.ContentHash} Actual(Hash(Content)):{actualHash}");
		//	}

		//	//  Check File Hash
		//	byte[] fileContents = GetFileContents();
		//	byte[] actualFileHash = _hasher.HashToBytes(fileContents);
		//	if (!Footer.FileHash.SequenceEqual(actualFileHash))
		//	{
		//		throw new CryptoFileFailedVerificationException(FullFileName, $"FileHash failed verification. Expected(Footer.FileHash):{Footer.FileHash} Actual(Hash('filecontents')):{actualFileHash}");
		//	}
		//}
	}
}
