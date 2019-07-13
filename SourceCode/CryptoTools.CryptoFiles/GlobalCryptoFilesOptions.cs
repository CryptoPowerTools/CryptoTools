using CryptoTools.CryptoFiles.DataFiles;

namespace CryptoTools.CryptoFiles
{

	public static class GlobalCryptoFilesOptions
	{
		public static CryptoDataFileOptions CryptoDataFileOptions { get; private set; }

		static GlobalCryptoFilesOptions()
		{
			ResetDefaults();
		}

		/// <summary>
		/// Resets all options back to default. This should generally not be called after the application sets Application specific options.
		/// </summary>
		public static void ResetDefaults()
		{
			//////////////////////////////////////////////////////////
			// CryptoDataFile Initialize
			//////////////////////////////////////////////////////////
			CryptoDataFileOptions = new CryptoDataFileOptions()
			{
				FileFormat = new byte[] { 0xFF, 0xAA },
				FileVersion = 1,
				ContentFormat = new byte[] { 0xA, 0x0 },
				ContentVersion = 1,
				EndFileFormat = new byte[] { 0xAA, 0xFF }
			};
		}
	}
}