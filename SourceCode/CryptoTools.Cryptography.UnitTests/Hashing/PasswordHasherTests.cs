using CryptoTools.Cryptography.Hashing;
using CryptoTools.Cryptography.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security;
using System.Security.Cryptography;

namespace CryptoTools.Cryptography.UnitTests.Hashing
{
	[TestClass]
	public class PasswordHasherTests
	{
		[TestMethod]
		public void PasswordHasher_BasicUsage()
		{
			Hasher sha256 = new Hasher(SHA256.Create());

			// Create password -> SecureString -> CryptpString
			string password = "My Passord";
			SecureString securePassword = CryptoString.StringToSecureString(password);
			CryptoString cryptoPassword = new CryptoString(securePassword);

			// Create the Hash
			PasswordHasher hasher = new PasswordHasher();
			string hash = hasher.PasswordHash(cryptoPassword);

			// FAIL - BAD Hash 
			string badHash = sha256.Hash("randomhash203984vb5230984vb230984tb7v23b098tu13908tu31908vn");
			Assert.IsFalse(hasher.PasswordCheck(cryptoPassword, badHash));

			// FAIL - BAD Password
			CryptoString badPassword = new CryptoString(CryptoString.StringToSecureString("BADPASSWORD"));
			Assert.IsFalse(hasher.PasswordCheck(badPassword, hash));

			// SUCCESS - GOOD Password and Hash
			Assert.IsTrue(hasher.PasswordCheck(cryptoPassword, hash));

		}

		[TestMethod]
		public void PasswordHasher_BasicUsageWiki()
		{
			// Create password -> SecureString -> CryptpString
			string password = "My Passord";
			PasswordHasher hasher = new PasswordHasher();
			SecureString securePassword = CryptoString.StringToSecureString(password);
			CryptoString cryptoPassword = new CryptoString(securePassword);

			// Create the Hash
			string hash = hasher.PasswordHash(cryptoPassword);

			// Check the password against the hash, salt and pepper
			bool valid = hasher.PasswordCheck(cryptoPassword, hash);

		}
	}
}
