namespace CryptoTools.CryptoFiles.DataFiles
{

	/// <summary>
	/// /// Data structure that represents the File footer of the data file
	/// </summary>
	public class CryptoDataFileHeader
	{
		#region Public Data Fields
		public byte[] FileFormat { get; set; } = new byte[2];
		public int FileVersion { get; set; } = 1;
		public byte[] ContentFormat { get; set; } = new byte[2];
		public int ContentVersion { get; set; } = 1;
		public int ContentSize { get; set; } = 0;
		public byte[] ContentHash { get; set; } = new byte[32];
		public byte[] ReservedArea { get; set; } = new byte[64]
			{
			0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7,
			0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7,
			0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7,
			0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7,
			0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7,
			0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7,
			0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7,
			0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7
			};
		#endregion

		#region Internal Data Field Sizes
		internal int FileFormatSize { get { return FileFormat.Length; } }
		internal int FileVersionSize { get { return sizeof(int); } }
		internal int ContentFormatSize { get { return ContentFormat.Length; } }
		internal int ContentVersionSize { get { return sizeof(int); } }
		internal int ContentSizeSize { get { return sizeof(int); } }
		internal int ContentHashSize { get { return ContentHash.Length; } }
		internal int ReservedAreaSize { get { return ReservedArea.Length; } }
		internal int HeaderLength { get { return FileFormatSize + FileVersionSize + ContentFormatSize + ContentVersionSize + ContentSizeSize + ContentHashSize + ReservedAreaSize; } }
		#endregion
	}
}