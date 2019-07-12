using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Cryptography.Utils
{

	/// <summary>
	/// Cryptography Secure Random Number Generator using the .NET Cryptography classes for optimum security. The class provides several 
	/// easy to use methods for easy integration into existing code.
	/// </summary>
	public class CryptoRandomizer
	{
		private RandomNumberGenerator _rng;

		/// <summary>
		/// Creates an instance of the default implementation of a cryptographic random number generator that can be used to generate random data.
		/// </summary>
		public CryptoRandomizer()
		{
			_rng = RandomNumberGenerator.Create();
		}

		/// <summary>
		/// Fills the elements of a specified array of bytes with random numbers.
		/// </summary>
		/// <param name="bytes"></param>
		public void GetBytes(byte[] bytes)
		{
			_rng.GetBytes(bytes);
		}

		/// <summary>
		/// Returns a random number between 0.0 and 1.0.
		/// </summary>
		/// <returns></returns>
		public double NextDouble()
		{
			byte[] b = new byte[4];
			_rng.GetBytes(b);
			return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
		}

		/// <summary>
		/// Returns a random number within the specified range.
		/// </summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public int Next(int minValue, int maxValue)
		{
			//return (int)Math.Round(NextDouble() * (maxValue – minValue – 1)) + minValue;
			return (int)Math.Round(NextDouble() * ((maxValue-minValue) - 1)) + minValue;
		}

		/// <summary>
		/// Returns a nonnegative random number.
		/// </summary>
		/// <returns></returns>
		public int Next()
		{
			return Next(0, Int32.MaxValue);
		}

		/// <summary>
		///  Returns a nonnegative random number less than the specified maximum
		/// </summary>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public int Next(int maxValue)
		{
			return Next(0, maxValue);
		}
	}
}



