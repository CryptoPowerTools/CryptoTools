using System;

namespace CryptoTools.Common.Events
{

	public enum LogEventType
	{
		Information,
		Debug,
		Warning,
		Error
	};

	public class LogEventArgs : EventArgs
	{
		public string Message { get; private set; }
		public DateTime Time { get; private set; }
		public LogEventType Type { get; private set; }
	}
}

