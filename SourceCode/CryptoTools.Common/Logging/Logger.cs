using System;
using System.Runtime.CompilerServices;

namespace CryptoTools.Common.Logging
{
	#region Interface
	/// <summary>
	/// Interface to expose our log object. This allows us to easily change the implementation as requirements change
	/// without modifying any application code! Also note, i do not expose all the native logger methods, this will force
	/// the developer to standardize on a method to log in the app for consistency.
	/// </summary>
	public interface ILog
	{
		bool IsDebugEnabled { get; }
		bool IsInfoEnabled { get; }
		bool IsWarnEnabled { get; }
		bool IsErrorEnabled { get; }
		bool IsFatalEnabled { get; }
		void Debug(object message);
		void Debug(object message, Exception exception);
		void Info(object message);
		void Info(object message, Exception exception);
		void Warn(object message);
		void Warn(object message, Exception exception);
		void Error(object message);
		void Error(string message, object thisObject, [CallerMemberName] string methodName = "");
		void ErrorException(Exception exception, object thisObject, [CallerMemberName] string methodName = "");
		void Fatal(object message);
		void Fatal(string message, object thisObject, [CallerMemberName] string methodName = "");
		void FatalException(Exception exception, object thisObject, [CallerMemberName] string methodName = "");

		// Notifications
		void NotificationLowPriority(object message);
		void NotificationHighPriority(object message);
		void NotificationHighestPriority(object message);

		// Creates an instance of the logger
		ILog CreateInstance(Type type);
	}
	#endregion

	/// <summary>
	/// Logger creates an instance of a logger for your application to use. In general, your class will create a single instance of the log and
	/// assign it to the ILog private field. As a convention, it is good practice to standardize the naming convention for getting a logger. 
	/// </summary>	
	/// <example>
	/// 1). ILog Log = Logger.GetInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
	/// OR
	/// 2). ILog Log = Logger.GetInstance(typeof(ClassName));
	/// 
	/// The first example has the benefit of cut and paste code to any class with no changes, but might take a 'small' performance
	/// hit, since it uses reflection. The second method is more efficient, but of course you must ensure the magic string class name 
	/// is correct.
	/// </example>
	public class Logger
	{
		public static ILog _logger;

		#region Public Static Methods
		/// <summary>
		/// Gets a logger for a class to use. In practice you can use reflection to reflect the Type of type parameter, however, if you require absolute performance,
		/// for example in a class that is created very often you can use the typeof() operator to get the class name.
		/// </summary>
		/// <example>
		/// ILog Log = Logger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		/// OR
		/// ILog Log = Logger.GetLogger(typeof(ClassName));
		/// </example>
		/// <param name="type"></param>
		/// <returns></returns>
		public static ILog CreateInstance(Type type)
		{
			if (_logger == null)
			{
				_logger = new DebugLogger();
			}

			// Creates a new instance of a logger for each logger
			ILog instance = _logger.CreateInstance(type);

			return instance;
		}

		public static void SetLogger(ILog logger)
		{
			_logger = logger;
		}
		#endregion
	}
}
