using CryptoTools.Cryptography.Utils;
//using System.IO.Compression;
////using Ionic.Zip;

using SharpCompress;
using SharpCompress.Archives.Zip;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;

namespace CryptoTools.CryptoArchivers.Archivers
{
  //  public class SharpCompressArchiver : IFileArchiver
  //  {

		////private ZipFile _zipFile = new ZipFile();
		////ZipFile _zip = new ZipFile

		//ZipArchive _zipArchive;
		//private string _fileFullName;             
		//private CryptoCredentials _credentials;

		//public SharpCompressArchiver()
		//{
		//	_zipArchive = ZipArchive.Create();
		//}

  //      public string FullFileName
  //      {
  //          get
  //          {
		//		//return _zipFile.Name;
		//		return _fileFullName;
  //          }
  //          set
  //          {
		//		//_zipFile.Name = value;
		//		_fileFullName = value;
  //          }
  //      }

  //      public CryptoCredentials Credentials
  //      {
  //          get
  //          {
  //              return _credentials;
  //          }
  //          set
  //          {
  //              _credentials = value;
  //          }
  //      }

  //      public bool RemoveFilesAfterSave { get; set; }

  //      public void AddFile(string fileName, string directoryPathInArchive = "")
  //      {
		//	//_zipFile.AddFile(fileName, directoryPathInArchive);
		//	//ZipFile.

		//	//ZipFile.A  

		//	FileInfo fileInfo = new FileInfo(fileName);
		//	_zipArchive.AddEntry(fileInfo.Name, fileInfo.OpenRead(), true);		
			
			
  //      }

  //      public void AddDirectory(string directoryName, string directoryNameInArchive = "")
  //      {
		//	//_zipFile.AddDirectory(directoryName, directoryNameInArchive);
		//	//ZipFile.
		//	//_zipArchive.

		//	DirectoryInfo di = new DirectoryInfo(directoryName);
		//	foreach (var fi in di.GetFiles())
		//	{
		//		//_zipArchive.AddEntry(fi.Name, fi.OpenRead(), true);
		//		AddFile(fi.FullName, directoryNameInArchive);
		//	}

		//	//using (Stream stream = File.OpenWrite(_fileFullName))
		//	//{
		//	//	using(var writer = WriterFactory.Open(stream, ArchiveType.Zip, CompressionType.Deflate))
		//	//	{
		//	//		writer.WriteAll(directoryName, "*", SearchOption.AllDirectories);
		//	//	}
		//	//}

		//	//zipArchive.AddEntry()

		//}


		//public void ExtractAll(string destinationPath)
  //      {
		//	//using (ZipFile zip = new ZipFile(FullFileName))
		//	//{
		//	//    try
		//	//    {
		//	//        zip.ExtractAll(destinationPath);

		//	//    }
		//	//    catch (Ionic.Zip.ZipException)
		//	//    {
		//	//        // Handle
		//	//    }
		//	//}

		//	//SharpCompress.Archives.ArchiveFactory.WriteToDirectory(_fileFullName, destinationPath);

		//	_zipArchive.ExtractAllEntries();
			


		//	//using (Stream fileStream = new FileStream(_fileFullName, FileMode.Open))
		//	//{
		//	//	using (IReader reader = ReaderFactory.Open(fileStream))
		//	//	{
		//	//		while (reader.MoveToNextEntry())
		//	//		{
		//	//			if (reader.Entry != null)
		//	//			{
		//	//				if (!reader.Entry.IsDirectory)
		//	//				{
		//	//					reader.WriteEntryToDirectory(destinationPath, new ExtractionOptions
		//	//					{
		//	//						ExtractFullPath = true,
		//	//						Overwrite = true
		//	//					});
		//	//				}
		//	//			}
		//	//		}
		//	//	}
		//	//}
  //      }

  //      public void Save()
  //      {
		//	//// These are logical default. Can tweak if needed.
		//	//_zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
		//	//_zipFile.CompressionMethod = CompressionMethod.BZip2;
		//	//_zipFile.Encryption = EncryptionAlgorithm.WinZipAes256; // ???? this should be the most secure
		//	//_zipFile.Strategy = Ionic.Zlib.CompressionStrategy.Default;

		//	//// Convert password to a string
		//	//_zipFile.Password = CryptoString.SecureStringToString(Credentials.Passphrase.GetSecureString());

		//	//_zipFile.Save(FullFileName);

		//	//// Fill with Random Text to obsfuscate in memory 
		//	//_zipFile.Password = CryptoString.GenerateRandomText(1000);

		//	////Debug.WriteLine(_zipFile.Info);
		//	///

		//	FileStream temp = new FileStream(_fileFullName, FileMode.OpenOrCreate, FileAccess.Write);
		//	_zipArchive.SaveTo(temp, CompressionType.Deflate);
		//	temp.Close(); //PUT IN TRY block


  //      }
  //  }
}

