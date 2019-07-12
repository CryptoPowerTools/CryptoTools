using CryptoTools.Common.Utils;
using CryptoTools.Cryptography.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Cryptography.Guids
{
	public class Sha256GuidAlgorithm : IGuidAlgorithm
	{
		Hasher _hasher = new Hasher(SHA256.Create());

		public string NewGuid()
		{
			string random = Guid.NewGuid().ToString("N");
			string sha256 = _hasher.Hash(random);

			// Create and Append Checksum
			string checksum = _hasher.Hash(sha256);
			string guid = $"{sha256}{checksum.Substring(0, 4)}";

			return guid;
		}

		public bool Verify(string guid)
		{
			if (string.IsNullOrWhiteSpace(guid))
			{
				return false;
			}
			// Check 68 chars long
			if (guid.Length != 68)
			{
				return false;
			}
			// Alpha numeric
			if (!ValidateUtils.IsAlphaNumeric(guid))
			{
				return false;
			}

			string sha256 = guid.Substring(0, 64);
			string checksum = guid.Substring(64, 4);

			string checkSha256 = _hasher.Hash(sha256);
			string checksumCalculated = checkSha256.Substring(0, 4);

			if (!checksum.Equals(checksumCalculated))
			{
				return false;
			}

			return true;
		}
	}
}
