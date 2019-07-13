using CryptoTools.Common.Utils;

namespace CryptoTools.Cryptography.Hashing
{
	/// <summary>
	/// Hasher Options allows you to customize eash instance of the Hasher as required.
	/// </summary>
	public class HasherOptions
	{
		public bool? HashFingerprintLowercase { get; set; }
		public string Salt { get; set; }
		public bool UseSalt { get; set; }

		public HasherOptions()
		{
		}

		public static HasherOptions CreateMergedInstance(HasherOptions options = null)
		{
			// Merge Options with Global Options
			HasherOptions globalOptions = GlobalCryptographyOptions.HasherOptions.MemberwiseClone() as HasherOptions;
			HasherOptions result = options == null ? globalOptions : ObjectUtils.MergeObjects(options, globalOptions) as HasherOptions;
			return result;
		}
	}

}
