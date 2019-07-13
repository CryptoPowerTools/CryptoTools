using CryptoTools.Cryptography.Utils;

namespace CryptoTools.CryptoArchivers
{
	public interface IFileArchiver
	{
		string FullFileName { get; set; }
		bool RemoveFilesAfterSave { get; set; }
		CryptoCredentials Credentials { get; set; }

		//void AddFile(string testFile, string directoryPathInArchive = "");
		//void AddDirectory(string directoryName, string directoryNameInArchive = "");
		//void Save();
		//void ExtractAll(string destinationPath);



		void CreateFromDirectory(string directoryName);
		void ExtractToDirectory(string directory);


	}
}
