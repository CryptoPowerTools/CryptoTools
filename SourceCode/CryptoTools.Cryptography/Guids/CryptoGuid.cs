using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Cryptography.Guids
{
	/*
Guid    1e82e39fa9e64d50af1d91821d88ee8a

base58		4KpVnNYsVyKVhbxkSb5EUp4V618pNwR3wuVejMnCzk3i
sha1		327fbf07189ea56d4b893fa60a434aa674cfd696
sha256		45db66ba4d4bf9b1c5e9149b46719dbaee020ad35591c4a7fffd66be217be8fe
sha512      d84ff2798c0c9f923c9a8f3ccbe33f5d62dc5661003d211abb64429346ef67e259aae07a89f711ad25da90e3629767df0c2920cd3031fead22631e10b688072f
ripeMD128	9a3fe339215ef90af38f5917d957432e

MD5			98de690064c467759f0ec08063f13f79


CRC16       ec38
CRC32		79460a47






UID1 = [U01][SHA1][CRC16] = U1327fbf07189ea56d4b893fa60a434aa674cfd696ec38

UID2 = [U02]
	[SHA256]
	[CRC16] = U245db66ba4d4bf9b1c5e9149b46719dbaee020ad35591c4a7fffd66be217be8feec38

UID3 = [U03][SHA256][SHA256 - Prefix] = U345db66ba4d4bf9b1c5e9149b46719dbaee020ad35591c4a7fffd66be217be8feXXXX

UID4 = [U04](SHA1->Base58)(CRC16->Base58) = U44KpVnNYsVyKVhbxkSb5EUp4V618pNwR3wuVejMnCzk3i3bK535

*/
	public class CryptoGuid
	{

		private static IGuidAlgorithm DefaultAlgorithm = new DotNetGuidAlgorithm();

		public static bool Verify(string guid)
		{
			return DefaultAlgorithm.Verify(guid);
		}

		private static bool DefaultAlgorithmSet = false;

		public static bool EnforceSingleSetDefaultAlgorithm { get; set; } = true;

		public static string NewGuid()
		{
			return DefaultAlgorithm.NewGuid();
		}

		public static void SetDefaultAlgorithm(IGuidAlgorithm guidAlgorithm)
		{
			if (DefaultAlgorithmSet && EnforceSingleSetDefaultAlgorithm)
				throw new Exception("SetDefaultAlgorithm can only be called a single time. This is usually called in the application inialization.");

			DefaultAlgorithm = guidAlgorithm;
			DefaultAlgorithmSet = true;
		}


	}
}
