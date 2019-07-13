using CryptoTools.Common.Utils;
using System;

namespace CryptoTools.Cryptography.Guids
{
	public class DotNetGuidAlgorithm : IGuidAlgorithm
	{
		public string NewGuid()
		{
			Guid guid = Guid.NewGuid();
			string stringGuid = guid.ToString("N");

			return stringGuid;

		}

		public bool Verify(string guid)
		{
			if (string.IsNullOrWhiteSpace(guid))
			{
				return false;
			}
			// Check 32 chars long
			if (guid.Length != 32)
			{
				return false;
			}

			// Alpha numeric
			if (!ValidateUtils.IsAlphaNumeric(guid))
			{
				return false;
			}
			return true;
		}
	}
}
