using System;
using System.Collections.Generic;
using System.IO;

namespace CryptoTools.Common.FileSystems
{
	public class CreateAutoDeleteDirectory : IDisposable
	{
		private List<DirectoryInfo> _directories = new List<DirectoryInfo>();
		private string _directoryName;
		private FileManager _fileMan = new FileManager();


		public CreateAutoDeleteDirectory(List<DirectoryInfo> directories)
		{
			_directories = directories;

			foreach (DirectoryInfo info in _directories)
			{
				// Using so the file closes ASAP
				Directory.CreateDirectory(info.FullName);
			}
		}

		public CreateAutoDeleteDirectory(string directoryName)
		{
			_directoryName = directoryName;
			DirectoryInfo info = Directory.CreateDirectory(_directoryName);
		}

		public void Dispose()
		{
			if (_fileMan.DirectoryExists(_directoryName))
				_fileMan.DeleteDirectory(_directoryName);

			// Delete Files
			foreach (DirectoryInfo info in _directories)
			{
				bool b = _fileMan.DirectoryExists(info.FullName);

				if (b)
				{
					_fileMan.DeleteDirectory(info.FullName, true);
				}
			}
		}
	}
}
