using System.Linq;

namespace CryptoTools.Common.Utils
{
    public class ValidateUtils
	{
		public static bool IsAlphaNumeric(string input)
		{
			return input.All(char.IsLetterOrDigit);
		}
	}
}
