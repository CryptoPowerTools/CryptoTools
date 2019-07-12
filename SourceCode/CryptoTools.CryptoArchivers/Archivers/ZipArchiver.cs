using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using CryptoTools.Cryptography.Utils;
using System.IO;

namespace CryptoTools.CryptoArchivers.Archivers
{
	public class ZipArchiver : IFileArchiver
	{
		private string _fileFullName;
		private CryptoCredentials _credentials;

		public ZipArchiver(string fullFileName)
		{
			FullFileName = fullFileName;
		}

		public string FullFileName
		{
			get
			{
				return _fileFullName;
			}
			set
			{
				_fileFullName = value;
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

	
		public void CreateFromDirectory(string directoryName)
		{
			ZipFile.CreateFromDirectory(directoryName, _fileFullName, CompressionLevel.Optimal, true);
		}

		public void ExtractToDirectory(string directory)
		{
			ZipFile.ExtractToDirectory(_fileFullName, directory, true);
		}
	}
}

