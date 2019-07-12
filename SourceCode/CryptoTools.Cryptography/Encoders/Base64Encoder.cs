using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Cryptography.Encoders
{
	/// <summary>
	/// This code was snipped from http://www.millsoftllc.com/2013/03/unique-hash-tiny-code-ported-to-c/
	/// treat the primes as your secret keys and change them up when you implement this code (keep the primes greater than .5*62^n).
	/// </summary>

	public class Base62Encoder
	{
		private static long[][] GoldenPrimes = new long[][]{
			new long[]{1,1},
			new long[]{41,59},
			new long[]{2377,1677},
			new long[]{147299,187507},
			new long[]{9132313,5952585},
			new long[]{566201239,643566407},
			new long[]{35104476161,22071637057},
			new long[]{2176477521929,294289236153},
			new long[]{134941606358731,88879354792675},
			new long[]{8366379594239857,7275288500431249},
			new long[]{518715534842869223,280042546585394647}
		};

		/* Ascii : 0  9, A  Z, a  z */
		private static int[] Chars64 = new int[]
		{
			48,49,50,51,52,53,54,55,56,57,65,
			66,67,68,69,70,71,72,73,74,75,
			76,77,78,79,80,81,82,83,84,85,
			86,87,88,89,90,97,98,99,100,101,
			102,103,104,105,106,107,108,109,110,
			111,112,113,114,115,116,117,118,119,
			120,121,122
		};

		public static string ToBase62(BigInteger i)
		{
			List<char> key = new List<char>();

			while (BigInteger.Compare(i, 0) > 0)
			{
				key.Add((char)Chars64[(int)(i % 62)]);
				i = BigInteger.Divide(i, 62);
			}

			key.Reverse();
			return new string(key.ToArray());
		}

		public static string ToHash(long num, int len = 5)
		{
			BigInteger dec = BigInteger.Multiply(num, GoldenPrimes[len][0]) % BigInteger.Pow(62, len);
			return ToBase62(dec).PadLeft(len, '0');
		}


		public static long FromBase62(string key)
		{
			long val = 0;

			for (int i = key.Length - 1; i >= 0; i--)
			{
				val += Array.IndexOf(Chars64, (int)key[i])
					* (long)Math.Pow(62, key.Length - 1 - i);
			}
			return val;
		}

		public static long FromHash(string hash)
		{
			return (long)(
				BigInteger.Multiply(FromBase62(hash), GoldenPrimes[hash.Length][1])
					% BigInteger.Pow(62, hash.Length));
		}
	}


}

