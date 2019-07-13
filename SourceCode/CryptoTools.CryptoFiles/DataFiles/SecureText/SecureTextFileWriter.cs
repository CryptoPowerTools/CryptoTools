using System;

namespace CryptoTools.CryptoFiles.UnitTests.DataFiles.SecureTextFiles
{
	public class SecureTextFileWriter : IDisposable
	{
		private string _fileName;

		public SecureTextFileWriter(string fileName)
		{
			this._fileName = fileName;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		internal void WriteLine(string line)
		{
			throw new NotImplementedException();
		}

		internal void AppendLine(string line)
		{
			throw new NotImplementedException();
		}

		internal void Write(string v)
		{
			throw new NotImplementedException();
		}
	}
}
