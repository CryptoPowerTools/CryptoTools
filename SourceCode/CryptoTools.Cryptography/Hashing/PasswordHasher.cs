using CryptoTools.Cryptography.Utils;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace CryptoTools.Cryptography.Hashing
{
	/// <summary>
	/// Creates Secure Password Hashes using the .NET Cryptography Api. The PasswordHash method uses a Salt and Pepper that is configured in the options object.
	/// When you call the PasswordCheck method it will handle building up the Salt & Pepper for you. 
	/// </summary>
	public class PasswordHasher
	{
		#region Private Fields
		// This should be refactored with the Encrypted generator
		private readonly CryptoRandomizer random = new CryptoRandomizer();
		private readonly Hasher _hasher;

		// Create a small delay which increase the iteration count for additional brute force protection
		private const int SecureHashDelay = 10;
		#endregion

		#region Public Properties
		public PasswordHasherOptions Options { get; private set; }
		#endregion


		#region Constructors
		/// <summary>
		/// Creates a PaswordHasher with the default settings of SHA256 algorithm and default options
		/// </summary>
		public PasswordHasher() : this(null, null) { }

		/// <summary>
		/// Creates a PaswordHasher with custom options.
		/// </summary>
		/// <param name="options"></param>
		public PasswordHasher(PasswordHasherOptions options) : this(null, options) { }

		/// <summary>
		/// Creates a PaswordHasher with a specified algorithm and custom options
		/// </summary>
		/// <param name="algorithm"></param>
		/// <param name="options"></param>
		public PasswordHasher(HashAlgorithm algorithm = null, PasswordHasherOptions options = null)
		{
			_hasher = algorithm != null ? new Hasher(algorithm) : new Hasher(SHA256.Create());
			Options = PasswordHasherOptions.CreateMergedInstance(options);
		}
		#endregion





		/// <summary>
		/// Hashes a Password and returns a hash fingerprint
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public string PasswordHash(CryptoString password)
		{
			Hasher hasher = new Hasher();
			string pepper;
			string hash;

			// Generate Pepper
			pepper = new string(Enumerable.Repeat(Options.PepperChars, 1).Select(s => s[random.Next(s.Length)]).ToArray());

			// Create your hash
			hash = hasher.SecureHash(CryptoString.SecureStringToString(password.GetSecureString()) + Options.Salt + pepper, SecureHashDelay);

			return hash;
		}

		/// <summary>
		/// Checks a password against its hash to validate if it matches. The check automagically handles incorporating the Salt and even checks 
		/// against all the Pepper value as well. As a User you just pass in the password and Hash and let the method do the work for you.
		/// </summary>
		/// <param name="password"></param>
		/// <param name="hash"></param>
		/// <returns></returns>
		public bool PasswordCheck(CryptoString password, string hash)
		{
			Hasher hasher = new Hasher();
			bool isValid = false;
			int iterations = 0;

			foreach (var c in Options.PepperChars.ToArray())
			{
				iterations++;
				string attemptPassword = password + Options.Salt + c;
				string attemptHash = hasher.SecureHash(CryptoString.SecureStringToString(password.GetSecureString()) + Options.Salt + c, SecureHashDelay);

				// Check if you got a hit
				if (hash.Equals(attemptHash))
				{
					// You got a hit so the Password is Valid
					isValid = true;
					Debug.WriteLine($"Found a hit after {iterations} iterations.");
					break;
				}
			}
			return isValid;
		}
	}
}
