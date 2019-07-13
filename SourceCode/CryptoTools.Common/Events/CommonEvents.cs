using System;

namespace CryptoTools.Common.Events
{

	public class CommonEvents
	{

		public event EventHandler<LogEventArgs> LogEvent;

		public void FireLogEvent(object source, LogEventArgs args)
		{
			LogEvent?.Invoke(source, args);
		}


	}
}
