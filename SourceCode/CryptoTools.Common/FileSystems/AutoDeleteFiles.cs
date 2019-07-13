using System;
using System.Collections.Generic;
using System.IO;

namespace CryptoTools.Common.FileSystems
{
	public class AutoDeleteFiles : IDisposable
	{
		private IList<FileInfo> _files;
		private FileManager _fileMan = new FileManager();

		public AutoDeleteFiles(IList<FileInfo> files)
		{
			_files = files;
		}
		public AutoDeleteFiles(string fileName)
		{
			_files = new List<FileInfo>() { new FileInfo(fileName) };
		}

		public void Dispose()
		{
			// Delete Files
			foreach (FileInfo info in _files)
			{
				bool b = _fileMan.IsFileLocked(info);

				if (!b)
				{
					File.Delete(info.FullName);
				}
			}
		}
	}
}
