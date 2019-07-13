using System;

namespace CryptoTools.Common.Utils
{
	public class Randomizer
	{
		private static Random instance = new Random();



		public static Random Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Random();
				}
				return instance;
			}
		}

		public static int GetInt(int minVal, int maxVal)
		{
			return Instance.Next(minVal, maxVal);
		}

		public static void GetBytes(byte[] bytes)
		{
			Instance.NextBytes(bytes);
		}
	}
}
