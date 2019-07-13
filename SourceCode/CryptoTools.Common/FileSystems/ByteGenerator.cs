using CryptoTools.Common.Utils;
using System;
using System.Text;

namespace CryptoTools.Common.FileSystems
{
	/// <summary>
	/// Generates Random Bytes. This is useful for various encryption operations or even unit tests.
	/// </summary>
	public class ByteGenerator
	{
		public ByteGenerator()
		{
		}

		public byte[] GenerateBytes(int size)
		{
			//Random random = new Random();
			byte[] bytes = new byte[size];
			//random.NextBytes(bytes);
			Randomizer.GetBytes(bytes);
			return bytes;
		}

		public string GenerateBytesString(int size)
		{
			byte[] bytes = GenerateBytes(size);
			string byteString = BytesToHashSignature(bytes);
			return byteString;
		}

		private string BytesToHashSignature(byte[] bytes)
		{
			StringBuilder builder = new StringBuilder();
			foreach (Byte b in bytes)
			{
				builder.Append(b.ToString("x2"));
			}
			return builder.ToString();
		}
	}
}
