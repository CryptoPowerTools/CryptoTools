using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoTools.Cryptography.Utils;

namespace CryptoTools.Cryptography.UnitTests.Utils
{
	[TestClass]
	public class CryptoRandomizerTests
	{
		[TestMethod]
		public void CryptoRandomizer_BasicUsage()
		{
			CryptoRandomizer randomizer = new CryptoRandomizer();
			int blockSize = 20000;
			int iterations = 50000;

			for (int i = 0; i < iterations; i++)
			{
				// Get some random bytes
				byte[] bytes = new byte[blockSize];
				randomizer.GetBytes(bytes);
				Assert.IsTrue(bytes.Length == blockSize);

				// Get Random Int
				int randomInt = randomizer.Next();
				// Not really testible

				// Get Random Int less than 5
				int randomIntLess5 = randomizer.Next(5);
				Assert.IsTrue(randomIntLess5 < 5);

				// Get Random Int between 1000 - 2000
				int randomIntBetween1000And1500 = randomizer.Next(1000, 2000);
				Assert.IsTrue(randomIntBetween1000And1500 > 999 && randomIntBetween1000And1500 < 20001);

				double randomDouble = randomizer.NextDouble();
				// Not really testible
			}
		}
	}
}
