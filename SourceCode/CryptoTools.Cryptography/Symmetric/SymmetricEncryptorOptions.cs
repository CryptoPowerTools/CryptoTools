using CryptoTools.Common.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Cryptography.Symmetric
{
	/// <summary>
	/// Hasher Options allows you to customize eash instance of the Hasher as required.
	/// </summary>
	public class SymmetricEncryptorOptions
	{
		public bool? HashFingerprintLowercase { get; set; }
		public string Salt { get; set; }
		public string IVSalt { get; set; }
		public string KeySalt { get; set; }
		public string InitializationVector { get; set; }
	
		public SymmetricEncryptorOptions()
		{
		}

		public static SymmetricEncryptorOptions CreateMergedInstance(SymmetricEncryptorOptions options = null)
		{
			// Merge Options with Global Options
			SymmetricEncryptorOptions result = DefaultOptionsBuilder.MergeOptions< SymmetricEncryptorOptions>(GlobalCryptographyOptions.SymmetricEncryptorOptions, options);
			return result;
		}
	}

}
