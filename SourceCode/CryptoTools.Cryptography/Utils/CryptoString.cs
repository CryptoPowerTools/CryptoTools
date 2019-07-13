using CryptoTools.Common.FileSystems;
using CryptoTools.Cryptography.Hashing;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace CryptoTools.Cryptography.Utils
{
	/// <summary>
	/// Crypto Safe String that should be used for all sensitive data to be used in your application. Although this is not
	/// a 100% safe assurance that a memory dump will not reveal the sensitive string, but using this class through all layers
	/// will provide much less surface area for a hacker to attack. Internally the string is stored as a .NET Secure string for
	/// additional safety.
	/// </summary>
	public class CryptoString
	{
		#region Private Fields
		private SecureString _secureString;
		#endregion

		#region Constructors
		public CryptoString(SecureString secureString)
		{
			_secureString = secureString;
		}

		public CryptoString(string passphrase) : this(CryptoString.StringToSecureString(passphrase))
		{
		}

		public SecureString GetSecureString()
		{
			return _secureString;
		}
		#endregion

		public string GetStringHash(HashAlgorithm algorithm = null)
		{
			HashAlgorithm a = algorithm == null ? SHA256.Create() : algorithm;
			Hasher hasher = new Hasher(a);
			string hash = hasher.Hash(CryptoString.SecureStringToString(GetSecureString()));
			return hash;
		}

		#region Useful Public Static Methods

		/// <summary>
		/// Generate Random Text to assing to the value of a 'string' for added obsfucation in memory. 
		/// This is not a 100% secure way to overwright the memory, but a good practice when possible
		/// </summary>
		/// <param name="approximateLength"></param>
		/// <returns></returns>
		public static string GenerateRandomText(int approximateLength)
		{
			ByteGenerator generator = new ByteGenerator();
			byte[] bytes = generator.GenerateBytes(approximateLength / 2);
			string result = BytesToString(bytes);
			return result;
		}

		/// <summary>
		/// Helper method to convert a byte array to a hexidecial readable string
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string BytesToString(byte[] bytes)
		{
			StringBuilder builder = new StringBuilder();
			foreach (Byte b in bytes)
			{
				builder.Append(b.ToString("x2"));

			}
			return builder.ToString();
		}

		/// <summary>
		/// Converts a .NET SecureString into a CryptoString.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SecureString StringToSecureString(string value)
		{
			SecureString secure = new SecureString();
			foreach (char c in value)
			{
				secure.AppendChar(c);
			}
			value = CryptoString.GenerateRandomText(10000);
			return secure;
		}

		/// <summary>
		/// Converts a .NET string into a CryptoString. It is recommended for the application to assign the 'string' argument
		/// with Random Text using the GenerateRandomText() method to overwrite the original string.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static CryptoString StringToCryptoString(string value)
		{
			CryptoString cryptoString = new CryptoString(StringToSecureString(value));
			value = GenerateRandomText(10000);
			return cryptoString;
		}

		/// <summary>
		/// Converts a Secure String back to a .NET string. The returned value will be insecure, so only call this method
		/// if you absolutely need to convert.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String SecureStringToString(SecureString value)
		{
			IntPtr valuePtr = IntPtr.Zero;
			try
			{
				valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
				return Marshal.PtrToStringUni(valuePtr);
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
			}
		}
		#endregion
	}
}
