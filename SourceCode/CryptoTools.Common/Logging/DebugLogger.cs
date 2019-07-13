using System;
using System.
/* Unmerged change from project 'CryptoTools.Common (net461)'
Before:
using System.Linq;
After:
using System.Diagnostics;
using System.Linq;
*/
Runtime.CompilerServices;
/* Unmerged change from project 'CryptoTools.Common (net461)'
Before:
using System.Threading.Tasks;
using System.Diagnostics;
After:
using System.Threading.Tasks;
*/


namespace CryptoTools.Common.Logging
{
	/// <summary>
	/// The Default Logger for the Logger component that has no dependencies on 3rd party logging frameworks. In a realife application
	/// it would be more practical to create a simple ILog implementation of your favorite logging framework such as Log4Net, NLog etc.
	/// I have created wrappers for most common frameworks so just let me know and I can provide. I will eventually post these on GitHub.
	/// </summary>
	public class DebugLogger : ILog
	{
		public bool IsDebugEnabled
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsErrorEnabled
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsFatalEnabled
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsInfoEnabled
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsWarnEnabled
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public void Debug(object message)
		{
			DebugWrite($"DEBUG - {message}");
		}

		public void Debug(object message, Exception exception)
		{
			DebugWrite(message);
		}

		public void Error(object message)
		{
			DebugWrite($"ERROR - {message}");
		}

		public void Error(string message, object thisObject, [CallerMemberName] string methodName = "")
		{
			DebugWrite($"ERROR - {message}");
		}

		public void ErrorException(Exception exception, object thisObject, [CallerMemberName] string methodName = "")
		{
			DebugWrite($"ERROR - {exception.Message}");
		}

		public void Fatal(object message)
		{
			DebugWrite($"FATAL - {message}");
		}

		public void Fatal(string message, object thisObject, [CallerMemberName] string methodName = "")
		{
			DebugWrite($"FATAL - {message}");
		}

		public void FatalException(Exception exception, object thisObject, [CallerMemberName] string methodName = "")
		{
			DebugWrite($"FATAL - {exception.Message}");
		}

		public void Info(object message)
		{
			DebugWrite($"INFO - {message}");
		}

		public void Info(object message, Exception exception)
		{
			DebugWrite($"INFO - {message}");
		}

		public void NotificationHighestPriority(object message)
		{
			DebugWrite($"NOTIFICATION - {message}");
		}

		public void NotificationHighPriority(object message)
		{
			DebugWrite($"NOTIFICATION - {message}");
		}

		public void NotificationLowPriority(object message)
		{
			DebugWrite($"NOTIFICATION (Low) - {message}");
		}

		public void Warn(object message)
		{
			DebugWrite($"WARN - {message}");
		}

		public void Warn(object message, Exception exception)
		{
			DebugWrite($"WARN - {message}");
		}


		private void DebugWrite(object message)
		{
			System.Diagnostics.Debug.WriteLine(message);
		}

		public ILog CreateInstance(Type type)
		{
			ILog instance = new DebugLogger();
			return instance;
		}
	}
}
