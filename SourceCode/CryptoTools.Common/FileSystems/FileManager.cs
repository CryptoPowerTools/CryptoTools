using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CryptoTools.Common.FileSystems
{
	/// <summary>
	/// Wrapper / Facade class to the file system. this provides a higher level interface to the core .NET Api for managing files.
	/// </summary>
	public class FileManager
	{
		public void DeleteDirectory(string folderName, bool recursive = false)
		{
			try
			{
				if (Directory.Exists(folderName))
				{
					Directory.Delete(folderName, recursive);
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}

		public void CreateDirectory(string _directoryName)
		{
			Directory.CreateDirectory(_directoryName);
		}

		public bool AllFilesExists(IList<FileInfo> files)
		{
			foreach (FileInfo info in files)
			{
				if (!File.Exists(info.FullName))
				{
					return false;
				}
			}
			return true;
		}

		public bool AllFilesDoNotExist(IList<FileInfo> files)
		{
			foreach (FileInfo info in files)
			{
				if (File.Exists(info.FullName))
				{
					return false;
				}
			}
			return true;
		}


		public void DeleteAllFilesAndDirectories(List<FileInfo> files, List<DirectoryInfo> directories, bool recursiveDirectories = false)
		{
			List<string> f = files.ConvertAll(file => file.FullName);
			List<string> d = directories.ConvertAll(directory => directory.FullName);

			DeleteAllFilesAndDirectories(f, d, recursiveDirectories);
		}


		public void DeleteAllFilesAndDirectories(List<string> files, List<string> directories, bool recursiveDirectories = false)
		{
			try
			{
				// Remove files first
				foreach (string fileName in files)
				{

					try
					{
						File.Delete(fileName);
					}
					catch (Exception exception)
					{
						Debug.WriteLine(exception.Message);
					}
				}

				// Now the directories
				foreach (string directoryName in directories)
				{
					try
					{
						Directory.Delete(directoryName, recursiveDirectories);
					}
					catch (Exception exception)
					{
						Debug.WriteLine(exception.Message);
					}
				}
			}
			catch (Exception exception)
			{
				Debug.WriteLine(exception.Message);
			}
		}

		public bool AllDirectoriesExists(List<DirectoryInfo> directories)
		{
			foreach (DirectoryInfo info in directories)
			{
				if (!Directory.Exists(info.FullName))
				{
					return false;
				}
			}
			return true;
		}

		public void DeleteFile(string fileName)
		{
			try
			{
				File.Delete(fileName);

			}
			catch (Exception)
			{
				throw;
			}

			File.Delete(fileName);

/* Unmerged change from project 'CryptoTools.Common (net461)'
Before:
		}

		
		public FileInfo GetFileInfo(string fileName)
After:
		}


		public FileInfo GetFileInfo(string fileName)
*/
		}


		public FileInfo GetFileInfo(string fileName)
		{
			FileInfo info = new FileInfo(fileName);
			return info;
		}



		public void CreateWriteBytes(string fullFileName, byte[] bytes)
		{
			File.WriteAllBytes(fullFileName, bytes);
		}

		public bool DirectoryExists(string directoryRoot)
		{
			return Directory.Exists(directoryRoot);
		}

		public bool FileExists(string archiveFileName)
		{
			return File.Exists(archiveFileName);
		}

		public bool IsFileLocked(FileInfo file)
		{
			FileStream stream = null;
			try
			{
				stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			}
			catch (IOException)
			{
				//the file is unavailable because it is:
				//still being written to
				//or being processed by another thread
				//or does not exist (has already been processed)
				return true;
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}

			//file is not locked
			return false;
		}
		public bool IsFileLocked(string fileName)
		{
			return IsFileLocked(new FileInfo(fileName));
		}

		public string GenerateTempFileName(string extension = "tmp", bool excludeTilde = false, int length = 10)
		{
			string name = new ByteGenerator().GenerateBytesString(length);
			string tilde = excludeTilde == true ? "" : "~";
			string fileName = $"{tilde}{name}.{extension}";
			return fileName;
		}
		public string GenerateTempDirectoryName(int length = 10)
		{
			string name = new ByteGenerator().GenerateBytesString(length);
			return name;
		}
	}
}
