using System;

namespace CryptoTools.Cryptography.Exceptions
{
	public class CryptoException : Exception
	{
		public CryptoException(string message = "", Exception innerException = null) : base(message, innerException)
		{


		}
	}

	public class CryptoDecryptionException : CryptoException
	{

		public CryptoDecryptionException(string message = "", Exception innerException = null) : base(message, innerException)
		{

		}
	}


	public class CryptoBlobChecksumFailedException : CryptoException
	{

		public CryptoBlobChecksumFailedException(string message = "", Exception innerException = null) : base(message, innerException)
		{

		}
	}


}
