using System;

namespace CryptoTools.Common.Logging
{
	public class LoggingException : Exception
	{
		public LoggingException(string message = "") : base(message)
		{
		}
	}
}
