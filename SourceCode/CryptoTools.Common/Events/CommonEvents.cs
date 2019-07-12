using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
