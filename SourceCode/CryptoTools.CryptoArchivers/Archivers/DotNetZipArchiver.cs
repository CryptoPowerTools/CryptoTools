using CryptoTools.Cryptography.Utils;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.CryptoArchivers.Archivers
{
	public class DotNetZipArchiver : IFileArchiver
	{

		private ZipFile _zipFile = new ZipFile();

		private CryptoCredentials _credentials;


		public string FullFileName
		{
			get
			{
				return _zipFile.Name;
			}
			set
			{
				_zipFile.Name = value;
			}
		}

		public CryptoCredentials Credentials
		{
			get
			{
				return _credentials;
			}
			set
			{
				_credentials = value;
			}
		}

		public bool RemoveFilesAfterSave { get; set; }

		public void AddFile(string fileName, string directoryPathInArchive = "")
		{
			_zipFile.AddFile(fileName, directoryPathInArchive);
		}

		public void AddDirectory(string directoryName, string directoryNameInArchive = "")
		{
			_zipFile.AddDirectory(directoryName, directoryNameInArchive);
		}


		public void ExtractAll(string destinationPath)
		{
			using (ZipFile zip = new ZipFile(FullFileName))
			{
				try
				{
					zip.ExtractAll(destinationPath);

				}
				catch (Ionic.Zip.ZipException)
				{
					// Handle
				}
			}
		}

		public void Save()
		{
			// These are logical default. Can tweak if needed.
			_zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
			_zipFile.CompressionMethod = CompressionMethod.BZip2;
			_zipFile.Encryption = EncryptionAlgorithm.WinZipAes256; // ???? this should be the most secure
			_zipFile.Strategy = Ionic.Zlib.CompressionStrategy.Default;

			// Convert password to a string
			_zipFile.Password = CryptoString.SecureStringToString(Credentials.Passphrase.GetSecureString());

			_zipFile.Save(FullFileName);

			// Fill with Random Text to obsfuscate in memory 
			_zipFile.Password = CryptoString.GenerateRandomText(1000);

			//Debug.WriteLine(_zipFile.Info);
			
		}

		public void CreateFromDirectory(string directoryName)
		{
			throw new NotImplementedException();
		}

		public void ExtractToDirectory(string directory)
		{
			throw new NotImplementedException();
		}
	}
}
