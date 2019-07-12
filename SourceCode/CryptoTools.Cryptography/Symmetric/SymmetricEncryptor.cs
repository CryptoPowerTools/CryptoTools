using CryptoTools.Common.Logging;
using CryptoTools.Cryptography.Exceptions;
using CryptoTools.Cryptography.Hashing;
using CryptoTools.Cryptography.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Cryptography.Symmetric
{
	public class SymmetricEncryptor : IDisposable	
	{
		#region Private Fields
		private ILog Log = Logger.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private SymmetricAlgorithm _algorithm;
		private Hasher _hasher;
		#endregion

		#region Public Properties
		public SymmetricEncryptorOptions Options { get; private set; }
		public CryptoCredentials Credentials { get; set; }
		#endregion

		#region Constructors
		public SymmetricEncryptor(	CryptoCredentials credentials, 
									SymmetricAlgorithm algorithm = null, 
									SymmetricEncryptorOptions options = null, 
									HashAlgorithm hashAlgorithm = null) : this(algorithm, options, hashAlgorithm)
		{
			Credentials = credentials;
		}

		public SymmetricEncryptor(SymmetricAlgorithm algorithm = null, SymmetricEncryptorOptions options = null, HashAlgorithm hashAlgorithm = null)
		{
			//_algorithm = algorithm != null ? algorithm : Aes.Create();
			_algorithm = algorithm != null ? algorithm : DES.Create();
			_hasher = hashAlgorithm != null ? new Hasher(hashAlgorithm) : new Hasher();
			Options = SymmetricEncryptorOptions.CreateMergedInstance(options);
		}
		#endregion

		#region Private Helper Methods
		private byte[] MakeKey(string key)
		{
			byte[] salt = UTF8Encoding.UTF8.GetBytes(Options.KeySalt);
			Rfc2898DeriveBytes generator = new Rfc2898DeriveBytes(key, salt);
			return generator.GetBytes(_algorithm.KeySize / 8);
		}
		private byte[] MakeIV(string iv)
		{
			byte[] salt = UTF8Encoding.UTF8.GetBytes(Options.IVSalt);
			Rfc2898DeriveBytes generator = new Rfc2898DeriveBytes(iv, salt);
			return generator.GetBytes(_algorithm.BlockSize / 8);
		}
		private void ReInitialize()
		{
			if(Credentials==null)
			{
				throw new CryptoCredentialsNullException(this.GetType());
			}


			_algorithm.Key = MakeKey(Credentials.Key);
			_algorithm.IV = MakeIV(Options.InitializationVector);
			_algorithm.Mode = CipherMode.CBC; // Seems to be a good default, could change if good reason is found
			_algorithm.Padding = PaddingMode.ISO10126; // ISO Mode seems to provide the most Obsfucation
		}
		#endregion

		#region Public Methods
			
		public byte[] EncryptBytes(byte[] bytesIn)
		{
			byte[] bytesOut;
			try
			{
				ReInitialize();
				using (MemoryStream memoryStream = new MemoryStream())
				{
					ICryptoTransform transform = _algorithm.CreateEncryptor();
					bytesOut = transform.TransformFinalBlock(bytesIn, 0, bytesIn.Length);
					//using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
					//{
					//	if (!cryptoStream.CanWrite) Debug.Assert(false, "Stream is not writable");

					//	cryptoStream.Write(bytesIn, 0, bytesIn.Length);

					//}
					//bytesOut = memoryStream.ToArray();
				}

			}
			catch (Exception exception)
			{
				Log.ErrorException(exception, this);
				throw;
			}
			
			return bytesOut;

		}

		public byte[] DecryptBytes(byte[] encryptedBytes)
		{
			byte[] output;
			try
			{
				ReInitialize();
				ICryptoTransform transform = _algorithm.CreateDecryptor();
				output = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
			
			}
			catch (CryptographicException cryptoException)
			{
				CryptoDecryptionException exception = new CryptoDecryptionException("Error Decrypting Bytes. This could be caused by invalid Credentials such as Passphrase or Pin.", cryptoException);
				Log.ErrorException(exception, this);
				throw exception;
			}
			catch (Exception exception)
			{
				Log.ErrorException(exception, this);
				throw;
			}
			return output;
		}
				

		public void Clear()
		{
			_algorithm.Clear();
		}

		public void Dispose()
		{
			Clear();
		}
		#endregion
	}
}
