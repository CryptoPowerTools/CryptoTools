using System;
using System.Text;

namespace CryptoTools.Common.FileSystems
{
	/// <summary>
	/// Generates Random Words. This is useful for various encryption operations or even unit tests.
	/// </summary>
	public class TextGenerator
	{
		const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // abcdefghijklmnopqrstuvwxyz";

		public int ApproxLineLength { get; set; } = 25;
		public int ApproxWordLength { get; set; } = 11;


		public TextGenerator()
		{
		}


		public string GenerateText(int length)
		{
			Random random = new Random();
			StringBuilder result = new StringBuilder(length);
			int lengthCounter = 0;
			for (int i = 0; i < length; i++)
			{
				// Make a word.
				string word = "";
				int wordLength = random.Next(ApproxWordLength + 3);
				for (int j = 1; j <= wordLength; j++)
				{
					// Pick a random number between 0 and 25
					// to select a letter from the letters array.
					int letterNumber = random.Next(0, Characters.Length - 1);

					// Append the letter.
					word += Characters[letterNumber];
				}
				result.Append($"{word} ");


				lengthCounter += wordLength;

				if (lengthCounter > ApproxLineLength)
					result.Append(Environment.NewLine);

			}
			return result.ToString();
		}
	}
}
