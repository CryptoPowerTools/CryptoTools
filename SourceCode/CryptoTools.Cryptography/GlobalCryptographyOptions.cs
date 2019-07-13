using CryptoTools.Cryptography.Hashing;
using CryptoTools.Cryptography.Symmetric;

namespace CryptoTools.Cryptography
{
	public static class GlobalCryptographyOptions
	{
		public static HasherOptions HasherOptions { get; private set; }
		public static PasswordHasherOptions PasswordHasherOptions { get; private set; }
		public static SymmetricEncryptorOptions SymmetricEncryptorOptions { get; private set; }

		//public static AsymmetricOptions AsymmetricOptions { get; private set; }

		static GlobalCryptographyOptions()
		{
			ResetDefaults();
		}

		/// <summary>
		/// Resets all options back to default. This should generally not be called after the application sets Application specific options.
		/// </summary>
		public static void ResetDefaults()
		{

			//////////////////////////////////////////////////////////
			// Hasher Initialize
			//////////////////////////////////////////////////////////
			HasherOptions = new HasherOptions()
			{
				HashFingerprintLowercase = true,
				Salt = "GLOBALSALT_902834vtn029384ytv20384tyvb13084tyv1b08ty1084vn",
				UseSalt = false
			};

			//////////////////////////////////////////////////////////
			// PasswordHasher Initialize
			//////////////////////////////////////////////////////////
			PasswordHasherOptions = new PasswordHasherOptions()
			{
				// NOTE: Changing these Value will break existing encrypted data. 
				//		 These are defaults only. It is recommended that each Application  
				//		create a custom Salt for individual security
				Salt = "6c5573acf3177dd019aa3cc3349349370d27b472744a9e5ed7a0385f686cdbb3fdba9d79b88a0b2a500543d5375a20e25177cf65c493f7a9a65dab85c9d71bab",
				PepperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqurtuvwxyz0123456789!@#$%^&*()_+?><:"

			};

			//////////////////////////////////////////////////////////
			// SymmetricEncryptorOptions Initialize
			//////////////////////////////////////////////////////////
			SymmetricEncryptorOptions = new SymmetricEncryptorOptions()
			{
				// NOTE: Changing these Value will break existing encrypted data. 
				//		 These are defaults only. It is recommended that each Application  
				//		create a custom Salt for individual security
				Salt = "13c035d5c0b55becec39f3cf6c8adc81c5db5818f8e4fc537d5cccd805df2b5bfb22c43d6846e50b364794d2784fbb90922e9c62882f464385162186f6035168",
				HashFingerprintLowercase = true,
				InitializationVector = "29C7B95FA4B76027B183EF75A10325D40AFF4FA5D444C45BEE714CBFDF92EE6C1D5E0D82D190B714AD5EEF0AC947C19596DF460F6F154C4A0C85EF39F95A2F8E",
				KeySalt = "72E7D56BAD8C4377C1E57E29EEE9BB0AD218D898EA3714F175702AD5D0E507873257B5B429624A8E406435CC8BBDD89194F5E0A42E92C5FA061D881B972FC56B",
				IVSalt = "F755FC686D4F2DA387D54F92B9C70CCF1B267B525A5DED28B8504C03A56E1B47EA33BFDC3C241041469AA28C0732A66671D7A8A51FCEBD6FE3B72758CDC2EC63",



			};
		}
	}
}
