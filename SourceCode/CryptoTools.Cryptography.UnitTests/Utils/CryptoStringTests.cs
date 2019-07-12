using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoTools.Cryptography.Utils;
using System.Security;
using CryptoTools.Common.FileSystems;

namespace CryptoTools.Cryptography.UnitTests.Utils
{
	[TestClass]
	public class CryptoStringTests
	{
		[TestMethod]
		public void CryptoString_BasicUsuage()
		{
			string password = "MyPassword";
			SecureString secureString= CryptoString.StringToSecureString(password); // Only used for test
			string decryptedString;

			CryptoString crypto = new CryptoString(secureString);

			// Make sure the ToString DOES NOT return the string
			decryptedString	= crypto.ToString();
			Assert.IsFalse(decryptedString.Equals(password));

			// Use this only if SecureString is not accepted
			string useString = CryptoString.SecureStringToString(crypto.GetSecureString());

			////////////////////////////////////////////
			// You can now use the useString variable
			////////////////////////////////////////////
			// When you done fill the variable full of random Text
			// NOTE: Not totally secure, but an added level of obsfucation
			useString = CryptoString.GenerateRandomText(10000);
			

		}


		[TestMethod]
		public void CryptoString_TestUtilityMethods()
		{
			string password = "MyPassword";
			int length = 100;

			string random = CryptoString.GenerateRandomText(length);
			Assert.IsTrue(random.Length > (length / 2));

			string byteString = CryptoString.BytesToString(new ByteGenerator().GenerateBytes(length));
			Assert.IsTrue(byteString.Length > 0);


			// String conversions
			SecureString secureString = CryptoString.StringToSecureString(password);
			string unsecureString = CryptoString.SecureStringToString(secureString);
			Assert.IsTrue(password.Equals(unsecureString));
			
		}
	}
}
