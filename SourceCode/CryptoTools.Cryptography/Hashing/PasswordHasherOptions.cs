﻿using CryptoTools.Common.Reflection;

namespace CryptoTools.Cryptography.Hashing
{
	/// <summary>
	/// Hasher Options allows you to customize eash instance of the Hasher as required.
	/// </summary>
	public class PasswordHasherOptions
	{
		public string Salt { get; set; }
		public string PepperChars { get; set; }

		public PasswordHasherOptions()
		{
		}

		public static PasswordHasherOptions CreateMergedInstance(PasswordHasherOptions options = null)
		{
			PasswordHasherOptions result = DefaultOptionsBuilder.MergeOptions<PasswordHasherOptions>(GlobalCryptographyOptions.PasswordHasherOptions, options);
			return result;
		}
	}

}
