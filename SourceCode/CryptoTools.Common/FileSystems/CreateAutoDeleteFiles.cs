using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Common.FileSystems
{
	public class CreateAutoDeleteFiles : IDisposable
	{
		private IList<FileInfo> _files;
		private FileManager _fileMan = new FileManager();

		public CreateAutoDeleteFiles(IList<FileInfo> files, bool fillWithBytes = false, int fillBufferSize = 10)
		{
			_files = files;

			foreach(FileInfo info in _files)
			{
				if (fillWithBytes)
				{
					byte[] bytes = new ByteGenerator().GenerateBytes(fillBufferSize);
					using (FileStream fileStream = File.Create(info.FullName, bytes.Length, FileOptions.WriteThrough))
					{
						fileStream.Write(bytes, 0, bytes.Length);
					}
				}
				else
				{
					using (File.Create(info.FullName))
					{
					}
				}
			}
		}

		public void Dispose()
		{
			// Delete Files
			foreach (FileInfo info in _files)
			{
				bool b = _fileMan.IsFileLocked(info);

				if(!b)
				{
					File.Delete(info.FullName);
				}				
			}
		}
	}
}
