using CryptoTools.Cryptography.Exceptions;
using CryptoTools.Cryptography.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Cryptography.Utils
{



	public class CryptoCredentialsException : CryptoException
	{
		public CryptoCredentialsException(string message) : base(message)
		{
		}
	}
	public class CryptoCredentialsNullException : CryptoException
	{
		public CryptoCredentialsNullException(Type type, string message = null) : base($"CryptoCredentials have not been set for object of type {type.Name}. {message}")
		{			
		}
	}
	


	public class CryptoCredentials
	{

		const string ReadOnlyExceptionMessage = "{0} cannot be set.Credentials have been set to ReadOnly.Once ReadOnly is set you CANNOT unset it for security.You must recreate the CryptoCredentials object.";


		// Ideas
		// MultiPassphrases
		// 

		#region Private Fields
		private int _pin;
		private CryptoString _passphrase;
		private bool _readOnly = false;
		#endregion





		public CryptoCredentials()
		{
		}

		public CryptoCredentials MakeReadOnly()
		{
			_readOnly = true;
			return this;
		}

		public bool UsePassphrase { get; set; } = false;
		public bool UsePin { get; set; } = false;



		#region Public Properties

		/// <summary>
		/// Passphrase (or password) that can contain words or a sentance that can be easily remembered by a user. This can also be a short password
		/// and there is nothing to prevent you from creating even a single word. It is up to the application to enforce any rules on the password
		/// length special characters etc. All Whitespace will be striped off the beginning and ending of the string, but may contain strings within
		/// the passphrase.
		/// </summary>
		public CryptoString Passphrase
		{
			get
			{
				return _passphrase;
			}
			set
			{
				if (_readOnly) throw new CryptoCredentialsException(string.Format(ReadOnlyExceptionMessage, nameof(Passphrase)));
				
				_passphrase = value;
				if (value == null)
				{
					UsePassphrase = false;
				}
				else
				{
					UsePassphrase = true;
				}
			}
		}

		/// <summary>
		/// Pin can be used as an extra level of security. Pin must be and Int32 number and only in 4 digit format in the range 1000 - 9999.
		/// This is an optional credential.
		/// </summary>
		public int Pin
		{
			get
			{
				return _pin;
			}
			set
			{
				if (_readOnly) throw new CryptoCredentialsException(string.Format(ReadOnlyExceptionMessage, nameof(Pin)));
				if (!IsPinValid(value))
				{
					throw new CryptoCredentialsException("Invalid Pin. The Pin must be a number between 1000 - 9999 or 0 indicating no Pin");
				}
				_pin = value;

				if (value == 0)
				{
					UsePin = false;
				}
				else
				{
					UsePin = true;
				}
			}		
		}

		/// <summary>
		/// the Key is the Read-Only Hash of all the credentials. The Key is really to main component that will be used in the encryption apis.
		/// </summary>
		public string Key
		{
			get
			{
				// We can tweak the algorithms as require, but once stable it should NOT 
				// change since it will break existing generated encryption
				Hasher hasher = new Hasher();

				// This routine arranges hashes in a specific order. If this changes it will 
				//		break any existing encryption hashes.
				string compoundString = "";
				compoundString += UsePassphrase ? Passphrase.GetStringHash() : "";
				compoundString += UsePin ? hasher.Hash(Pin) : "";

				// Now Create the Final Key based on all the credentials
				string key = hasher.Hash(compoundString);
				return key;
			}
		}
		#endregion

		public bool IsPinValid(int pin)
		{
			if (pin == 0)
				return true;
			if (pin >= 1000 && pin <= 9999)
				return true;

			return false;
		}		
	}
}
