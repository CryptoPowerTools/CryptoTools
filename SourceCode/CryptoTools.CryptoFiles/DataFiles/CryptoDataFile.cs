using CryptoTools.CryptoFiles.Exceptions;
using CryptoTools.Cryptography.Hashing;
using CryptoTools.Cryptography.Symmetric;
using CryptoTools.Cryptography.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.CryptoFiles.DataFiles
{

	public class CryptoDataFile
	{
		#region File Structure
		public CryptoDataFileHeader Header = new CryptoDataFileHeader();
		public CryptoDataFileFooter Footer = new CryptoDataFileFooter();
		#endregion

		#region Private Fields
		private bool _isLoaded = false;
		private Hasher _hasher = null;
		#endregion

		#region Constructor
		public CryptoDataFile(string fullFileName, Hasher hasher = null) : this(null, fullFileName, hasher)
		{			
		}

		public CryptoDataFile(CryptoDataFileOptions options = null, string fullFileName = "", Hasher hasher = null)
		{
			FullFileName = fullFileName;
			Options = CryptoDataFileOptions.CreateMergedInstance(options);
			_hasher = hasher != null ? hasher : new Hasher();
		}

		#endregion

		#region Public Properties
		/// <summary>
		/// Name of the File
		/// </summary>
		public string FullFileName { get; set; }

		/// <summary>
		/// Returns true if the file is Loaded by calling the Load() method
		/// </summary>
		public bool IsLoaded { get { return _isLoaded; } }

		/// <summary>
		/// Contains the Content of the File
		/// </summary>
		public byte[] Content { get; set; }

		/// <summary>
		/// If True the Entire File will be encrypted with a checksum of they bytes at the end of the file
		/// </summary>
		public bool EncryptFile { get; set; } = false;
		
		/// <summary>
		/// If True the the Content (not the entire file) will be encrypted with a checksum of they bytes at the end of the content area
		/// NOTE: THIS IS NOT IMPLEMENTED YET AND WILL BE ON DEMAD
		/// </summary>
		public bool EncryptContent { get; set; } = false;

		/// <summary>
		/// Returns the Credentials. This is only required if the EncryptFile property is set to true.
		/// </summary>
		public CryptoCredentials Credentials { get; set; }

		/// <summary>
		/// Options to modifify the behavior of the DataFile. This can be useful if you want to override and customize file formats.
		/// </summary>
		public CryptoDataFileOptions Options { get; private set; }

		#endregion

		#region Private Helper Methods
		private void BuildHeader()
		{
			Header.FileFormat = Options.FileFormat;
			Header.FileVersion = Options.FileVersion.Value;
			Header.ContentFormat = Options.ContentFormat;
			Header.ContentVersion = Options.ContentVersion.Value;
			Header.ContentSize = Content.Length;
			Header.ContentHash = _hasher.HashToBytes(Content);
		}

		private void BuildFooter()
		{
			byte[] fileContents = GetFileContents();
			Footer.FileHash = _hasher.HashToBytes(fileContents);
			Footer.EndFileFormat = Options.EndFileFormat;
		}

		private byte[] GetFileContents()
		{
			// This needs to be updated if the file format changes
			byte[] fileContents = Header.FileFormat.
								Concat(BitConverter.GetBytes(Header.FileVersion)).
								Concat(Header.ContentFormat).
								Concat(BitConverter.GetBytes(Header.ContentVersion)).
								Concat(BitConverter.GetBytes(Header.ContentSize)).
								Concat(Header.ContentHash).
								Concat(Header.ReservedArea).
								Concat(Content).
								Concat(Footer.ReservedArea).
								ToArray();
								// Specifically EXCLUDES hash at the end
			return fileContents;
		}

		private CryptoCredentials GetCredentials()
		{
			if (Credentials == null)
			{
				throw new CryptoCredentialsNullException(this.GetType());
			}
			return Credentials;
		}

		private string GetFileHashFingerprint()
		{
			byte[] fileContents = GetFileContents();
			fileContents.Concat(Footer.FileHash).Concat(Footer.EndFileFormat);
			string hash = _hasher.Hash(fileContents);
			return hash;
		}


		#endregion

		#region Public Methods
		/// <summary>
		/// Checks the Format of the Data File quickly without reading the entire file and then return true if the format is valid based on the File/Content Format Marker. 
		/// Return false if the file does not have the proper format
		/// </summary>
		/// <returns></returns>
		public bool CheckFormat()
		{
			if (EncryptFile)
				throw new CryptoFileException(FullFileName, "CheckFormat() is not impemented when the file is encrypted.");

			using (FileStream fileStream = new FileStream(FullFileName, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader reader = new BinaryReader(fileStream))
				{
					// Check File Format
					byte[] fileFormat = reader.ReadBytes(Header.FileFormatSize);
					if (!Header.FileFormat.SequenceEqual(fileFormat))
					{
						return false;
					}
					// Check File Version
					int fileVersion = reader.ReadInt32();
					if (Header.FileVersion != fileVersion)
					{
						return false;
					}

					// Check Content format
					byte[] contentFormat = reader.ReadBytes(Header.ContentFormatSize);
					if (!Header.ContentFormat.SequenceEqual(contentFormat))
					{
						return false;
					}
					// Check File Version
					int contentVersion = reader.ReadInt32();
					if (Header.ContentVersion != contentVersion)
					{
						return false;
					}

					/////////////////////////////////////////////////////////////////////////////////
					// DONE at this time if the first 4 bytes indicate its the right 
					// format then we assume its valid.
					// NOTE: You must Open the File which will Verify it before using the data
					/////////////////////////////////////////////////////////////////////////////////					
				}
			}
			return true;
		}


		/// <summary>
		/// Verifies that the data is in the expected format. Ensures that the data is valid by checking the hashes of the data. Ensure the file has not been tampered with 
		/// or corrupted. This method will throw an exception with useful debug messages.
		/// </summary>
		public void Verify()
		{

			// Check Formats and Markers
			if (!_isLoaded)
			{
				Load();
			}

			// Check Format Markers
			if (!Header.FileFormat.SequenceEqual(Options.FileFormat))
			{
				throw new CryptoFileInvalidFormatException(FullFileName, $"FileFormat failed verification. Expected:{Options.FileFormat} Actual:{Header.FileFormat}");
			}
			if (Header.FileVersion != Options.FileVersion)
			{
				throw new CryptoFileInvalidFormatException(FullFileName, $"FileVersion failed verification. Expected:{Options.FileVersion} Actual:{Header.FileVersion}");
			}
			if (!Header.ContentFormat.SequenceEqual(Options.ContentFormat))
			{
				throw new CryptoFileInvalidFormatException(FullFileName, $"ContentFormat failed verification. Expected:{Options.ContentFormat} Actual:{Header.ContentFormat}");
			}
			if (Header.ContentVersion != Options.ContentVersion)
			{
				throw new CryptoFileInvalidFormatException(FullFileName, $"FileVersion failed verification. Expected:{Options.ContentVersion} Actual:{Header.ContentVersion}");
			}

			if (!Footer.EndFileFormat.SequenceEqual(Options.EndFileFormat))
			{
				throw new CryptoFileInvalidFormatException(FullFileName, $"EndFileFormat failed verification. Expected:{Options.EndFileFormat} Actual:{Footer.EndFileFormat}");
			}

			//  Check Content Size
			if (Content.Length != Header.ContentSize)
			{
				throw new CryptoFileInvalidFormatException(FullFileName, $"ContentSize failed verification. Expected(Header.ContentSize):{Header.ContentSize} Actual(Content.Length):{Content.Length}");
			}

			//  Check Content Hash
			byte[] actualHash = _hasher.HashToBytes(Content);
			if (!Header.ContentHash.SequenceEqual(actualHash))
			{
				throw new CryptoFileInvalidFormatException(FullFileName, $"ContentHash failed verification. Expected(Header.ContentHash):{Header.ContentHash} Actual(Hash(Content)):{actualHash}");
			}

			//  Check File Hash
			byte[] fileContents = GetFileContents();
			byte[] actualFileHash = _hasher.HashToBytes(fileContents);
			if (!Footer.FileHash.SequenceEqual(actualFileHash))
			{
				throw new CryptoFileInvalidFormatException(FullFileName, $"FileHash failed verification. Expected(Footer.FileHash):{Footer.FileHash} Actual(Hash('filecontents')):{actualFileHash}");
			}
		}

		/// <summary>
		/// Loads the file from an unencrypted byte array. This can useful when your loading the data from another source.
		/// </summary>
		/// <param name="bytes"></param>
		public void LoadFromBytes(byte[] bytes)
		{
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				using (BinaryReader reader = new BinaryReader(stream))
				{
					/////  Header  /////////////////////////////////////////////////
					Header.FileFormat = reader.ReadBytes(Header.FileFormatSize);
					Header.FileVersion = reader.ReadInt32();
					Header.ContentFormat = reader.ReadBytes(Header.ContentFormatSize);
					Header.ContentVersion = reader.ReadInt32();
					Header.ContentSize = reader.ReadInt32();
					Header.ContentHash = reader.ReadBytes(Header.ContentHashSize);
					Header.ReservedArea = reader.ReadBytes(Header.ReservedAreaSize);

					/////  Content  /////////////////////////////////////////////////
					Content = reader.ReadBytes(Header.ContentSize);

					/////  Footer  /////////////////////////////////////////////////
					Footer.ReservedArea = reader.ReadBytes(Footer.ReservedAreaSize);
					Footer.FileHash = reader.ReadBytes(Footer.FileHashSize);
					Footer.EndFileFormat = reader.ReadBytes(Footer.EndFileFormatSize);
				}
			}
			_isLoaded = true;

			// Verify
			Verify();
		}

		/// <summary>
		/// Loads the File from the FileSystem based on the FileName property
		/// </summary>
		public virtual void Load()
		{

			byte[] bytes;

			if (EncryptFile)
			{
				CryptoCredentials credentials = GetCredentials();
				byte[] encryptedBytes = File.ReadAllBytes(FullFileName);
				CryptoBlob blob = new CryptoBlob(credentials);
				blob.SetEncryptedBytes(encryptedBytes);
				bytes = blob.Decrypt(true);

			}
			else
			{
				bytes = File.ReadAllBytes(FullFileName);
			}			
			LoadFromBytes(bytes);			
		}

		/// <summary>
		/// Save the raw bytes of the file to an unencrypted byte array.
		/// </summary>
		/// <returns></returns>
		public byte[] SaveToBytes()
		{
			// Calculate Header/Footer Hashes, content size etc.
			BuildHeader();
			BuildFooter();

			//////////////////////////////////////////////////////////////////////
			// Consider back up the file with a ".backup.timestamp" extension"
			//////////////////////////////////////////////////////////////////////

			byte[] buffer = null;
			using (MemoryStream stream = new MemoryStream(0))
			{
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					/////  Header  /////////////////////////////////////////////////
					writer.Write(Header.FileFormat);
					writer.Write(Header.FileVersion);
					writer.Write(Header.ContentFormat);
					writer.Write(Header.ContentVersion);
					writer.Write(Header.ContentSize);
					writer.Write(Header.ContentHash);
					writer.Write(Header.ReservedArea);

					/////  Content  /////////////////////////////////////////////////
					writer.Write(Content);

					/////  Footer  /////////////////////////////////////////////////
					writer.Write(Footer.ReservedArea);
					writer.Write(Footer.FileHash);
					writer.Write(Footer.EndFileFormat);
				}

				buffer = stream.ToArray();
			}		
			return buffer;
		}

		/// <summary>
		/// Saves the File to the FileSystem based on the FileName. If the EncryptFile property is set then this file will be encrypted with a checksum at the end.
		/// </summary>
		public virtual void Save()
		{
			byte[] bytesToWrite;
			if(EncryptFile)
			{
				byte[] unencryptedBytes = SaveToBytes();
				CryptoBlob blob = new CryptoBlob(GetCredentials(), unencryptedBytes);
				bytesToWrite = blob.GetEncryptedBytes();
			}
			else
			{
				bytesToWrite = SaveToBytes();				
			}
			
			using (FileStream fileStream = new FileStream(FullFileName, FileMode.Create))
			{
				fileStream.Write(bytesToWrite, 0, bytesToWrite.Length);
			}			
		}


		#endregion
	}
}
