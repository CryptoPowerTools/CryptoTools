using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Common.Logging
{
	public class LoggingException : Exception
	{
		public LoggingException(string message = "") : base(message)
		{
		}
	}	
}
