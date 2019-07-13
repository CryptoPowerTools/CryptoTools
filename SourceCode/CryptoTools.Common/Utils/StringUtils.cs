using System;
using System.Text;

namespace CryptoTools.Common.Utils
{
	public class StringUtils
	{
		public static byte[] TextToBytes(string text)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			return bytes;

		}
		public static string BytesToText(byte[] bytes)
		{
			string text = Encoding.UTF8.GetString(bytes); ;
			return text;

		}

		public static string BytesToHexString(byte[] bytes)
		{
			StringBuilder builder = new StringBuilder();
			foreach (Byte b in bytes)
			{
				builder.Append(b.ToString("x2"));
			}
			return builder.ToString();
		}

		public static string StringArrayToString(string[] array, bool appendNewLine = false)
		{
			// Concatenate all the elements into a StringBuilder.
			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < array.Length; i++)
			{
				builder.Append(array[i]);
				if (appendNewLine)
				{
					// Append NewLine unless it is the last item
					if (i != array.Length - 1)
						builder.Append(Environment.NewLine);
				}
			}


			//foreach (string value in array)
			//{
			//	builder.Append(value);
			//	if (appendNewLine)
			//	{
			//		builder.Append(Environment.NewLine);
			//	}
			//}
			return builder.ToString();
		}

		public static string[] StringToStringArray(string data)
		{
			string[] lines = data.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			return lines;
		}

	}
}
