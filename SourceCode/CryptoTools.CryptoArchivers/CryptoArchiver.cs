using CryptoTools.Common.FileSystems;
using CryptoTools.CryptoArchivers.Archivers;
using CryptoTools.Cryptography.Hashing;
using CryptoTools.Cryptography.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.CryptoArchivers
{
	public class CryptoArchiver : IFileArchiver
	{
		public string FullFileName
		{
			get
			{
				return _archiver.FullFileName;
			}
			set
			{
				_archiver.FullFileName = value;
			}
		}
		public CryptoCredentials Credentials
		{
			get
			{
				if (_archiver.Credentials == null)
					throw new CryptoCredentialsNullException(typeof(CryptoArchiver));

				return _archiver.Credentials;
			}
			set
			{
				_archiver.Credentials = value;
			}
		}

		public bool RemoveFilesAfterSave { get; set; }

		private IFileArchiver _archiver;


		public CryptoArchiver(string fullFileName, IFileArchiver archiver = null)
		{
			if(archiver==null)
			{
				archiver = new ZipArchiver(fullFileName);
			}
			_archiver = archiver;
			_archiver.FullFileName = fullFileName;

		}		

		public void CreateFromDirectory(string directoryName)
		{
			_archiver.CreateFromDirectory(directoryName);
		}
		
		public void ExtractToDirectory(string directory)
		{
			_archiver.ExtractToDirectory(directory);
		}
		
		public byte[] GetBytesFromDirectory(string directoryName)
		{
			FileManager fileMan = new FileManager();
			byte[] bytes;
			string tempFileName = fileMan.GenerateTempFileName("GetBytes");

			using (AutoDeleteFiles autoDelete = new AutoDeleteFiles(tempFileName))
			{
				// Create a temp archiver
				CryptoArchiver tempArchiver = new CryptoArchiver(tempFileName, this);
				tempArchiver.CreateFromDirectory(directoryName);

				// Read Bytes and delete file
				bytes = File.ReadAllBytes(tempFileName);
			}
			return bytes;
		}

		public void ExtractToDirectoryFromBytes(string directoryName, byte[] bytes)
		{
			FileManager fileMan = new FileManager();
			string tempFileName = fileMan.GenerateTempFileName("ExtractBytes");

			using (AutoDeleteFiles autoDelete = new AutoDeleteFiles(tempFileName))
			{
				// Create/Write the bytes to a file
				fileMan.CreateWriteBytes(tempFileName, bytes);

				// Create a temp archiver
				CryptoArchiver tempArchiver = new CryptoArchiver(tempFileName, this);
				tempArchiver.ExtractToDirectory(directoryName);
			}
		}

	
	}
}
