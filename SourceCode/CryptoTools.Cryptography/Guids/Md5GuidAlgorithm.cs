using CryptoTools.Common.Utils;
using CryptoTools.Cryptography.Hashing;
using System;
using System.Security.Cryptography;

namespace CryptoTools.Cryptography.Guids
{
	public class Md5GuidAlgorithm : IGuidAlgorithm
	{
		Hasher _hasher = new Hasher(MD5.Create());

		public string NewGuid()
		{
			string random = Guid.NewGuid().ToString("N");
			string md5 = _hasher.Hash(random);

			// Create and Append Checksum
			string checksum = _hasher.Hash(md5);
			string guid = $"{md5}{checksum.Substring(0, 4)}";

			return guid;
		}

		public bool Verify(string guid)
		{
			if (string.IsNullOrWhiteSpace(guid))
			{
				return false;
			}
			// Check 32 chars long
			if (guid.Length != 36)
			{
				return false;
			}
			// Alpha numeric
			if (!ValidateUtils.IsAlphaNumeric(guid))
			{
				return false;
			}

			string md5 = guid.Substring(0, 32);
			string checksum = guid.Substring(32, 4);

			string checkMd5 = _hasher.Hash(md5);
			string checksumCalculated = checkMd5.Substring(0, 4);

			if (!checksum.Equals(checksumCalculated))
			{
				return false;
			}

			return true;
		}
	}
}
